using System;
using System.Reflection;
using UnityEngine;

using Harmony12;
using UnityModManagerNet;

using XLShredLib;
using XLShredLib.UI;
using XLShredReplayEditor;

namespace XLShredMenuMod {
    public class ReplayModMenuCompatibility : MonoBehaviour {
        private object replayModInstance = null;
        private Traverse replayModActiveField = null;
        private bool replayWasActive = false;

        public void Start() {
            ModUIBox uiBoxKiwi = ModMenu.Instance.RegisterModMaker("com.kiwi", "Kiwi");
            uiBoxKiwi.AddLabel("Start - Replay Editor", Side.right, () => UnityModManager.FindMod("XLShredReplayEditor").Enabled);

            ModMenu.Instance.RegisterShowCursor("XLShredReplayEditor", () => {
                if (replayModActiveField != null) {
                    return replayModActiveField.GetValue<bool>() ? 1 : 0;
                } else {
                    return 0;
                }
            });

        }

        public void Update() {
            if (replayModInstance == null) {
                replayModInstance = ReplayManager.Instance;

                if (replayModInstance != null) {
                    Traverse tReplayModInstance = Traverse.Create(replayModInstance);
                    replayModActiveField = tReplayModInstance.Field("isEditorActive");
                }
                
            } else {
                XLShredDataRegistry.SetData("blendermf.ReplayModMenuCompatibility", "isReplayEditorActive", replayModActiveField.GetValue<bool>());

                if (!replayWasActive && replayModActiveField.GetValue<bool>()) {
                    ModMenu.Instance.RegisterTimeScaleExclusive(() => {
                        bool ret = !(replayWasActive && !replayModActiveField.GetValue<bool>());
                        if (ret) Time.timeScale = 0f;
                        return ret;
                    });
                }

                replayWasActive = replayModActiveField.GetValue<bool>();
            }
        }
    }
}