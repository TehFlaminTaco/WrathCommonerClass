using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static UnityModManagerNet.UnityModManager;

namespace WrathCommonerClass.Config
{
    static class ModSettings
    {
        public static ModEntry ModEntry;
        public static Blueprints Blueprints;

        public static void LoadAllSettings()
        {
            LoadSettings("Blueprints.json", ref Blueprints);
        }
        private static void LoadSettings<T>(string fileName, ref T setting) where T : IUpdatableSettings
        {
            var assembly = Assembly.GetExecutingAssembly();
            string userConfigFolder = ModEntry.Path + "UserSettings";
            Directory.CreateDirectory(userConfigFolder);
            var resourcePath = $"WrathCommonerClass.Config.{fileName}";
            var userPath = $"{userConfigFolder}{Path.DirectorySeparatorChar}{fileName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                setting = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            if (File.Exists(userPath))
            {
                using (StreamReader reader = File.OpenText(userPath))
                {
                    try
                    {
                        T userSettings = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                        setting.OverrideSettings(userSettings);
                    }
                    catch
                    {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_{fileName}", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            File.WriteAllText(userPath, JsonConvert.SerializeObject(setting, Formatting.Indented));
        }
    }
}
