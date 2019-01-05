using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using XLShredFlipMods.Extensions;

namespace XLShredFlipMods.Patches {
    [HarmonyPatch(typeof(PlayerController), "OnFlipStickUpdate")]
    static class PlayerController_OnFlipStickUpdate_Patch {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();
            for (int i = 0; i < codes.Count; i++) {
                var inst = codes[i];
                if (inst.opcode == OpCodes.Call
                     && (MethodInfo)inst.operand == AccessTools.Method(typeof(PlayerController), "AnimSetFlip")) {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerControllerExtensions), nameof(PlayerControllerExtensions.FixedSwitchFlipPositions)));
                    continue;
                }
                yield return inst;
            }
        }
    }
}
