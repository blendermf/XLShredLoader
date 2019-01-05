using UnityEngine;
using XLShredLib;

using System;

namespace XLShredRespawnNearBail {
    class XLShredRespawnNearBail : MonoBehaviour {

        public void Start() {
            ModUIBox uiBoxGHFear = ModMenu.Instance.RegisterModMaker("ghfear", "GHFear");
            uiBoxGHFear.AddLabel("V - Realistic Vert", ModUIBox.Side.left, () => Main.enabled, 1);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.V, 0.2f, () => {
                    Main.settings.realisticVert = !Main.settings.realisticVert;
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
