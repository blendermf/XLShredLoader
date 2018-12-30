using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using XLShredFasterSpin.Extensions.Components;

namespace XLShredFasterSpin.Patches {
    [HarmonyPatch(typeof(Respawn), "DoRespawn")]
    static class Respawn_DoRespawn_Patch {

        static void Prefix(Respawn __instance, bool ____canPress) {
            if (Main.enabled) {
                if (____canPress && !__instance.respawning) {
                    PlayerControllerData.Instance.resetSpinVelocity();
                }
            }
        }
    }

    [HarmonyPatch(typeof(Respawn), "EndRespawning")]
    static class Respawn_EndRespawning_Patch {

        static void Prefix() {
            if (Main.enabled) {
                PlayerControllerData.Instance.resetSpinVelocity();
            }
        }
    }
}
