using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredRespawnNearBail {
    class XLShredRespawnNearBail : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelRespawnNearBail;
        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("com.kiwi", "Kiwi");
            uiLabelRespawnNearBail = uiBox.AddLabel("respawn-near-bail", LabelType.Toggle, "Respawn Near Bail (R)", Side.left,
                () => Main.enabled, Main.settings.RespawnNearBail && Main.enabled, 
                (b) => {
                    Main.settings.RespawnNearBail = b;
                    XLShredDataRegistry.SetData("kiwi.XLShredRespawnNearBail", "isRespawnNearBailActive", b);
                }
            );
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.R, 0.2f, () => {
                    Main.settings.RespawnNearBail = !Main.settings.RespawnNearBail;
                    uiLabelRespawnNearBail.SetToggleValue(Main.settings.RespawnNearBail);
                    XLShredDataRegistry.SetData("kiwi.XLShredRespawnNearBail", "isRespawnNearBailActive", Main.settings.RespawnNearBail);
                    if (Main.settings.RespawnNearBail) {
                        ModMenu.Instance.ShowMessage("Respawn Near Bail: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Respawn Near Bail: OFF");
                    }
                });
            }
        }

        public void OnDestroy() {
            uiBox.RemoveLabel("respawn-near-bail");
        }
    }
}
