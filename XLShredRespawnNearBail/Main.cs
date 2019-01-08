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
            
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            
            ModMenu.Instance.gameObject.AddComponent<XLShredRespawnNearBail>();

            RespawnData respawnData = PlayerController.Instance.respawn.gameObject.AddComponent<RespawnData>();
            respawnData.RespawnComponent = PlayerController.Instance.respawn;

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
