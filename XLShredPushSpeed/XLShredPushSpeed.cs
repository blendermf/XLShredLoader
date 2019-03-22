using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredPushSpeed {
    class XLShredPushSpeed : MonoBehaviour {
        ModUIBox uiBox;

        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("figzy", "*Figzyy");
            uiBox.AddLabel("adjust-push-speed", "Page UP/DOWN - Adjust Push Speed", Side.left, () => Main.enabled);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.PageUp, 0.2f, () => {
                    if (Main.settings.CustomPushForce <= 300f) {
                        Main.settings.CustomPushForce += 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.CustomPushForce) + " Default: 6.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.PageDown, 0.2f, () => {
                    if (Main.settings.CustomPushForce >= 1f) {
                        Main.settings.CustomPushForce -= 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.CustomPushForce) + " Default: 6.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Insert, 0.2f, () => {
                    Main.settings.CustomPushForce += 10f;

                    ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.CustomPushForce) + " Default: 6.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Delete, 0.2f, () => {
                    Main.settings.CustomPushForce -= 10f;

                    ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.CustomPushForce) + " Default: 6.0");
                });
            }
        }

        public void OnDestroy() {
            uiBox.RemoveLabel("adjust-push-speed");
        }
    }
}
