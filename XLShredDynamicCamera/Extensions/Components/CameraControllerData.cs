using UnityEngine;
using Harmony12;
using System;
using System.Reflection;

namespace XLShredDynamicCamera.Extensions.Components {

    public class CameraControllerData : MonoBehaviour {

        public bool inGrindCamera = false;

        private Traverse cObj;
        private Traverse<Vector3> _actualCam_posField;
        private Traverse<Quaternion> _actualCam_rotField;
        private Traverse<Vector3> _rightTopPos_posField;
        private Traverse<Quaternion> _rightTopPos_rotField;
        private Traverse<Vector3> _leftTopPos_posField;
        private Traverse<Quaternion> _leftTopPos_rotField;
        private Traverse<bool> _rightField;

        private CameraController _cameraController;

        public CameraController CameraControllerComponent {
            get { return _cameraController; }
            set {
                _cameraController = value;
                cObj = Traverse.Create(_cameraController);
                _actualCam_posField = cObj.Field("_actualCam").Property<Vector3>("position");
                _actualCam_rotField = cObj.Field("_actualCam").Property<Quaternion>("rotation");
                _rightTopPos_posField = cObj.Field("_rightTopPos").Property<Vector3>("position");
                _rightTopPos_rotField = cObj.Field("_rightTopPos").Property<Quaternion>("rotation");
                _leftTopPos_posField = cObj.Field("_leftTopPos").Property<Vector3>("position");
                _leftTopPos_rotField = cObj.Field("_leftTopPos").Property<Quaternion>("rotation");
                _rightField = cObj.Field<bool>("_right");
            }
        }

        private Vector3 ActualCamPos {
            get { return _actualCam_posField.Value; }
            set { _actualCam_posField.Value = value; }
        }

        private Quaternion ActualCamRot {
            get { return _actualCam_rotField.Value; }
            set { _actualCam_rotField.Value = value; }
        }

        private Vector3 RightTopPos {
            get { return _rightTopPos_posField.Value; }
            set { _rightTopPos_posField.Value = value; }
        }

        private Quaternion RightTopRot {
            get { return _rightTopPos_rotField.Value; }
            set { _actualCam_rotField.Value = value; }
        }

        private Vector3 LeftTopPos {
            get { return _rightTopPos_posField.Value; }
            set { _rightTopPos_posField.Value = value; }
        }

        private Quaternion LeftTopRot {
            get { return _rightTopPos_rotField.Value; }
            set { _actualCam_rotField.Value = value; }
        }

        private bool Right {
            get { return _rightField.Value; }
            set { _rightField.Value = value; }
        }

