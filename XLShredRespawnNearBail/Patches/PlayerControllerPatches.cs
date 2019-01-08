using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using XLShredRespawnNearBail.Extensions;

namespace XLShredRespawnNearBail.Patches {
    
    [HarmonyPatch(typeof(PlayerController), "DoBailDelay")]
    static class PlayerController_DoBailDelay_Patch {

        static bool Prefix(PlayerController __instance) {
            if (Main.settings.respawnNearBail && Main.enabled) {
                __instance.respawn.GetExtensionComponent().DoBailTmpCoroutine = __instance.StartCoroutine(__instance.DoBailTmp());
                return false;
            } else return true;
        }
    }
}
