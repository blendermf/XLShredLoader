using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;

using XLShredLib;

namespace XLShredObjectSpawner
{
    static class Main
    {
        public static String modId;
        public static String modPath;
        public static bool enabled;
        public static HarmonyInstance harmonyInstance;

        static bool Load(UnityModManager.ModEntry modEntry) {
            modId = modEntry.Info.Id;
            modPath = modEntry.Path;

            modEntry.OnToggle = OnToggle;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            if (enabled == value) return true;
            enabled = value;
            if (enabled) {
                harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                ModMenu.Instance.gameObject.AddComponent<XLShredObjectSpawner>();
            } else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.GetComponent<XLShredObjectSpawner>());
            }
            return true;
        }
    }
}
