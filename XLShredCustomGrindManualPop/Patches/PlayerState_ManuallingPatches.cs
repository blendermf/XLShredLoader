using Harmony12;

namespace XLShredCustomGrindManualPop.Patches {

    [HarmonyPatch(typeof(PlayerState_Manualling), "Enter")]
    static class PlayerState_Manualling_Enter_Patch {

        static void Prefix(ref float ____popForce) {
            if (Main.enabled) {
                ____popForce = Main.settings.customManualPopForce;
            }
        }
    }
}
