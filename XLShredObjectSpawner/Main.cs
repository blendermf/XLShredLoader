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
        public static bool enabled;
        private static GameObject modmenu;

        static bool Load(UnityModManager.ModEntry modEntry) {

            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnToggle = OnToggle;

            ModMenu.Instance.gameObject.AddComponent<XLShredObjectSpawner>();

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            enabled = value;
            return true;
        }
    }
}
