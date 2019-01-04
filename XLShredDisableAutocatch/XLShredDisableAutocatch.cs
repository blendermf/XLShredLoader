using UnityEngine;
using XLShredLib;

using System;

namespace XLShredDisableAutocatch {
    class XLShredDisableAutocatch : MonoBehaviour {

        public void Start() {
            ModUIBox uiBoxGHFear = ModMenu.Instance.RegisterModMaker("ghfear", "GHFear");
            uiBoxGHFear.AddLabel("A - Disable Autocatch", ModUIBox.Side.right, () => Main.enabled, 1);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.A, 0.2f, () => {
                    Main.settings.disableAutocatch = !Main.settings.disableAutocatch;
                    if (Main.settings.disableAutocatch) {
                        ModMenu.Instance.ShowMessage("Autocatch: OFF");
                    } else {
                        ModMenu.Instance.ShowMessage("Autocatch: ON");
                    }
                });
            }
        }

    }
}
