using UnityEngine;
using XLShredLib;

using System;

namespace XLShredFasterSpin {
    class XLShredFasterSpin : MonoBehaviour {

        public void Start() {
            ModUIBox uiBoxRafahel = ModMenu.Instance.RegisterModMaker("rafahel_mello", "Rafahel Mello");
            uiBoxRafahel.AddLabel("G - Toggle Faster Grind Spin", ModUIBox.Side.left, () => Main.enabled, 2);
            uiBoxRafahel.AddLabel("L - Toggle Faster Body Spin", ModUIBox.Side.right, () => Main.enabled, 2);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.L, 0.2f, () => {
                    Main.settings.spinVelocityEnabled = !Main.settings.spinVelocityEnabled;
                    if (Main.settings.spinVelocityEnabled) {
                        ModMenu.Instance.ShowMessage("Faster Body Spin: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Faster Body Spin: OFF");
                    }
                });

                ModMenu.Instance.KeyPress(KeyCode.G, 0.2f, () => {
                    Main.settings.grindSpinVelocityEnabled = !Main.settings.grindSpinVelocityEnabled;
                    if (Main.settings.grindSpinVelocityEnabled) {
                        ModMenu.Instance.ShowMessage("Faster Grind Spin: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Faster Grind Spin: OFF");
                    }
                });
            }
        }
    }
}
