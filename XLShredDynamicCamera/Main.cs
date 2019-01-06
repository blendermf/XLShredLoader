using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;
using XLShredLib;

namespace XLShredDynamicCamera {

    using Extensions;
    using Extensions.Components;

    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        private bool _cameraModActive = false;

        public bool CameraModActive {
            get {
                return this._cameraModActive;
            }
            set {
                _cameraModActive = value;
                if (!_cameraModActive) {
                    if (PlayerController.Instance != null) {
                        CameraControllerData cameraControllerData = PlayerController.Instance.cameraController.GetExtensionComponent();
                        if (cameraControllerData != null) {
                            cameraControllerData.inGrindCamera = false;
                        }
                    }
                }
            }
        }

        public void ToggleCameraModActive() {
            CameraModActive = !CameraModActive;
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
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            
            ModMenu.Instance.gameObject.AddComponent<XLShredDynamicCamera>();

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
