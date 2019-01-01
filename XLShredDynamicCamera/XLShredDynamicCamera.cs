using UnityEngine;
using XLShredLib;

using System;

namespace XLShredDynamicCamera {
    class XLShredDynamicCamera : MonoBehaviour {

        public void Start() {
            ModUIBox uiBoxRafahel = ModMenu.Instance.RegisterModMaker("rafahel_mello", "Rafahel Mello");
            uiBoxRafahel.AddLabel("C - Toggle Dynamic Camera", ModUIBox.Side.left, () => Main.enabled, 0);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.C, 0.2f, () => {
                    Main.settings.ToggleCameraModActive();
                    if (Main.settings.GetCameraModActive()) {
                        ModMenu.Instance.ShowMessage("Dynamic Camera: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Dynamic Camera: OFF");
                    }
                });
            }
        }
    }
}
