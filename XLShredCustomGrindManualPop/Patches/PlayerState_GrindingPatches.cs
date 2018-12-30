using Harmony12;

namespace XLShredCustomGrindManualPop.Patches {

    [HarmonyPatch(typeof(PlayerState_Grinding), "Enter")]
    static class PlayerState_Grinding_Enter_Patch {

        static void Prefix(ref float ____popForce) {
            if (Main.enabled) {
                ____popForce = Main.settings.customGrindPopForce;
            }
        }
    }
}
