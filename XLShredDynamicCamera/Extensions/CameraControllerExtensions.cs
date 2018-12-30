using UnityEngine;
using Harmony12;
using System;

namespace XLShredDynamicCamera.Extensions {

    using Components;

    public static class CameraControllerExtensions {
        public static CameraControllerData GetExtensionComponent(this CameraController ob) {
            return ob.GetComponent<CameraControllerData>();
        }
    }
}
