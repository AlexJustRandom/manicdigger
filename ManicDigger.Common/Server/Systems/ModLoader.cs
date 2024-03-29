using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using ManicDigger.Common;
using Microsoft.CSharp;

namespace ManicDigger.Server
{
    public class ServerSystemModLoader : ServerSystem
    {
        public ServerSystemModLoader()
        {
            jintEngine.DisableSecurity();
            jintEngine.AllowClr = true;

            #if DEBUG
                modlodingDebug = true;
            #endif
        }

        bool started;
        Server server;
        public bool modlodingDebug = false;
        public override void Update(Server _server, float dt)
        {
            if (!started)
            {
                started = true;
                server = _server;
                LoadMods(server, false);
            }
        }

        public override bool OnCommand(Server server, int sourceClientId, string command, string argument)
        {
            if (command == "mods")
            {
                RestartMods(server, sourceClientId);
                return true;
            }
            return false;
        }

        public bool RestartMods(Server server, int sourceClientId)
        {
            if (!server.PlayerHasPrivilege(sourceClientId, ServerClientMisc.Privilege.restart))
            {
                server.SendMessage(sourceClientId, string.Format(server.language.Get("Server_CommandInsufficientPrivileges"), server.colorError));
                return false;
            }
            server.SendMessageToAll(string.Format(server.language.Get("Server_CommandRestartModsSuccess"), server.colorImportant, server.GetClient(sourceClientId).ColoredPlayername(server.colorImportant)));
            server.ServerEventLog(string.Format("{0} restarts mods.", server.GetClient(sourceClientId).playername));

            server.modEventHandlers = new ModEventHandlers();
            for (int i = 0; i < server.systemsCount; i++)
            {
                if (server.systems[i] == null) { continue; }
                server.systems[i].OnRestart(server);
            }

            LoadMods(server, true);
            return true;
        }
         void LoadMods(Server server, bool restart)
        {
            server.modManager = new ModManager1();
            server.modManager.Start(server);
            if (server.serverInitSettings.ModCount == 0) {
                Console.WriteLine("Error no mods selected");
                return; 
            }
            else
            {

                if (modlodingDebug) Console.WriteLine(string.Format("Server Mods to load: {0}", server.serverInitSettings.mods.Length));
                for(int index = 0; index < server.serverInitSettings.mods.Length; index++)
                {
                    CompileMod(new List<ModInformation>(server.serverInitSettings.mods), index);
                }
 
            }
 
        }

        bool CompileMod(List<ModInformation> modsources,int index) {

            //load all dependencis first
            if (loadedMods.ContainsKey(modsources[index].ModID))
                return true;

            if (modsources[index].ModDependencies != null) 
            foreach (var dep in modsources[index].ModDependencies)
            {
                if (loadedMods.ContainsKey(dep))
                    continue;
                bool found=false;
                for(int i = 0 ; i < modsources.Count;i++)
                {
                    if (modsources[i].ModID == dep)

                        found = CompileMod(modsources, i);
                }
                if (!found) {
                    try
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format("Can't load mod {0} because its dependency {1} couldn't be loaded.", modsources[index].ModID, dep));
                       
                    }
                    catch
                    {
                        //This will be the case if the server is running on a headless linux server without X11 installed (previously crashed)
                        Console.WriteLine(string.Format("[Mod error] Can't load mod {0} because its dependency {1} couldn't be loaded.", modsources[index].ModID, dep));
                    }
                    return false;
                }

            }
            if (modlodingDebug) Console.WriteLine(string.Format("Starting compilation of {0}", modsources[index].ModID));
           

            CompileScripts(modsources[index].ModID, ModLodingUtil.GetScriptSources(modsources[index].SrcFolder), false);
            //Load all json as blocks in subfolder
            LoadBlocks(modsources[index]);

