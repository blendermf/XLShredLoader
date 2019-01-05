using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredFasterSpin {
    class XLShredFasterSpin : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelFasterGrindSpin;
        ModUILabel uiLabelFasterBodySpin;
        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("rafahel_mello", "Rafahel Mello");
            uiLabelFasterGrindSpin = uiBox.AddLabel(LabelType.Toggle, "Faster Grind Spin (G)", Side.left, () => Main.enabled, Main.settings.grindSpinVelocityEnabled && Main.enabled, (b) => Main.settings.grindSpinVelocityEnabled = b, 2);
            uiLabelFasterBodySpin = uiBox.AddLabel(LabelType.Toggle, "Faster Body Spin (L)", Side.right, () => Main.enabled, Main.settings.spinVelocityEnabled && Main.enabled, (b) => Main.settings.spinVelocityEnabled = b, 2);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.L, 0.2f, () => {
                    Main.settings.spinVelocityEnabled = !Main.settings.spinVelocityEnabled;
                    uiLabelFasterBodySpin.SetToggleValue(Main.settings.spinVelocityEnabled);
                    if (Main.settings.spinVelocityEnabled) {
                        ModMenu.Instance.ShowMessage("Faster Body Spin: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Faster Body Spin: OFF");
                    }
                });

                ModMenu.Instance.KeyPress(KeyCode.G, 0.2f, () => {
                    Main.settings.grindSpinVelocityEnabled = !Main.settings.grindSpinVelocityEnabled;
                    uiLabelFasterGrindSpin.SetToggleValue(Main.settings.grindSpinVelocityEnabled);
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
