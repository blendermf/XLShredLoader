using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredDynamicCamera {
    class XLShredDynamicCamera : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelDynamicCamera;
        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("rafahel_mello", "Rafahel Mello");
            uiLabelDynamicCamera = uiBox.AddLabel(LabelType.Toggle, "Dynamic Grind Camera (C)", Side.left, () => Main.enabled, Main.settings.CameraModActive && Main.enabled, (b) => Main.settings.CameraModActive = b, 0);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.C, 0.2f, () => {
                    Main.settings.ToggleCameraModActive();
                    uiLabelDynamicCamera.SetToggleValue(Main.settings.CameraModActive);
                    if (Main.settings.CameraModActive) {
                        ModMenu.Instance.ShowMessage("Dynamic Camera: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Dynamic Camera: OFF");
                    }
                });
            }
        }
    }
}
