using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ManicDigger.Common
{
    public class IDBlocktype
    {
        public int id;
        public BlockType type;

    }

    public class ModLodingUtil
    {
        public ModLodingUtil()
        {

        }

        public static Dictionary<string, string> GetScriptSources(string modpath)
        {

            Dictionary<string, string> scripts = new Dictionary<string, string>();

            if (!Directory.Exists(modpath))
            {
                throw new Exception("Trying to load mods from non existent directory:" + modpath);
            }

            string[] dir = Directory.GetDirectories(modpath);
            List<string> directories = new List<string>(dir);
            directories.Add(modpath);

            foreach (string d in directories)
            {
                string[] files = Directory.GetFiles(d);

                foreach (string s in files)
                {
                    if (!GameStorePath.IsValidName(Path.GetFileNameWithoutExtension(s)))
                    {
                        continue;
                    }
                    if (!(Path.GetExtension(s).Equals(".cs", StringComparison.InvariantCultureIgnoreCase)
                        || Path.GetExtension(s).Equals(".js", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }
                    string scripttext = File.ReadAllText(s);
                    string filename = new FileInfo(s).Name;
                    scripts[filename] = scripttext;
                }
            }



            return scripts;
        }

        public static string[] GetListOfModpacks(IntRef  lenght) {

            string path =  GameStorePath.GetModpacksPath();
            string[] files = Directory.GetFiles(path);

            List<string> modpacks = new List<string>();
            modpacks.Add("Default");

            foreach (string s in files)
            {
                if (!(Path.GetExtension(s).Equals(".mp", StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }
                modpacks.Add(Path.GetFileNameWithoutExtension(s));
            }
 
            lenght.SetValue(modpacks.Count);
            return modpacks.ToArray();
        }
        public static void DeleteModpack(string name)
        {
            string path = GameStorePath.GetModpacksPath();
            File.Delete(Path.Combine(path, name + ".mp"));
        }
        public static void SaveModpack(string name,string[] activeMods) {
            string path = GameStorePath.GetModpacksPath();
            File.WriteAllText(Path.Combine(path, name+ ".mp"), string.Join("\n", activeMods));
        }

        public static void SetCurrentModpack(string value) {
            string[] modpaths = new[] { Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine("..", ".."), ".."), "ManicDigger.Common"), "Server"), "Mods"), "Mods" };
            for (int i = 0; i < modpaths.Length; i++)
            {
                if (File.Exists(Path.Combine(modpaths[i], "current.txt")))
                {
                    File.WriteAllText(Path.Combine(modpaths[i], "current.txt"), value);
                     
                }
                else if (Directory.Exists(modpaths[i]))
                {
                    File.WriteAllText(Path.Combine(modpaths[i], "current.txt"), value);

                }
            }
        }

        public static string[] GetMods(string name,IntRef lenght)
        {
            if(name == "Default") {//Hardcoded? what else TODO
                List<string> defaultModpacks =new List<string>();
                defaultModpacks.Add("core");
                defaultModpacks.Add("morecomands");
                defaultModpacks.Add("upgrades"); 
                lenght.SetValue(defaultModpacks.Count);
                return defaultModpacks.ToArray();
            }



            string path = GameStorePath.GetModpacksPath();
            string[] files = Directory.GetFiles(path);

             foreach (string s in files)
            {
                if (!(Path.GetExtension(s).Equals(".mp", StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }
                if (Path.GetFileNameWithoutExtension(s).ToLower() != name.ToLower()) continue;
                string content = File.ReadAllText(s);
                 string[] mods = content.Split('\n');
                lenght.SetValue(mods.Length);
                return mods;
            }
            Console.WriteLine("CRITICAL ERROR :; Are u tring to break the game? or did you ran out of storage space?");
            lenght.SetValue(0);
            return new string[0];
        }

        public static string GetCurrentModpack() {
            string[] modpaths = new[] { Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine("..", ".."), ".."), "ManicDigger.Common"), "Server"), "Mods"), "Mods" };

            for (int i = 0; i < modpaths.Length; i++)
            {
                if (File.Exists(Path.Combine(modpaths[i], "current.txt")))
                {
                    string modpackname = File.ReadAllText(Path.Combine(modpaths[i], "current.txt")).Trim(); ;
                    IntRef dummy = new IntRef();
                    var modpacks = GetListOfModpacks(dummy);
                    if (Array.IndexOf(modpacks, modpackname) == -1)
                        return "Default";
                    return modpackname;
                }
                else if (Directory.Exists(modpaths[i]))
                {
                    try
                    {
                        string modpackname = "Default";//something frong TODO im not sober enought to find what
                        File.WriteAllText(Path.Combine(modpaths[i], "current.txt"), modpackname);
                        return modpackname;
                    }
                    catch
                    {
                    }
                }
 
            }
            return "Default";
        }


        public static ModInformation[] GetModlist(IntRef length)
        {
 
            string[] modpaths = new[] { Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine("..", ".."), ".."), "ManicDigger.Common"), "Server"), "Mods"), "Mods" };

            //for (int i = 0; i < modpaths.Length; i++)
            //{
            //    if (File.Exists(Path.Combine(modpaths[i], "current.txt")))
            //    {
            //        gamemode = File.ReadAllText(Path.Combine(modpaths[i], "current.txt")).Trim();
            //    }
            //    else if (Directory.Exists(modpaths[i]))
            //    {
            //        try
            //        {
            //            File.WriteAllText(Path.Combine(modpaths[i], "current.txt"), gamemode);
            //        }
            //        catch
            //        {
            //        }
            //    }
            //    modpaths[i] = Path.Combine(modpaths[i], gamemode);
            //}


            List<Tuple<JObject, string>> modinfoDict = new List<Tuple<JObject, string>>();
            foreach (string modpath in modpaths)
            {
                if (!Directory.Exists(modpath))
                {
                    continue;
                }

                string[] directories = Directory.GetDirectories(modpath);

                foreach (string d in directories)
                {
                    string[] files = Directory.GetFiles(d);

                    foreach (string s in files)
                    {
                        if (!GameStorePath.IsValidName(Path.GetFileNameWithoutExtension(s)))
                        {
                            continue;
                        }
                        if (!(Path.GetExtension(s).Equals(".json", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            continue;
                        }
                        using (StreamReader file = File.OpenText(s))
                        using (JsonTextReader reader = new JsonTextReader(file))
                        {
                            modinfoDict.Add(new Tuple<JObject, string>((JObject)JToken.ReadFrom(reader), Path.GetDirectoryName(s)));
                        }

                    }
                }


            }

            ModInformation[] modinfos = new ModInformation[modinfoDict.Count];

            for (int i = 0; i < modinfoDict.Count; i++)
            {
                modinfos[i] = modinfoDict[i].Item1.ToObject<ModInformation>();
                modinfos[i].SrcFolder = modinfoDict[i].Item2;
                if (string.IsNullOrEmpty(modinfos[i].ModID))
                {
                    modinfos[i].ModID = modinfos[i].Name.Replace(" ", "_").ToLower();
                    Console.WriteLine(string.Format("WARNING Mod ID is null (trying to autoasign) in {0}", modinfoDict[i].Item2));

                }
                if (string.IsNullOrEmpty(modinfos[i].ModID))
                    Console.WriteLine(string.Format("ERROR Mod ID Cant be null in {0}",modinfoDict[i].Item2));

            }
            length.SetValue(modinfoDict.Count);
            return modinfos;

           



        }


        public static string[] FindBlockDefinitions(string path, IntRef lenght)
        {
            List<string> blockdef = new List<string>();

            string[] directories = Directory.GetDirectories(path);

            foreach (string d in directories)
            {
                string[] files = Directory.GetFiles(d);

                foreach (string s in files)
                {
                    if (!GameStorePath.IsValidName(Path.GetFileNameWithoutExtension(s))) continue;
                    if (!s.ToLower().Contains("block")) continue;
                    if (!(Path.GetExtension(s).Equals(".json", StringComparison.InvariantCultureIgnoreCase))) continue;

                    blockdef.Add(s);
                }
            }
            lenght.SetValue(blockdef.Count);
            return blockdef.ToArray();

        }

        public static IDBlocktype[] LoadBlocks(string path,IntRef lenght)
        {
            Console.WriteLine(string.Format("Loading blocks from:{0}", path));


            using (StreamReader file = File.OpenText(path))
            {
                var Blocks = JsonConvert.DeserializeObject<List<IDBlocktype>>(file.ReadToEnd());
            
                Console.WriteLine("Blocks loaded: {0}", Blocks.Count);
                lenght.SetValue(Blocks.Count);
                return Blocks.ToArray();
            }

        }
    }
}
