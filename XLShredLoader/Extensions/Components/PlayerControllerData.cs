using UnityEngine;
using System;


namespace XLShredLoader.Extensions.Components {

    public class PlayerControllerData : MonoBehaviour {

        private static PlayerControllerData _instance;
        private float spinVelocity;

        public static PlayerControllerData Instance {
            get {
                return PlayerControllerData._instance;
            }
        }

        private void Awake() {
            Cursor.visible = false;
            if (PlayerControllerData._instance != null && PlayerControllerData._instance != this) {
                Destroy(this);
                return;
            }
            PlayerControllerData._instance = this;
        }

        public float getSpinVelocity() {
            return this.spinVelocity;
        }

        public void resetSpinVelocity() {
            this.spinVelocity = 0f;
        }

        public void addSpinVelocity(string side) {
            if (side == "right") {
                this.spinVelocity += 0.25f;
                return;
            }
            this.spinVelocity -= 0.25f;
        }
    }
}
