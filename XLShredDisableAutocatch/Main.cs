using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace XLShredDisableAutocatch {
    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        public bool disableAutocatch = false;

        public override void Save(UnityModManager.ModEntry modEntry) {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }

    static class Main
    {
        public static bool enabled;
        public static Settings settings;
        public static HarmonyInstance harmonyInstance;

        static bool Load(UnityModManager.ModEntry modEntry) {
            settings = Settings.Load<Settings>(modEntry);
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            if (enabled == value) return true;
            enabled = value;
            if (enabled) {
                harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                ModMenu.Instance.gameObject.AddComponent<XLShredDisableAutocatch>();
            } else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<XLShredDisableAutocatch>());
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