            Start(server.modManager, server.modManager.required, modsources[index].ModID);
            loadedMods.Add(modsources[index].ModID, true);
            return true;
        }

        public void LoadBlocks(ModInformation modinfo) {

            IntRef dummy=new IntRef();
            var files = ModLodingUtil.FindBlockDefinitions(modinfo.SrcFolder, dummy);
            foreach(var file in files) {
                server.modManager.LoadBlocks(file);
            }
        }
 
        Jint.JintEngine jintEngine = new Jint.JintEngine();
        Dictionary<string, string> javascriptScripts = new Dictionary<string, string>();

        public void CompileScripts(string modID,Dictionary<string, string> scripts, bool restart)
        {
            CSharpCodeProvider compiler = new CSharpCodeProvider(new Dictionary<String, String> { { "CompilerVersion", "v4.0" } });
            var parms = new CompilerParameters();
            parms.GenerateExecutable = false;
            parms.CompilerOptions = "/unsafe";

#if !DEBUG
            parms.GenerateInMemory = true;
#else
            //Prepare for mod debugging
            //IMPORTANT: Visual Studio breakpoints will not jump into a generatet .cs file
            //Instead, call "System.Diagnostics.Debugger.Break()" to create a breakpoint in the mod-class

            //Generate files to debug
            parms.GenerateInMemory = false;
            parms.IncludeDebugInformation = true;

            //Use a local temp folder
            DirectoryInfo dirTemp = new DirectoryInfo(Path.Combine(new FileInfo(GetType().Assembly.Location).DirectoryName, "ModDebugInfos"));

            //Prepare temp directory
            if (!dirTemp.Exists)
            {
                Directory.CreateDirectory(dirTemp.FullName);
            }
            else
            {
                try
                {
                    //Clear temp files
                    foreach (FileInfo f in dirTemp.GetFiles())
                    {
                        f.Delete();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR in Modloader files delete: " + ex.Message);
                }
            }

            //created locally, this allows the debugger to find the .pdb ? TODO idk if i broke this by moving to /ModAssemblyFiles
            parms.OutputAssembly = Path.Combine(new DirectoryInfo(new FileInfo(GetType().Assembly.Location).DirectoryName).FullName, "ModAssemblyFiles", String.Format("{0}-Mod.dll",modID));
            Directory.CreateDirectory(Path.GetDirectoryName(parms.OutputAssembly));
          
             //generatet .cs files are stored here
            //they are rather important for this debug session, since the .pdb link to them
            parms.TempFiles = new TempFileCollection(dirTemp.FullName, true);
#endif

            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("System.Drawing.dll");
            parms.ReferencedAssemblies.Add("ManicDigger.ScriptingApi.dll");
            parms.ReferencedAssemblies.Add("LibNoise.dll");
            parms.ReferencedAssemblies.Add("protobuf-net.dll");
            parms.ReferencedAssemblies.Add("System.Xml.dll");
            //TODO dont need to link every asembly only required ones 
            foreach(var modAssemble in CompiledAssemblies) {
                parms.ReferencedAssemblies.Add(modAssemble);
            }

            Dictionary<string, string> csharpScripts = new Dictionary<string, string>();
            foreach (var k in scripts)
            {
                if (k.Key.EndsWith(".js"))
                {
                    javascriptScripts[k.Key] = k.Value;
                }
                else
                {
                    csharpScripts[k.Key] = k.Value;
                }
            }
            if (restart)
            {
                // javascript only
                return;
            }

            string[] csharpScriptsValues = new string[csharpScripts.Values.Count];
            int i = 0;
            foreach (var k in csharpScripts)
            {
                csharpScriptsValues[i++] = k.Value;
            }

            {
                CompilerResults results = compiler.CompileAssemblyFromSource(parms, csharpScriptsValues);

  
                int warningsCount = 0;
                int errorCount = 0;
                
                for (int j =0; j < results.Errors.Count; j++) {
                    if (results.Errors[j].IsWarning) {
                        warningsCount++;
                        if (modlodingDebug) Console.WriteLine("----------------------------------WARNING------------------------------------");

                    }
                    else
                    {
                        errorCount++;
                        if (modlodingDebug) Console.WriteLine("-----------------------------------ERROR-------------------------------------");

                    }
                    if (modlodingDebug)
                    {
                        //TODO ORGINAL FILENAME ?
                        Console.WriteLine("Filename:" + errorCount + results.Errors[j].FileName);
                        Console.WriteLine("Line:" + results.Errors[j].Line);
                        Console.WriteLine("Text:" + results.Errors[j].ErrorText);
                        Console.WriteLine("-----------------------------------------------------------------------------");
                    }

                }
                if(errorCount!=0| warningsCount != 0) {
                    if (modlodingDebug)
                    {
                        Console.WriteLine("-----------------------------------------------------------------------------");
                        Console.WriteLine("Compiled with:");
                        Console.WriteLine("Errors:" + errorCount);
                        Console.WriteLine("Warnings:" + warningsCount);
                        Console.WriteLine("-----------------------------------------------------------------------------");
                    }
                }
 
                mods.Add(modID, new Dictionary<string, IMod>());
                if (errorCount == 0)
                {
                    Use(results, modID);
                    return;
                }

            }

            //Error. Load scripts separately.
            Console.WriteLine("Loading scripts separetly");
            //TODO THIS NEVER WORKS
            foreach (var k in csharpScripts)
            {
                CompilerResults results = compiler.CompileAssemblyFromSource(parms, new string[] { k.Value });
                if (results.Errors.Count != 0)
                {
                    try
                    {
                        string errors = "";
                        foreach (CompilerError error in results.Errors)
                        {
                            //mono is treating warnings as errors.
                            //if (error.IsWarning)
                            {
                                //continue;
                            }
                            errors += string.Format("{0} Line:{1} {2}", error.ErrorNumber, error.Line, error.ErrorText);
                        }
                        string errormsg = "Can't load mod: " + k.Key + "\n" + errors;
                        try
                        {
                            System.Windows.Forms.MessageBox.Show(errormsg);
                        }
                        catch
                        {
                        }
                        Console.WriteLine(errormsg);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    continue;
                }
                Use(results, modID);
                
            }
        }

        void Use(CompilerResults results,string id)
        {
            CompiledAssemblies.Add(results.PathToAssembly);
            if (modlodingDebug)
                Console.WriteLine("CompiledAssemblies.Add : {0} : {1}", id, results.PathToAssembly);
            foreach (Type t in results.CompiledAssembly.GetTypes())
            {
                if (typeof(IMod).IsAssignableFrom(t))
                {
                    mods[id][t.Name] = (IMod)results.CompiledAssembly.CreateInstance(t.FullName);
                    if (modlodingDebug)
                        Console.WriteLine("Loaded file from : {0} : {1}",id, t.Name);

                }
            }
        }

        Dictionary<string,Dictionary<string, IMod>> mods = new Dictionary<string, Dictionary<string, IMod>>();
        Dictionary<string, string[]> modRequirements = new Dictionary<string, string[]>();
        Dictionary<string, bool> loaded = new Dictionary<string, bool>();
        Dictionary<string, bool> loadedMods = new Dictionary<string,bool>();
        List<string> CompiledAssemblies = new List<string>();

        public void Start(ModManager m, List<string> currentRequires,string ModId)
        {
            /*
            foreach (var mod in mods)
            {
                mod.Start(m);
            }
            */

            modRequirements.Clear();
            loaded.Clear();

            foreach (var k in mods[ModId])
            {
                k.Value.PreStart(m);
                modRequirements[k.Key] = currentRequires.ToArray();
                currentRequires.Clear();
            }
            foreach (var k in mods[ModId])
            {
                StartMod(ModId,k.Key, k.Value, m);
            }

            StartJsMods(m);
        }

        void StartJsMods(ModManager m)
        {
            jintEngine.SetParameter("m", m);
            // TODO: javascript mod requirements
            foreach (var k in javascriptScripts)
            {
                try
                {
                    jintEngine.Run(k.Value);
                    if (modlodingDebug)
                        Console.WriteLine("Loaded Script: {0}", k.Key);
                }
                catch
                {
                    if (modlodingDebug)
                        Console.WriteLine("Error script: {0} ", k.Key);
                }
            }
        }

        void StartMod(string ModId, string name, IMod mod, ModManager m)
        {
            if (loaded.ContainsKey(name))
            {
                return;
            }
            if (modRequirements.ContainsKey(name))
            {
                foreach (string required_name in modRequirements[name])
                {
                    if (!mods[ModId].ContainsKey(required_name))
                    {
                        try
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("Can't load mod {0} because its dependency {1} couldn't be loaded.", name, required_name));
                        }
                        catch
                        {
                            //This will be the case if the server is running on a headless linux server without X11 installed (previously crashed)
                            Console.WriteLine(string.Format("[Mod error] Can't load mod {0} because its dependency {1} couldn't be loaded.", name, required_name));
                        }
                    }
                   
                        StartMod(ModId, required_name, mods[ModId][required_name], m);
          

                }
            }

 
                mod.Start(m);
 
          
            loaded[name] = true;
        }
    }
}
