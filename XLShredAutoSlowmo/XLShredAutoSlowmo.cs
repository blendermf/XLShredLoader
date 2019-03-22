using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredAutoSlowmo {
    class XLShredAutoSlowmo : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelAutoSlowmo;
        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("kubas121", "kubas121");
            uiLabelAutoSlowmo = uiBox.AddLabel("auto-slow-motion", LabelType.Toggle, "Auto Slow Motion (S)", Side.left, () => Main.enabled, Main.settings.autoSlowmo && Main.enabled, (b) => Main.settings.autoSlowmo = b);

            ModMenu.Instance.RegisterTimeScaleTarget(Main.modId, () => {
                if (Main.enabled && Main.settings.autoSlowmo && !PlayerController.Instance.boardController.AllDown) {
                    return 0.6f;
                }
                return 1f;
            });
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.S, 0.2f, () => {
                    Main.settings.autoSlowmo = !Main.settings.autoSlowmo;

                    uiLabelAutoSlowmo.SetToggleValue(Main.settings.autoSlowmo);
                    if (Main.settings.autoSlowmo) {
                        ModMenu.Instance.ShowMessage("Auto Slow Motion: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Auto Slow Motion: OFF");
                    }
                });
            }
        }

        public void OnDestroy() {
            uiBox.RemoveLabel("auto-slow-motion");
        }
    }
}
