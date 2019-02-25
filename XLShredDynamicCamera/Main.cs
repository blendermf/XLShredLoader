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
                CameraControllerData cameraControllerData = PlayerController.Instance.cameraController.gameObject.AddComponent<CameraControllerData>();
                cameraControllerData.CameraControllerComponent = PlayerController.Instance.cameraController;
                ModMenu.Instance.gameObject.AddComponent<XLShredDynamicCamera>();
            } else {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                UnityEngine.Object.Destroy(PlayerController.Instance.cameraController.gameObject.GetComponent<CameraControllerData>());
                UnityEngine.Object.Destroy(ModMenu.Instance.gameObject.AddComponent<XLShredDynamicCamera>());
            }
            return true;
        }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