        public void AdjustCameraToGrind(bool backside) {
            
            if (!Main.settings.CameraModActive || !Main.enabled) {
                return;
            }
            
            this.inGrindCamera = true;

            if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
                if (PlayerController.Instance.IsSwitch) {
                    if (backside) {
                        ActualCamPos = Vector3.Lerp(ActualCamPos, RightTopPos, Time.fixedDeltaTime * 4f);
                        ActualCamRot = Quaternion.Slerp(ActualCamRot, RightTopRot, Time.fixedDeltaTime * 4f);
                        Right = true;
                        return;
                    }
                    ActualCamPos = Vector3.Lerp(ActualCamPos, LeftTopPos, Time.fixedDeltaTime * 4f);
                    ActualCamRot = Quaternion.Slerp(ActualCamRot, LeftTopRot, Time.fixedDeltaTime * 4f);
                    Right = false;
                    return;
                } else {
                    if (!backside) {
                        ActualCamPos = Vector3.Lerp(ActualCamPos, RightTopPos, Time.fixedDeltaTime * 4f);
                        ActualCamRot = Quaternion.Slerp(ActualCamRot, RightTopRot, Time.fixedDeltaTime * 4f);
                        Right = true;
                        return;
                    }
                    ActualCamPos = Vector3.Lerp(ActualCamPos, LeftTopPos, Time.fixedDeltaTime * 4f);
                    ActualCamRot = Quaternion.Slerp(ActualCamRot, LeftTopRot, Time.fixedDeltaTime * 4f);
                    Right = false;
                    return;
                }
            } else if (PlayerController.Instance.IsSwitch) {
                if (!backside) {
                    ActualCamPos = Vector3.Lerp(ActualCamPos, RightTopPos, Time.fixedDeltaTime * 4f);
                    ActualCamRot = Quaternion.Slerp(ActualCamRot, RightTopRot, Time.fixedDeltaTime * 4f);
                    Right = true;
                    return;
                }
                ActualCamPos = Vector3.Lerp(ActualCamPos, LeftTopPos, Time.fixedDeltaTime * 4f);
                ActualCamRot = Quaternion.Slerp(ActualCamRot, LeftTopRot, Time.fixedDeltaTime * 4f);
                Right = false;
                return;
            } else {
                if (!backside) {
                    ActualCamPos = Vector3.Lerp(ActualCamPos, LeftTopPos, Time.fixedDeltaTime * 4f);
                    ActualCamRot = Quaternion.Slerp(ActualCamRot, LeftTopRot, Time.fixedDeltaTime * 4f);
                    Right = false;
                    return;
                }
                ActualCamPos = Vector3.Lerp(ActualCamPos, RightTopPos, Time.fixedDeltaTime * 4f);
                ActualCamRot = Quaternion.Slerp(ActualCamRot, RightTopRot, Time.fixedDeltaTime * 4f);
                Right = true;
                return;
            }
        }

        public void ResetGrindCamera() {

            if (!Main.settings.CameraModActive || !Main.enabled) {
                return;
            }

            if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy && Right) {
                ActualCamPos = Vector3.Lerp(ActualCamPos, LeftTopPos, Time.fixedDeltaTime * 1f);
                ActualCamRot = Quaternion.Slerp(ActualCamRot, LeftTopRot, Time.fixedDeltaTime * 1f);
                Right = false;
                inGrindCamera = false;
                return;
            }

            if (!Right) {
                ActualCamPos = Vector3.Lerp(ActualCamPos, RightTopPos, Time.fixedDeltaTime * 1f);
                ActualCamRot = Quaternion.Slerp(ActualCamRot, RightTopRot, Time.fixedDeltaTime * 1f);
                Right = true;
                inGrindCamera = false;
                return;
            }
        }

        public void ChangeCameraToFront() {
            if (!inGrindCamera && Main.settings.CameraModActive && Main.enabled) {
                if (PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
                    ActualCamPos = Vector3.Lerp(ActualCamPos, RightTopPos, Time.fixedDeltaTime * 2f);
                    ActualCamRot = Quaternion.Slerp(ActualCamRot, RightTopRot, Time.fixedDeltaTime * 2f);
                    Right = true;
                }
                if (!PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
                    ActualCamPos = Vector3.Lerp(ActualCamPos, LeftTopPos, Time.fixedDeltaTime * 2f);
                    ActualCamRot = Quaternion.Slerp(ActualCamRot, LeftTopRot, Time.fixedDeltaTime * 2f);
                    Right = false;
                }
                if (PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                    ActualCamPos = Vector3.Lerp(ActualCamPos, LeftTopPos, Time.fixedDeltaTime * 2f);
                    ActualCamRot = Quaternion.Slerp(ActualCamRot, LeftTopRot, Time.fixedDeltaTime * 2f);
                    Right = false;
                }
                if (!PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                    ActualCamPos = Vector3.Lerp(ActualCamPos, RightTopPos, Time.fixedDeltaTime * 2f);
                    ActualCamRot = Quaternion.Slerp(ActualCamRot, RightTopRot, Time.fixedDeltaTime * 2f);
                    Right = true;
                }
            }
        }
    }
}
