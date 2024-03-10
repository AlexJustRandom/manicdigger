using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ManicDigger.Common
{
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


        public static ModInformation[] GetModlist(IntRef length)
        {
            string gamemode = "";

            string[] modpaths = new[] { Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine("..", ".."), ".."), "ManicDigger.Common"), "Server"), "Mods"), "Mods" };

            for (int i = 0; i < modpaths.Length; i++)
            {
                if (File.Exists(Path.Combine(modpaths[i], "current.txt")))
                {
                    gamemode = File.ReadAllText(Path.Combine(modpaths[i], "current.txt")).Trim();
                }
                else if (Directory.Exists(modpaths[i]))
                {
                    try
                    {
                        File.WriteAllText(Path.Combine(modpaths[i], "current.txt"), gamemode);
                    }
                    catch
                    {
                    }
                }
                modpaths[i] = Path.Combine(modpaths[i], gamemode);
            }


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
    }
}
