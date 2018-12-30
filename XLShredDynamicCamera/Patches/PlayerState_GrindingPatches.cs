using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using XLShredDynamicCamera.Extensions;
using XLShredDynamicCamera.Extensions.Components;

namespace XLShredDynamicCamera.Patches {
    [HarmonyPatch(typeof(PlayerState_Grinding), "Enter")]
    static class PlayerState_Grinding_Enter_Patch {

        static void Prefix(ref float ____popForce) {
            PlayerController.Instance.cameraController.GetExtensionComponent().adjustCameraToGrind(PlayerController.Instance.IsBacksideGrind());
        }
    }

    [HarmonyPatch(typeof(PlayerState_Grinding), "Exit")]
    static class PlayerState_Grinding_Exit_Patch {

        static void Prefix() {
            PlayerController.Instance.cameraController.GetExtensionComponent().ResetGrindCamera();
        }
    }
}
