using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;
using System.Linq;

using UnityModManagerNet;
using System.Reflection;

namespace XLShredFixedSlowmo {
    class XLShredFixedSlowmo : MonoBehaviour {
        private ModUIBox uiBox;
        private ModUILabel uiLabelSlowMotion;

        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("commander_klepto", "Commander Klepto");
            uiLabelSlowMotion = uiBox.AddLabel(LabelType.Toggle, "Slow Motion (LB)", Side.right, () => Main.enabled, Main.settings.fixedSlowmo && Main.enabled, (b) => Main.settings.fixedSlowmo = b);

            ModMenu.Instance.RegisterTimeScaleTarget(Main.modId, () => {
                if (Main.enabled && Main.settings.fixedSlowmo) {
                    return 0.6f;
                }
                return 1f;
            });
        }

        public void Update() {


            if (!XLShredDataRegistry.TryGetData<bool>("blendermf.ReplayModMenuCompatibility", "isReplayEditorActive", out bool replayEditorActive, false)) {
                replayEditorActive = false;
            }

            if (!replayEditorActive) {
                if (PlayerController.Instance.inputController.player.GetButtonSinglePressDown("LB")) {
                    if (Main.enabled) {
                        Main.settings.fixedSlowmo = !Main.settings.fixedSlowmo;
                        uiLabelSlowMotion.SetToggleValue(Main.settings.fixedSlowmo);

                        if (Main.settings.fixedSlowmo) {
                            ModMenu.Instance.ShowMessage("Slowmo: ON");
                        } else {
                            ModMenu.Instance.ShowMessage("Slowmo: OFF");
                        }
                    }
                }
            }
        }
    }
}
