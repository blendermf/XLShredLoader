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
                    break;
                }

                yield return inst;
            }
        }

        static void Postfix(PlayerController __instance, float p_pop, float p_scoop) {
            if (Main.settings.realisticVert && Main.enabled) {
                Vector3 popDir = Vector3.up * p_pop;
                Rigidbody skaterBody = __instance.skaterController.skaterRigidbody;
                Vector3 p_up = __instance.boardController.GroundNormal;
                Vector3 forwardNoY = new Vector3(-__instance.GetGroundNormal().x, 0, -__instance.GetGroundNormal().z).normalized;
                if (Vector3.Angle(__instance.GetGroundNormal(), Vector3.up) > 26.0f) {
                    float z = Vector3.Project(skaterBody.velocity, forwardNoY).magnitude * __instance.skaterController.skaterRigidbody.mass;
                    float angle_percent = Vector3.Angle(__instance.GetGroundNormal(), Vector3.up) / 80f;
                    popDir = (-forwardNoY.normalized * Mathf.Max(z - (0.4f * (1.0f - angle_percent)), 0f) * angle_percent) + (Vector3.up * p_pop);
                }
                Vector3 vector = popDir;
                Vector3 to = __instance.skaterController.skaterRigidbody.velocity + vector;
                Vector3.Angle(__instance.cameraController._actualCam.forward, to);
                Vector3 force = __instance.skaterController.PredictLanding(vector);
                __instance.skaterController.skaterRigidbody.AddForce(vector, ForceMode.Impulse);
                __instance.skaterController.skaterRigidbody.AddForce(force, ForceMode.VelocityChange);
                SoundManager.Instance.PlayPopSound(p_scoop);
                __instance.comController.popForce = vector;
            } else {
                Vector3 vector = __instance.skaterController.skaterTransform.up * p_pop;
                Vector3 to = __instance.skaterController.skaterRigidbody.velocity + vector;
                Vector3.Angle(__instance.cameraController._actualCam.forward, to);
                Vector3 force = __instance.skaterController.PredictLanding(vector);
                __instance.skaterController.skaterRigidbody.AddForce(vector, ForceMode.Impulse);
                __instance.skaterController.skaterRigidbody.AddForce(force, ForceMode.VelocityChange);
                SoundManager.Instance.PlayPopSound(p_scoop);
                __instance.comController.popForce = vector;
            }
        }
    }
}
