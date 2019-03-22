using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredRealisticVert {

    class XLShredRealisticVert : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelRealisticVert;

        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("ghfear", "GHFear");
            uiLabelRealisticVert = uiBox.AddLabel(LabelType.Toggle, "Realistic Vert (V)", Side.left, () => Main.enabled, Main.settings.realisticVert && Main.enabled, (b) => Main.settings.realisticVert = b, 1);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.V, 0.2f, () => {
                    Main.settings.realisticVert = !Main.settings.realisticVert;
                    uiLabelRealisticVert.SetToggleValue(Main.settings.realisticVert);
                    if (Main.settings.realisticVert) {
                        ModMenu.Instance.ShowMessage("Realistic Vert: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Realistic Vert: OFF");
                    }
                });
            }
        }
    }
}
