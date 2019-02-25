using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace XLShredRespawnNearBail {

    using Extensions.Components;

    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        public bool respawnNearBail = false;

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
                RespawnData respawnData = PlayerController.Instance.respawn.gameObject.AddComponent<RespawnData>();
                respawnData.RespawnComponent = PlayerController.Instance.respawn;
            } else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<XLShredRespawnNearBail>());
                UnityEngine.Object.Destroy(PlayerController.Instance.respawn.gameObject.GetComponent<RespawnData>());
            }
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
