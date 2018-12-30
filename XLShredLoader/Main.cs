using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;

using XLShredLib;

namespace XLShredLoader
{
    [Serializable]
    public class Settings : UnityModManager.ModSettings {



        private float _customPopForce = 3f;
        
        

       public Settings() : base() {
            PlayerController.Instance.popForce = _customPopForce;
       }

        public float customPopForce {
            get {
                return this._customPopForce;
            }
            set {
                this._customPopForce = value;
                PlayerController.Instance.popForce = value;
            }
        }





        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(modEntry);
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
            modEntry.OnSaveGUI = OnSaveGUI;

            ModMenu.Instance.gameObject.AddComponent<ModMenuXLLoader>();

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
