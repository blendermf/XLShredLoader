using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;

using XLShredLoader.Extensions;
using XLShredLoader.Extensions.Components;
using XLShredLoader.Patches;

namespace XLShredLoader
{
    [Serializable()]
    public class Settings : UnityModManager.ModSettings {

        public bool realisticFlipTricks = true;
        public bool fixedSwitchFlipPositions = true;
        public float customGrindPopForce = 2f;
        public bool grindSpinVelocityEnabled = true;

        private bool _cameraModActive = true;

        public void ToggleCameraModActive() {
            this._cameraModActive = !this._cameraModActive;
            if (!this._cameraModActive) {
                if (PlayerController.Instance != null) {
                    PlayerController.Instance.cameraController.GetExtensionComponent().inGrindCamera = false;
                }
            }
        }

        public bool GetCameraModActive() {
            return this._cameraModActive;
        }

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

            PlayerController.Instance.gameObject.AddComponent<PlayerControllerData>();
            PlayerController.Instance.cameraController.gameObject.AddComponent<CameraControllerData>();

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            enabled = value;
            return true;
        }
    }
}
