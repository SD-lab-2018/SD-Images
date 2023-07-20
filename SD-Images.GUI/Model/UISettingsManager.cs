using Newtonsoft.Json;
using SD_Images.GUI.Model.Entities;
using System;
using System.IO;

namespace SD_Images.GUI.Model
{
    public class UISettingsManager
    {
        private static Lazy<UISettingsManager> _lazy = new Lazy<UISettingsManager>(() => new UISettingsManager());

        private const string PATH = "settings.json";

        private UISettings _settings;

        private UISettingsManager() 
        {
            if (File.Exists(PATH))
            {
                var json = File.ReadAllText(PATH);
                _settings = JsonConvert.DeserializeObject<UISettings>(json)!;
            }
            else
            {
                _settings = new();  
            }
        }

        public static UISettings Current => _lazy.Value._settings;

        public static void Update()
        {
            var json = JsonConvert.SerializeObject(Current, Formatting.Indented);
            File.WriteAllText(PATH, json);
        }
    }
}
