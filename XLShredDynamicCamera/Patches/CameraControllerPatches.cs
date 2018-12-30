using UnityEngine;
using Harmony12;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System;

using XLShredDynamicCamera.Extensions;
using XLShredDynamicCamera.Extensions.Components;

namespace XLShredDynamicCamera.Patches {

    [HarmonyPatch(typeof(CameraController), "MoveCameraToPlayer")]
    static class CameraController_MoveCameraToPlayer_Patch {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            var codes = instructions.ToList();

            for (int i = 0; i < codes.Count; i++) {
                var inst = codes[i];

                if (inst.opcode == OpCodes.Stfld
                    && (FieldInfo)inst.operand == AccessTools.Field(typeof(CameraController), "_projectedVelocity")
                    && codes[i - 1].opcode == OpCodes.Call
                    && (MethodInfo)codes[i - 1].operand == AccessTools.Method(typeof(Vector3), nameof(Vector3.ProjectOnPlane))) {
                    yield return inst;
                    break;
                }

                yield return inst;
            }
        }

        static void Postfix(CameraController __instance, ref Vector3 ____projectedVelocity, ref Vector3 ____forwardTarget, ref Transform ____camTransform, ref Transform ____actualCam, ref Transform ____rightTopPos, ref Transform ____leftTopPos, ref bool ____right) {
            if (____projectedVelocity.magnitude > 0.3f) {
                ____forwardTarget = PlayerController.Instance.boardController.boardTransform.position + ____projectedVelocity * 10f;
                Quaternion quaternion = Quaternion.FromToRotation(____camTransform.forward, ____projectedVelocity);
                quaternion *= ____camTransform.rotation;
                ____camTransform.rotation = Quaternion.Slerp(____camTransform.rotation, quaternion, Time.fixedDeltaTime * 10f);
                Quaternion quaternion2 = Quaternion.FromToRotation(____camTransform.up, Vector3.up);
                quaternion2 *= ____camTransform.rotation;
                ____camTransform.rotation = Quaternion.Slerp(____camTransform.rotation, quaternion2, Time.fixedDeltaTime * 10f);
            }
            if (PlayerController.Instance.inputController.player.GetAxis("DPadX") < 0f) {
                ____right = false;
            } else if (PlayerController.Instance.inputController.player.GetAxis("DPadX") > 0f) {
                ____right = true;
            }
            __instance.GetExtensionComponent().ChangeCameraToFront();
            if (____right) {
                ____actualCam.position = Vector3.Lerp(____actualCam.position, ____rightTopPos.position, Time.fixedDeltaTime * 4f);
                ____actualCam.rotation = Quaternion.Slerp(____actualCam.rotation, ____rightTopPos.rotation, Time.fixedDeltaTime * 4f);
                return;
            }
            ____actualCam.position = Vector3.Lerp(____actualCam.position, ____leftTopPos.position, Time.fixedDeltaTime * 4f);
            ____actualCam.rotation = Quaternion.Slerp(____actualCam.rotation, ____leftTopPos.rotation, Time.fixedDeltaTime * 4f);
        }
    }
}
