using Harmony12;

using XLShredFasterSpin.Extensions.Components;

namespace XLShredFasterSpin.Patches {

    [HarmonyPatch(typeof(PlayerState_Grinding), "Enter")]
    static class PlayerState_Grinding_Enter_Patch {

        static void Prefix(ref float ____popForce) {
            if (Main.enabled) {
                PlayerControllerData.Instance.resetSpinVelocity();
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState_Grinding), "Exit")]
    static class PlayerState_Grinding_Exit_Patch {

        static void Prefix() {
            if (Main.enabled) {
                PlayerControllerData.Instance.resetSpinVelocity();
            }
        }
    }


    [HarmonyPatch(typeof(PlayerState_Grinding), "LeftTriggerHeld")]
    static class PlayerState_Grinding_LeftTriggerHeld_Patch {
        static bool Prefix(PlayerState_Grinding __instance, ref float ____leftTrigger, float p_value) {
            if (Main.settings.grindSpinVelocityEnabled && Main.enabled) {
                Traverse tObj = Traverse.Create(__instance);
                tObj.Method("RotatePlayer", -p_value * 2f).GetValue();
                ____leftTrigger = p_value * 2f;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerState_Grinding), "RightTriggerHeld")]
    static class PlayerState_Grinding_RightTriggerHeld_Patch {
        static bool Prefix(PlayerState_Grinding __instance, ref float ____rightTrigger, float p_value) {
            if (Main.settings.grindSpinVelocityEnabled && Main.enabled) {
                Traverse tObj = Traverse.Create(__instance);
                tObj.Method("RotatePlayer", p_value * 2f).GetValue();
                ____rightTrigger = p_value * 2f;
                return false;
            }
            return true;
        }
    }
}
