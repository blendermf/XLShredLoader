using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredDisablePushReduction {
    class XLShredDisablePushReduction : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelDisablePushReduction;
        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("com.kiwi", "Kiwi");
            uiLabelDisablePushReduction = uiBox.AddLabel(LabelType.Toggle, "Disable Push Reduction (P)", Side.left, () => Main.enabled, Main.settings.disablePushReduction && Main.enabled, (b) => Main.settings.disablePushReduction = b);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.P, 0.2f, () => {
                    Main.settings.disablePushReduction = !Main.settings.disablePushReduction;
                    uiLabelDisablePushReduction.SetToggleValue(Main.settings.disablePushReduction);
                    if (Main.settings.disablePushReduction) {
                        ModMenu.Instance.ShowMessage("Disable Push Reduction: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Disable Push Reduction: OFF");
                    }
                });
            }
        }
    }
}
