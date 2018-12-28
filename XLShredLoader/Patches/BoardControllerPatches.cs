using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using XLShredLoader.Extensions;

namespace XLShredLoader.Patches {
    
    [HarmonyPatch(typeof(BoardController), "Rotate")]
    static class BoardController_Rotate_Patch {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();
            int bufferedRotationMultCnt = 0;
            int skipCount = 0;
            for (int i = 0; i < codes.Count; i++) {
                var inst = codes[i];

                if (skipCount > 0) {
                    skipCount--;
                    continue;
                }

                if (inst.opcode == OpCodes.Stfld
                    && (FieldInfo)inst.operand == AccessTools.Field(typeof(BoardController), "_bufferedRotation")
                    && codes[i - 1].opcode == OpCodes.Call
                    && bufferedRotationMultCnt == 0) {
                    skipCount = 23;

                    bufferedRotationMultCnt++;

                    yield return inst;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BoardControllerExtensions), nameof(BoardControllerExtensions.RealisticFlipTricks)));
                    continue;
                }

                if (inst.opcode == OpCodes.Callvirt
                    && (MethodInfo)inst.operand == AccessTools.Property(typeof(Transform), "rotation").GetSetMethod()) {
                    skipCount = 7;

                    yield return inst;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BoardControllerExtensions), nameof(BoardControllerExtensions.FixedSwitchPositions)));
                    continue;
                }

                yield return inst;
            }
        }
    }
}
