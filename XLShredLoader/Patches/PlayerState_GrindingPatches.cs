using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using XLShredLoader.Extensions;
using XLShredLoader.Extensions.Components;

namespace XLShredLoader.Patches {
    [HarmonyPatch(typeof(PlayerState_Grinding), "Enter")]
    static class PlayerState_Grinding_Enter_Patch {

        static void Prefix(ref float ____popForce) {
            ____popForce = Main.settings.customGrindPopForce;
            PlayerControllerData.Instance.resetSpinVelocity();
            PlayerController.Instance.cameraController.GetExtensionComponent().adjustCameraToGrind(PlayerController.Instance.IsBacksideGrind());
        }
    }

    [HarmonyPatch(typeof(PlayerState_Grinding), "Exit")]
    static class PlayerState_Grinding_Exit_Patch {

        static void Prefix() {
            PlayerControllerData.Instance.resetSpinVelocity();
            PlayerController.Instance.cameraController.GetExtensionComponent().ResetGrindCamera();
        }
    }

    [HarmonyPatch(typeof(PlayerState_Grinding), "LeftTriggerHeld")]
    static class PlayerState_Grinding_LeftTriggerHeld_Patch {
        static void Prefix(PlayerState_Grinding __instance, ref float ____leftTrigger, float p_value) {
            if (Main.settings.grindSpinVelocityEnabled) {
                Traverse tObj = Traverse.Create(__instance);
                tObj.Method("RotatePlayer", -p_value * 2f ).GetValue();
                ____leftTrigger = p_value * 2f;
                return;
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState_Grinding), "RightTriggerHeld")]
    static class PlayerState_Grinding_RightTriggerHeld_Patch {
        static void Prefix(PlayerState_Grinding __instance, ref float ____rightTrigger, float p_value) {
            if (Main.settings.grindSpinVelocityEnabled) {
                Traverse tObj = Traverse.Create(__instance);
                tObj.Method("RotatePlayer", p_value * 2f).GetValue();
                ____rightTrigger = p_value * 2f;
                return;
            }
        }
    }
}
