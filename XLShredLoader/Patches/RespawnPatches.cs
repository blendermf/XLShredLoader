using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using XLShredLoader.Extensions.Components;

namespace XLShredLoader.Patches {
    [HarmonyPatch(typeof(Respawn), "DoRespawn")]
    static class Respawn_DoRespawn_Patch {

        static void Prefix(Respawn __instance, bool ____canPress) {
            if (____canPress && !__instance.respawning) {
                PlayerControllerData.Instance.resetSpinVelocity();
            }
        }
    }

    [HarmonyPatch(typeof(Respawn), "EndRespawning")]
    static class Respawn_EndRespawning_Patch {

        static void Prefix() {
            PlayerControllerData.Instance.resetSpinVelocity();
        }
    }
}
