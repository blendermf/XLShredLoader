using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace XLShredDisablePushReduction.Patches {
    
    [HarmonyPatch(typeof(PlayerState_Pushing), "OnPush")]
    static class PlayerState_Pushing_OnPush_Patch {

        static bool Prefix(PlayerController __instance, ref float ____pushPower) {
            if (Main.settings.disablePushReduction && Main.enabled) {
                ____pushPower = 1f;
                PlayerController.Instance.AddPushForce(PlayerController.Instance.GetPushForce() * 1.8f * ____pushPower);
                ____pushPower = 0f;
                return false;
            }
            return true;
        }
    }
}
