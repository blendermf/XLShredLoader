using UnityEngine;
using XLShredLib;

using System;

namespace XLShredFixedSlowmo {
    class XLShredFixedSlowmo : MonoBehaviour {
        public void Start() {
            ModUIBox uiBoxKlepto = ModMenu.Instance.RegisterModMaker("com.commander_klepto", "Commander Klepto");
            uiBoxKlepto.AddLabel("LB - Enable Slow Motion", ModUIBox.Side.right, () => Main.enabled);

            ModMenu.Instance.RegisterTimeScaleTarget(Main.modId, () => {
                if (Main.enabled && Main.settings.fixedSlowmo) {
                    return 0.6f;
                }
                return 1f;
            });
        }

        public void Update() {
            if (PlayerController.Instance.inputController.player.GetButtonSinglePressDown("LB")) {
                if (Main.enabled) {
                    Main.settings.fixedSlowmo = !Main.settings.fixedSlowmo;
                }
            }
        }
    }
}
