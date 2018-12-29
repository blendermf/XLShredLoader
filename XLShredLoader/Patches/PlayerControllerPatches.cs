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
    [HarmonyPatch(typeof(PlayerController), "TurnLeft")]
    static class PlayerController_TurnLeft_Patch {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Ldarg_1);
            yield return new CodeInstruction(OpCodes.Ldarg_2);
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerControllerExtensions), nameof(PlayerControllerExtensions.TurnLeftModified)));
            yield return new CodeInstruction(OpCodes.Ret);
        }
    }

    [HarmonyPatch(typeof(PlayerController), "TurnRight")]
    static class PlayerController_TurnRight_Patch {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            yield return new CodeInstruction(OpCodes.Ldarg_0);
            yield return new CodeInstruction(OpCodes.Ldarg_1);
            yield return new CodeInstruction(OpCodes.Ldarg_2);
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerControllerExtensions), nameof(PlayerControllerExtensions.TurnRightModified)));
            yield return new CodeInstruction(OpCodes.Ret);
        }
    }

    [HarmonyPatch(typeof(PlayerController), "AnimSetManual")]
    static class PlayerController_AnimSetManual_Patch {
        static void Postfix() {
            PlayerControllerData.Instance.resetSpinVelocity();
        }
    }

    [HarmonyPatch(typeof(PlayerController), "AnimSetFlip")]
    static class PlayerController_AnimSetFlip_Patch {
        static bool Prefix(PlayerController __instance, float p_value, ref float ____flipAxisTarget) {
            if (Main.settings.fixedSwitchFlipPositions && PlayerController.Instance.IsSwitch) {
                    ____flipAxisTarget = -p_value;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerController), "CanOllieOutOfGrind")]
    static class PlayerController_CanOllieOutOfGrind_Patch {
        static void Prefix() {
            PlayerControllerData.Instance.resetSpinVelocity();
        }
    }

    [HarmonyPatch(typeof(PlayerController), "CanNollieOutOfGrind")]
    static class PlayerController_CanNollieOutOfGrind_Patch {
        static void Prefix() {
            PlayerControllerData.Instance.resetSpinVelocity();
        }
    }
}
