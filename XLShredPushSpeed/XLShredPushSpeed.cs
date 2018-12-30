using UnityEngine;
using XLShredLib;

using System;

namespace XLShredPushSpeed {
    class XLShredPushSpeed : MonoBehaviour {
        public void Start() {
            ModUIBox uiBoxFigzyy = ModMenu.Instance.RegisterModMaker("com.figzy", "*Figzyy");
            uiBoxFigzyy.AddLabel("Page UP/DOWN - Adjust Push Speed", ModUIBox.Side.left, () => Main.enabled);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.PageUp, 0.1f, () => {
                    if (Main.settings.customPushForce <= 300f) {
                        Main.settings.customPushForce += 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.PageDown, 0.1f, () => {
                    if (Main.settings.customPushForce >= 1f) {
                        Main.settings.customPushForce -= 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Insert, 0.1f, () => {
                    Main.settings.customPushForce += 10f;

                    ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Delete, 0.1f, () => {
                    Main.settings.customPushForce -= 10f;

                    ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
                });
            }
        }
    }
}
