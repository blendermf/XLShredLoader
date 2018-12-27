using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;

namespace XLShredLoader
{

    public class Settings : UnityModManager.ModSettings {

        public bool realisticFlipTricks = true;
        public bool fixedSwitchFlipPositions = true;

        public override void Save(UnityModManager.ModEntry modEntry) {
            base.Save(modEntry);
        }
    }

    static class Main
    {
        public static bool enabled;
        public static Settings settings;
        private static GameObject modmenu;

        static bool Load(UnityModManager.ModEntry modEntry) {
            settings = Settings.Load<Settings>(modEntry);

            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnToggle = OnToggle;

            modmenu = new GameObject();
            modmenu.AddComponent<ModMenu>();
            UnityEngine.Object.DontDestroyOnLoad(Main.modmenu);

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            enabled = value;
            return true;
        }
    }
}
