using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace XLShredPushSpeed {
    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        private float _customPushForce = 6f;

        public Settings() : base() {
            PlayerController.Instance.skaterController.pushForce = _customPushForce;
        }

        public float CustomPushForce {
            get {
                return this._customPushForce;
            }
            set {
                if (Main.enabled) {
                    this._customPushForce = value;
                }
                PlayerController.Instance.skaterController.pushForce = value;
                PlayerController.Instance.topSpeed = 7f + ((value - 6f) * 0.5f);
            }
        }

        public void RestoreCustomPushForce() {
            CustomPushForce = _customPushForce;
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

        static bool Load(UnityModManager.ModEntry modEntry) {
            settings = Settings.Load<Settings>(modEntry);
            modId = modEntry.Info.Id;
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            if (!modEntry.Enabled) {
                Main.settings.CustomPushForce = 6f;
            } else {
                Main.settings.RestoreCustomPushForce();
            }

            ModMenu.Instance.gameObject.AddComponent<XLShredPushSpeed>();

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            enabled = value;
            if (!value) {
                Main.settings.CustomPushForce = 6f;
            } else {
                Main.settings.RestoreCustomPushForce();
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
