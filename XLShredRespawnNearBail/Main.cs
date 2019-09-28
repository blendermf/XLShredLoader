using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace XLShredRespawnNearBail {

    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        public bool _respawnNearBail = false;

        public bool RespawnNearBail {
            get {
                return this._respawnNearBail;
            }
            set {
                if (Main.enabled) {
                    _respawnNearBail = value;
                    PlayerController.Instance.respawn.respawnAtSpot = value;
                }
            }
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
            XLShredDataRegistry.SetData("kiwi.XLShredRespawnNearBail", "isRespawnNearBailActive", settings.RespawnNearBail);

            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            
            return true;
        }

        public static HarmonyInstance harmonyInstance;

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            if (enabled == value) return true;
            enabled = value;
            if (enabled) {
                harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                ModMenu.Instance.gameObject.AddComponent<XLShredRespawnNearBail>();
                PlayerController.Instance.respawn.respawnAtSpot = settings.RespawnNearBail;

            } else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<XLShredRespawnNearBail>());
                PlayerController.Instance.respawn.respawnAtSpot = false;
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
