using UnityEngine;
using Harmony12;
using System;

namespace XLShredFlipMods.Extensions {
    public static class PlayerControllerExtensions {
        public static void FixedSwitchFlipPositions(this BoardController ob, float p_value) {
            Traverse tObj = Traverse.Create(ob).Field("_flipAxisTarget");

            if (Main.settings.fixedSwitchFlipPositions && PlayerController.Instance.IsSwitch && Main.enabled) {
                tObj.SetValue(-p_value);
            } else {
                tObj.SetValue(p_value);
            }
        }
    }
}
