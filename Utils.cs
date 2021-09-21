﻿using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrathCommonerClass.Config;

namespace WrathCommonerClass
{
    public static class Utils
    {
        // See: Tabletop Tweaks for source
        public static T CreateBlueprint<T>([NotNull] string name, Action<T> init = null) where T : SimpleBlueprint, new()
        {
            var result = new T
            {
                name = name,
                AssetGuid = ModSettings.Blueprints.GetGUID(name)
            };
            Resources.AddBlueprint(result);
            init?.Invoke(result);
            return result;
        }

        // All localized strings created in this mod, mapped to their localized key. Populated by CreateString.
        static Dictionary<String, LocalizedString> textToLocalizedString = new Dictionary<string, LocalizedString>();
        public static LocalizedString CreateString(string key, string value)
        {
            // See if we used the text previously.
            // (It's common for many features to use the same localized text.
            // In that case, we reuse the old entry instead of making a new one.)
            if(textToLocalizedString is null)
            {
                textToLocalizedString = new Dictionary<string, LocalizedString>();
            }
            LocalizedString localized = null;
            if (textToLocalizedString.TryGetValue(value, out localized))
            {
                return localized;
            }
            var strings = LocalizationManager.CurrentPack.Strings;
            String oldValue;
            if (strings.TryGetValue(key, out oldValue) && value != oldValue)
            {
#if DEBUG
                Main.LogDebug($"Info: duplicate localized string `{key}`, different text.");
#endif
            }
            strings[key] = value;
            localized = new LocalizedString
            {
                Key = key
            };
            textToLocalizedString[value] = localized;
            return localized;
        }
    }
}
