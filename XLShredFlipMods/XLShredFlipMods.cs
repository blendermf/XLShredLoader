using UnityEngine;
using XLShredLib;

using System;

namespace XLShredFlipMods {
    class XLShredFlipMods : MonoBehaviour {

        public void Start() {
            ModUIBox uiBoxRafahel = ModMenu.Instance.RegisterModMaker("com.rafahel_mello", "Rafahel Mello");
            uiBoxRafahel.AddLabel("M - Toggle Switch Flip Trick Positions", ModUIBox.Side.right, () => Main.enabled, 1);
            uiBoxRafahel.AddLabel("N - Toggle Realistic Flip Tricks Mode", ModUIBox.Side.left, () => Main.enabled, 1);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.M, 0.2f, () => {
                    Main.settings.fixedSwitchFlipPositions = !Main.settings.fixedSwitchFlipPositions;
                    if (Main.settings.fixedSwitchFlipPositions) {
                        ModMenu.Instance.ShowMessage("Switch Flip Trick Positions: CHANGED");
                    } else {
                        ModMenu.Instance.ShowMessage("Switch Flip Trick Positions: DEFAULT");
                    }
                });

                ModMenu.Instance.KeyPress(KeyCode.N, 0.2f, () => {
                    Main.settings.realisticFlipTricks = !Main.settings.realisticFlipTricks;
                    if (Main.settings.realisticFlipTricks) {
                        ModMenu.Instance.ShowMessage("Realistic Flip Tricks: ACTIVATED");
                    } else {
                        ModMenu.Instance.ShowMessage("Realistic Flip Tricks: DEACTIVATED");
                    }
                });
            }
        }
    }
}
