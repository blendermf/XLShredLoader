using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace XLShredCustomGrindManualPop {
    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        public float customGrindPopForce = 2f;
        public float customManualPopForce = 2.5f;

        public void IncreaseGrindPopForce() {
            customGrindPopForce += 0.1f;
        }

        public void DecreaseGrindPopForce() {
            customGrindPopForce -= 0.1f;
        }

        public void IncreaseManualPopForce() {
            customManualPopForce += 0.1f;
        }

        public void DecreaseManualPopForce() {
            customManualPopForce -= 0.1f;
        }

        public override void Save(UnityModManager.ModEntry modEntry) {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }

    static class Main
    {
        public static bool enabled;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry) {
            settings = Settings.Load<Settings>(modEntry);
            
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            
            ModMenu.Instance.gameObject.AddComponent<XLShredCustomGrindManualPop>();

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            enabled = value;
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
