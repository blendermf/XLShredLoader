using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredPopForce {
    class XLShredPopForce : MonoBehaviour {

        ModUIBox uiBox;

        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("figzy", "*Figzyy");
            uiBox.AddLabel("adjust-pop-force","+/- - Adjust Pop Force", Side.right, () => Main.enabled);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.Equals, 0.2f, () => {
                    if (Main.settings.CustomPopForce <= 7.8f) {
                        Main.settings.CustomPopForce += 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.CustomPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Minus, 0.2f, () => {
                    if (Main.settings.CustomPopForce >= 1.5f) {
                        Main.settings.CustomPopForce -= 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.CustomPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.KeypadPlus, 0.2f, () => {
                    if (Main.settings.CustomPopForce <= 7.8f) {
                        Main.settings.CustomPopForce += 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.CustomPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.KeypadMinus, 0.2f, () => {
                    if (Main.settings.CustomPopForce >= 1.5f) {
                        Main.settings.CustomPopForce -= 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.CustomPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Plus, 0.2f, () => {
                    if (Main.settings.CustomPopForce <= 7.8f) {
                        Main.settings.CustomPopForce += 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.CustomPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Minus, 0.2f, () => {
                    if (Main.settings.CustomPopForce >= 1.5f) {
                        Main.settings.CustomPopForce -= 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.CustomPopForce) + " Default: 3.0");
                });
            }
        }

        public void OnDestroy() {
            uiBox.RemoveLabel("adjust-pop-force");
        }
    }
}
