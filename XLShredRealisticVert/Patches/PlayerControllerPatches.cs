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
                Vector3 popDir = new Vector3(0f, 1f, 0f) * p_pop;
                Rigidbody skaterBody = __instance.skaterController.skaterRigidbody;
                Vector3 p_up = __instance.boardController.GroundNormal;

                if (Vector3.Angle(__instance.GetGroundNormal(), Vector3.up) > 26.0f) {
                    Vector3 forwardNoY = new Vector3(-__instance.GetGroundNormal().x, 0, -__instance.GetGroundNormal().z).normalized;
                    float z = Vector3.Project(skaterBody.velocity, forwardNoY).magnitude * __instance.skaterController.skaterRigidbody.mass;
                   // float y = z / Mathf.Tan(Mathf.Asin(z / p_pop));
                    popDir = (-forwardNoY.normalized * Mathf.Max(z - 0.5f, 0f)) + (Vector3.up * p_pop);
                        //popDir.x = (Mathf.Abs(popDir.x) >= Mathf.Abs(forwardNoY.x * z)) ? popDir.x : -forwardNoY.x * z;
                        //popDir.z = (Mathf.Abs(popDir.z) >= Mathf.Abs(forwardNoY.z * z)) ? popDir.x : -forwardNoY.z * z;
                    
                }
                Console.WriteLine(Vector3.Angle(__instance.GetGroundNormal(), Vector3.up));
                __instance.skaterController.skaterRigidbody.AddForce(popDir, ForceMode.Impulse);
                SoundManager.Instance.PlayPopSound(p_scoop);
                __instance.comController.popForce = popDir;



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

    [HarmonyPatch(typeof(SkaterController), "InAirRotationLogic")]
    static class SkaterController_InAirRotationLogic_Patch {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            yield return new CodeInstruction(OpCodes.Nop);
        }

        static void Prefix(SkaterController __instance, bool ____landingPrediction, ref float ____startTime, ref float ____duration,
            ref Quaternion ____startRotation, ref Vector3 ____startUpVector, ref Quaternion ____newUp) {
            if (!____landingPrediction) {
                ____startTime = Time.time;
                ____duration = PlayerController.Instance.boardController.trajectory.CalculateTrajectory(__instance.skaterTransform.position - Vector3.up * 0.9765f, __instance.skaterRigidbody, 50f);
                ____startRotation = __instance.skaterRigidbody.rotation;
                ____startUpVector = __instance.skaterTransform.up;
                ____landingPrediction = true;
                //____newUp = Quaternion.FromToRotation(____startUpVector, Vector3.up);
                ____newUp = Quaternion.identity;
                ____newUp *= __instance.skaterRigidbody.rotation;
                ((MonoBehaviour) __instance).Invoke("PreLandingEvent", ____duration - 0.3f);
            }

            AccessTools.Method(typeof(SkaterController), "InAirRotation").Invoke(__instance, new object[] { Mathf.Clamp((Time.time - ____startTime) / ____duration, 0f, 1f) });
        }

    }
}
