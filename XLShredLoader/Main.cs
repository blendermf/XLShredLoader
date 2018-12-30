using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;

using XLShredLoader.Extensions;
using XLShredLoader.Extensions.Components;
using XLShredLoader.Patches;
using XLShredLib;

namespace XLShredLoader
{
    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        public bool realisticFlipTricks = false;
        public bool fixedSwitchFlipPositions = false;
        public bool grindSpinVelocityEnabled = false;
        public bool spinVelocityEnabled = false;
        public bool autoSlowmo = false;
        public float timeScaleTarget = 1f;

        private float _customPopForce = 3f;
        private float _customPushForce = 8f;
        private bool _cameraModActive = false;

       public Settings() : base() {
            PlayerController.Instance.popForce = _customPopForce;
            PlayerController.Instance.skaterController.pushForce = _customPushForce;
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

        public float customPushForce {
            get {
                return this._customPushForce;
            }
            set {
                this._customPushForce = value;
                PlayerController.Instance.skaterController.pushForce = value;
                PlayerController.Instance.topSpeed = 7f + ((value - 8f) * 0.5f);
            }
        }

        public bool GetCameraModActive() {
            return _cameraModActive;
        }

        public void ToggleCameraModActive() {
            _cameraModActive = !_cameraModActive;
            if (!_cameraModActive) {
                if (PlayerController.Instance != null) {
                    PlayerController.Instance.cameraController.GetExtensionComponent().inGrindCamera = false;
                }
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

            PlayerController.Instance.gameObject.AddComponent<PlayerControllerData>();
            CameraControllerData cameraControllerData = PlayerController.Instance.cameraController.gameObject.AddComponent<CameraControllerData>();
            cameraControllerData.cameraController = PlayerController.Instance.cameraController;

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
