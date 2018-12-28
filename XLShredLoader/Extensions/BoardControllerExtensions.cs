using UnityEngine;
using Harmony12;
using System;

namespace XLShredLoader.Extensions {
    public static class BoardControllerExtensions {
        public static void RealisticFlipTricks(this BoardController ob) {
            if (Main.settings.realisticFlipTricks && Main.enabled) {

                if (ob.secondVel < 10f) {
                    ob.RotateBoardWithSkater();
                }
            } else {
                ob.RotateBoardWithSkater();
            }
        }

        public static void FixedSwitchPositions(this BoardController ob) {
            
            Traverse tObj = Traverse.Create(ob);
            float bufferedFlip = tObj.Field("_bufferedFlip").GetValue<float>();
            float thirdDelta = tObj.Field("_thirdDelta").GetValue<float>();

            if (Main.settings.fixedSwitchFlipPositions && PlayerController.Instance.IsSwitch && Main.enabled) {
                tObj.Field("_bufferedFlip").SetValue(bufferedFlip - thirdDelta);
            } else {
                tObj.Field("_bufferedFlip").SetValue(bufferedFlip + thirdDelta);
            }
        }

        public static void RotateBoardWithSkater(this BoardController ob) {

            Vector3 vector = Mathd.LocalAngularVelocity(PlayerController.Instance.skaterController.skaterRigidbody);
            Quaternion rhs = Quaternion.AngleAxis(57.29578f * vector.y * Time.deltaTime, PlayerController.Instance.skaterController.skaterTransform.up);

            Traverse tObj = Traverse.Create(ob);
            Quaternion bufferedRotation = tObj.Field("_bufferedRotation").GetValue<Quaternion>();

            tObj.Field("_bufferedRotation").SetValue(bufferedRotation * rhs);
        }
    }
}
