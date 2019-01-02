using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace XLShredRealisticVert.Patches {
    [HarmonyPatch(typeof(PlayerController), "OnPop")]
    [HarmonyPatch(new [] { typeof(float), typeof(float)})]
    static class PlayerController_Pop_Patch {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();

            for (int i = 0; i < codes.Count; i++) {
                var inst = codes[i];

                if (inst.opcode == OpCodes.Call
                    && (MethodInfo)inst.operand == AccessTools.Method(typeof(PlayerController), "SetSkaterToMaster")) {

                    yield return inst;
                    yield return new CodeInstruction(OpCodes.Ret);
                    break;
                }

                yield return inst;
            }
        }

        static void Postfix(PlayerController __instance, float p_pop, float p_scoop) {
            if (Main.settings.realisticVert && Main.enabled) {
                __instance.skaterController.skaterRigidbody.AddForce(new Vector3(0f, 1f, 0f) * p_pop, ForceMode.Impulse);
                SoundManager.Instance.PlayPopSound(p_scoop);
                __instance.comController.popForce = new Vector3(0f, 1f, 0f) * p_pop;
            } else {
                __instance.skaterController.skaterRigidbody.AddForce(__instance.skaterController.skaterTransform.up * p_pop, ForceMode.Impulse);
                SoundManager.Instance.PlayPopSound(p_scoop);
                __instance.comController.popForce = __instance.skaterController.skaterTransform.up * p_pop;
            }
        }
    }

    [HarmonyPatch(typeof(PlayerController), "OnPop")]
    [HarmonyPatch(new [] { typeof(float), typeof(float), typeof(Vector3) })]
    static class PlayerController_Pop_PopDir_Patch {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();

            for (int i = 0; i < codes.Count; i++) {
                var inst = codes[i];

                if (inst.opcode == OpCodes.Call
                    && (MethodInfo)inst.operand == AccessTools.Method(typeof(PlayerController), "SetSkaterToMaster")) {

                    yield return inst;
                    yield return new CodeInstruction(OpCodes.Ret);
                    break;
                }

                yield return inst;
            }
        }

        static void Postfix(PlayerController __instance, float p_pop, float p_scoop, Vector3 p_popOutDir) {
            if (Main.settings.realisticVert && Main.enabled) {
                __instance.skaterController.skaterRigidbody.AddForce((new Vector3(0f, 1f, 0f) + p_popOutDir) * p_pop, ForceMode.Impulse);
                SoundManager.Instance.PlayPopSound(p_scoop);
                __instance.comController.popForce = (new Vector3(0f, 1f, 0f) + p_popOutDir) * p_pop;
            } else {
                __instance.skaterController.skaterRigidbody.AddForce((__instance.skaterController.skaterTransform.up + p_popOutDir) * p_pop, ForceMode.Impulse);
                SoundManager.Instance.PlayPopSound(p_scoop);
                __instance.comController.popForce = (__instance.skaterController.skaterTransform.up + p_popOutDir) * p_pop;
            }
        }
    }
}
