using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredFlipMods {
    class XLShredFlipMods : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelFixedSwitchFlipPositions;
        ModUILabel uiLabelRealisticFlipTricks;
        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("rafahel_mello", "Rafahel Mello");
            uiLabelFixedSwitchFlipPositions = uiBox.AddLabel("switch-flip-trick-positions", LabelType.Toggle, "Switch Flip Trick Positions (M)", Side.right, () => Main.enabled, Main.settings.fixedSwitchFlipPositions && Main.enabled, (b) => Main.settings.fixedSwitchFlipPositions = b, 1);
            uiLabelRealisticFlipTricks = uiBox.AddLabel("realistic-flip-tricks", LabelType.Toggle, "Realistic Flip Tricks (N)", Side.left, () => Main.enabled, Main.settings.realisticFlipTricks && Main.enabled, (b) => Main.settings.realisticFlipTricks = b, 1);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.M, 0.2f, () => {
                    Main.settings.fixedSwitchFlipPositions = !Main.settings.fixedSwitchFlipPositions;
                    uiLabelFixedSwitchFlipPositions.SetToggleValue(Main.settings.fixedSwitchFlipPositions);
                    if (Main.settings.fixedSwitchFlipPositions) {
                        ModMenu.Instance.ShowMessage("Switch Flip Trick Positions: CHANGED");
                    } else {
                        ModMenu.Instance.ShowMessage("Switch Flip Trick Positions: DEFAULT");
                    }
                });

                ModMenu.Instance.KeyPress(KeyCode.N, 0.2f, () => {
                    Main.settings.realisticFlipTricks = !Main.settings.realisticFlipTricks;
                    uiLabelRealisticFlipTricks.SetToggleValue(Main.settings.realisticFlipTricks);
                    if (Main.settings.realisticFlipTricks) {
                        ModMenu.Instance.ShowMessage("Realistic Flip Tricks: ACTIVATED");
                    } else {
                        ModMenu.Instance.ShowMessage("Realistic Flip Tricks: DEACTIVATED");
                    }
                });
            }
        }

        public void OnDestroy() {
            uiBox.RemoveLabel("switch-flip-trick-positions");
            uiBox.RemoveLabel("realistic-flip-tricks");
        }
    }
}
