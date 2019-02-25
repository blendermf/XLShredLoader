using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace XLShredFasterSpin {
    using Extensions.Components;

    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        public bool grindSpinVelocityEnabled = false;
        public bool spinVelocityEnabled = false;

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
                ModMenu.Instance.gameObject.AddComponent<XLShredFasterSpin>();
                PlayerController.Instance.gameObject.AddComponent<PlayerControllerData>();
            } else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<XLShredFasterSpin>());
                UnityEngine.Object.Destroy(PlayerController.Instance.gameObject.GetComponent<PlayerControllerData>());
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
