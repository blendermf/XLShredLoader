using UnityEngine;
using XLShredLib;

using System;
using System.Linq;

using UnityModManagerNet;
using System.Reflection;

namespace XLShredFixedSlowmo {
    class XLShredFixedSlowmo : MonoBehaviour {
        private UnityModManager.ModEntry replayModEntry = null;
        private object replayModInstance = null;
        private FieldInfo replayEditorActiveField = null;

        public void Start() {
            ModUIBox uiBoxKlepto = ModMenu.Instance.RegisterModMaker("commander_klepto", "Commander Klepto");
            uiBoxKlepto.AddLabel("LB - Enable Slow Motion", ModUIBox.Side.right, () => Main.enabled);

            ModMenu.Instance.RegisterTimeScaleTarget(Main.modId, () => {
                if (Main.enabled && Main.settings.fixedSlowmo) {
                    return 0.6f;
                }
                return 1f;
            });
        }

        public void Update() {

            bool replayEditorActive;

            if (!XLShredDataRegistry.TryGetData<bool>("blendermf.ReplayModMenuCompatibility", "isReplayEditorActive", out replayEditorActive, false)) {
                replayEditorActive = false;
            }

            if (!replayEditorActive) {
                if (PlayerController.Instance.inputController.player.GetButtonSinglePressDown("LB")) {
                    if (Main.enabled) {
                        Main.settings.fixedSlowmo = !Main.settings.fixedSlowmo;
                    }
                }
            }
        }
    }
}
