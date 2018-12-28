using UnityEngine;
using Harmony12;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System;

using XLShredLoader.Extensions;
using XLShredLoader.Extensions.Components;

namespace XLShredLoader.Patches {

    [HarmonyPatch(typeof(CameraController), "MoveCameraToPlayer")]
    static class CameraController_MoveCameraToPlayer_Patch {
        
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();

            for (int i = 0; i < codes.Count; i++) {
                var inst = codes[i];

                if (inst.opcode == OpCodes.Stfld
                    && (FieldInfo)inst.operand == AccessTools.Field(typeof(CameraController), "_right")
                    && codes[i - 1].opcode == OpCodes.Ldc_I4_1) {

                    yield return inst;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CameraControllerExtensions), nameof(CameraControllerExtensions.ChangeCameraToFront)));
                    continue;
                }

                yield return inst;
            }
        }
    }
}
