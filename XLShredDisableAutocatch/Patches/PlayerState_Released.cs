using Harmony12;

namespace XLShredDisableAutocatch.Patches {

    [HarmonyPatch(typeof(PlayerState_Released), "Update")]
    static class PlayerState_Released_Patch {

        static bool Prefix() {
            if (Main.enabled && Main.settings.disableAutocatch) {
                return false;
            }
            return true;
        }
    }
}
