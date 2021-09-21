using UnityEngine;
using UnityModManagerNet;
using UnityEngine.UI;
using WrathCommonerClass.Config;
using System;
using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints.JsonSystem;
using WrathCommonerClass.Classes;
using Kingmaker.Settings;

namespace WrathCommonerClass
{
    static class Main
    {
        public static bool Enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            var harmony = new Harmony(modEntry.Info.Id);
            ModSettings.ModEntry = modEntry;
            ModSettings.LoadAllSettings();
            harmony.PatchAll();
            //PostPatchInitializer.Initialize();
            //Commoner.Load();
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        public static void Log(string msg)
        {
            ModSettings.ModEntry.Logger.Log(msg);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogDebug(string msg)
        {
            ModSettings.ModEntry.Logger.Log(msg);
        }
        public static void LogPatch(string action, [NotNull] IScriptableObjectWithAssetId bp)
        {
            Log($"{action}: {bp.AssetGuid} - {bp.name}");
        }
        public static Exception Error(String message)
        {
            Log(message);
            return new InvalidOperationException(message);
        }

        [HarmonyPatch(typeof(GameSettingsController), "LocalizationManagerInitialized")]
        static class GameSettingsController_LocalizationManagerInitialized_LoadClasses
        {
            static void Postfix()
            {
                Commoner.Load();
            }
        }
    }
}
