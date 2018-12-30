using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace XLShredFlipMods.Patches {
    [HarmonyPatch(typeof(PlayerController), "AnimSetFlip")]
    static class PlayerController_AnimSetFlip_Patch {
        static bool Prefix(PlayerController __instance, float p_value, ref float ____flipAxisTarget) {
            if (Main.settings.fixedSwitchFlipPositions && PlayerController.Instance.IsSwitch && Main.enabled) {
                ____flipAxisTarget = -p_value;
                return false;
            }
            return true;
        }
    }
}
