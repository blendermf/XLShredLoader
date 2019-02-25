using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace XLShredPopForce {
    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        private float _customPopForce = 3f;

        public Settings() : base() {
            PlayerController.Instance.popForce = _customPopForce;
        }

        public float CustomPopForce {
            get {
                return this._customPopForce;
            }
            set {
                if (Main.enabled) {
                    this._customPopForce = value;
                }
                
                PlayerController.Instance.popForce = value;
            }
        }

        public void RestoreCustomPopForce() {
            CustomPopForce = _customPopForce;
        }

        public override void Save(UnityModManager.ModEntry modEntry) {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }

    static class Main
    {
        public static bool enabled;
        public static Settings settings;
        public static String modId;
        public static HarmonyInstance harmonyInstance;

        static bool Load(UnityModManager.ModEntry modEntry) {
            settings = Settings.Load<Settings>(modEntry);
            modId = modEntry.Info.Id;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            if (enabled == value) return true;
            enabled = value;
            if (enabled) {
                Main.settings.CustomPopForce = 3f;
                harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                ModMenu.Instance.gameObject.AddComponent<XLShredPopForce>();
            } else {
                Main.settings.RestoreCustomPopForce();
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<XLShredPopForce>());
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
