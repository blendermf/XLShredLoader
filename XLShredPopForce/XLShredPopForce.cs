using UnityEngine;
using XLShredLib;

using System;

namespace XLShredPopForce {
    class XLShredPopForce : MonoBehaviour {
        public void Start() {
            ModUIBox uiBoxFigzyy = ModMenu.Instance.RegisterModMaker("figzy", "*Figzyy");
            uiBoxFigzyy.AddLabel("+/- - Adjust Pop Force", ModUIBox.Side.right, () => Main.enabled);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.Equals, 0.2f, () => {
                    if (Main.settings.customPopForce <= 7.8f) {
                        Main.settings.customPopForce += 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Minus, 0.2f, () => {
                    if (Main.settings.customPopForce >= 1.5f) {
                        Main.settings.customPopForce -= 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.KeypadPlus, 0.2f, () => {
                    if (Main.settings.customPopForce <= 7.8f) {
                        Main.settings.customPopForce += 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.KeypadMinus, 0.2f, () => {
                    if (Main.settings.customPopForce >= 1.5f) {
                        Main.settings.customPopForce -= 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Plus, 0.2f, () => {
                    if (Main.settings.customPopForce <= 7.8f) {
                        Main.settings.customPopForce += 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.Minus, 0.2f, () => {
                    if (Main.settings.customPopForce >= 1.5f) {
                        Main.settings.customPopForce -= 0.2f;
                    }
                    ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
                });
            }
        }
    }
}
