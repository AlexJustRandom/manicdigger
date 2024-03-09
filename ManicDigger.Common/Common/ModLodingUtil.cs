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


            List<Tuple<JObject,string>> modinfoDict = new List<Tuple<JObject, string>>();
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
                            modinfoDict.Add(new Tuple<JObject,string>((JObject)JToken.ReadFrom(reader),Path.GetDirectoryName(s)));
                        }

                    }
                }


            }

            ModInformation[] modinfos = new ModInformation[modinfoDict.Count];

            for (int i = 0; i < modinfoDict.Count; i++)
            {
                modinfos[i] = modinfoDict[i].Item1.ToObject<ModInformation>();
                modinfos[i].SrcFolder = modinfoDict[i].Item2;


            }
            length.SetValue(modinfoDict.Count);
            return modinfos;



        }
    }
}
