using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredDisableAutocatch {
    class XLShredDisableAutocatch : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelDisableAutocatch;
        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("ghfear", "GHFear");
            uiLabelDisableAutocatch = uiBox.AddLabel("disable-autocatch", LabelType.Toggle, "Disable Autocatch (A)", Side.right, () => Main.enabled, Main.settings.disableAutocatch && Main.enabled, (b) => Main.settings.disableAutocatch = b, 0);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.A, 0.2f, () => {
                    Main.settings.disableAutocatch = !Main.settings.disableAutocatch;

                    uiLabelDisableAutocatch.SetToggleValue(Main.settings.disableAutocatch);
                    if (Main.settings.disableAutocatch) {
                        ModMenu.Instance.ShowMessage("Disable Autocatch: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Disable Autocatch: OFF");
                    }
                });
            }
        }

        public void OnDestroy() {
            uiBox.RemoveLabel("disable-autocatch");
        }

    }
}
