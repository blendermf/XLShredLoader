using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredCustomGrindManualPop {
    class XLShredCustomGrindManualPop : MonoBehaviour {
        ModUIBox uiBox;
        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("commander_klepto", "Commander Klepto");
            uiBox.AddLabel("adjust-grind-pop-force", "UP/DOWN - Adjust Grind Pop Force", Side.left, () => { return Main.enabled; });
            uiBox.AddLabel("adjust-manual-pop-force", "LEFT/RIGHT - Adjust Manual Pop Force", Side.left, () => { return Main.enabled; });
        }

        public void Update() {
            if (Main.enabled) {

                ModMenu.Instance.KeyPress(KeyCode.UpArrow, 0.2f, () => {
                    Main.settings.IncreaseGrindPopForce();
                    ModMenu.Instance.ShowMessage("Grind Pop Force: " + Main.settings.customGrindPopForce + " Default: 2.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.DownArrow, 0.2f, () => {
                    Main.settings.DecreaseGrindPopForce();
                    ModMenu.Instance.ShowMessage("Grind Pop Force: " + Main.settings.customGrindPopForce + " Default: 2.0");
                });

                ModMenu.Instance.KeyPress(KeyCode.RightArrow, 0.2f, () => {
                    Main.settings.IncreaseManualPopForce();
                    ModMenu.Instance.ShowMessage("Manual Pop Force: " + Main.settings.customManualPopForce + " Default: 2.5");
                });

                ModMenu.Instance.KeyPress(KeyCode.LeftArrow, 0.2f, () => {
                    Main.settings.DecreaseManualPopForce();
                    ModMenu.Instance.ShowMessage("Manual Pop Force: " + Main.settings.customManualPopForce + " Default: 2.5");
                });
            }
        }

        public void OnDestroy() {
            uiBox.RemoveLabel("adjust-grind-pop-force");
            uiBox.RemoveLabel("adjust-manual-pop-force");
        }

    }
}
