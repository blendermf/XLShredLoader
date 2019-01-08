using System;
using Harmony12;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using UnityEngine;

using XLShredRespawnNearBail.Extensions;
using XLShredRespawnNearBail.Extensions.Components;

namespace XLShredRespawnNearBail.Patches {
    [HarmonyPatch(typeof(Respawn), "Update")]
    static class Respawn_Update_Patch {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();
            var skipCount = 12;

            for (int i = 0; i < codes.Count; i++) {
                var inst = codes[i];

                if (skipCount > 0) {
                    skipCount--;
                    continue;
                }

                yield return inst;
            }
        }
        
        static void Prefix(Respawn __instance, ref bool ____init) {
            RespawnData respawnData = PlayerController.Instance.respawn.GetExtensionComponent();

            if (!____init && PlayerController.Instance.boardController.AllDown) {
                respawnData.SetSpawnPos();
                ____init = true;
            }
            if (Main.enabled && Main.settings.respawnNearBail && !__instance.respawning && !__instance.puppetMaster.isBlending && Time.time - respawnData.lastTmpSave > 0.5f && PlayerController.Instance.IsGrounded() && !__instance.bail.bailed && Time.timeScale != 0f) {
                respawnData.lastTmpSave = Time.time;
                respawnData.SetTmpSpawnPos();
            }
        }
    }
}
