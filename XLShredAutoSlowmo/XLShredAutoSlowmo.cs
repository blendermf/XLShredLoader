using UnityEngine;
using XLShredLib;

using System;

namespace XLShredAutoSlowmo {
    class XLShredAutoSlowmo : MonoBehaviour {
        public void Start() {
            ModUIBox uiBoxKubas = ModMenu.Instance.RegisterModMaker("com.kubas121", "kubas121");
            uiBoxKubas.AddLabel("S - Enable Automatic Slow Motion", ModUIBox.Side.left, () => Main.enabled);

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
                    if (Main.settings.autoSlowmo) {
                        ModMenu.Instance.ShowMessage("Automatic Slow Motion: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Automatic Slow Motion: OFF");
                    }
                });
            }
        }
    }
}
