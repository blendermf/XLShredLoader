using UnityEngine;
using System;

namespace XLShredLoader {

    public class ModMenu : MonoBehaviour {
        private float btnLastPressed;
        private ModMenu.TmpMessage tmpMessage;
        private GUIStyle fontLarge;

        public void Start() {
            this.fontLarge = new GUIStyle();
            this.fontLarge.fontSize = 32;
            this.fontLarge.normal.textColor = Color.red;
        }

        public void Update() {
            float realtimeSinceStartup = Time.realtimeSinceStartup;

            if (Input.GetKey(KeyCode.N) && (double)(realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = realtimeSinceStartup;
                Main.settings.realisticFlipTricks = !Main.settings.realisticFlipTricks;
                if (Main.settings.realisticFlipTricks) {
                    this.showMessage("Realistic Flip Tricks: ACTIVATED");
                } else {
                    this.showMessage("Realistic Flip Tricks: DEACTIVATED");
                }
            }
            if (Input.GetKey(KeyCode.M) && (double)(realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = realtimeSinceStartup;
                Main.settings.fixedSwitchFlipPositions = !Main.settings.fixedSwitchFlipPositions;
                if (Main.settings.fixedSwitchFlipPositions) {
                    this.showMessage("Switch kickflip/heelflip positions CHANGED");
                } else {
                    this.showMessage("Switch kickflip/heelflip positions set to DEFAULT");
                }
            }
        }

        private void OnGUI() {
            float num = 20f;
            float num2 = 20f;
            float width = 750f;

            if (this.tmpMessage != null) {
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                GUI.color = Color.white;
                GUI.Label(new Rect(20f, (float)(Screen.height - 50), 600f, 100f), this.tmpMessage.msg, this.fontLarge);
                if (realtimeSinceStartup - this.tmpMessage.epoch > 1f) {
                    this.tmpMessage = null;
                }
            }
        }

        private void showMessage(string msg) {
            Console.WriteLine(msg);
            float realtimeSinceStartup = Time.realtimeSinceStartup;
            this.tmpMessage = new ModMenu.TmpMessage {
                msg = msg,
                epoch = realtimeSinceStartup
            };
        }

        private class TmpMessage {
            public string msg { get; set; }

            public float epoch { get; set; }
        }
    }
}
