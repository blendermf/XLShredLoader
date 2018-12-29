using UnityEngine;
using Harmony12;
using System;
using System.Reflection;

namespace XLShredLoader.Extensions.Components {

    public class CameraControllerData : MonoBehaviour {

        public float bloom;
        public bool inGrindCamera;

        private Traverse cObj;
        private Traverse<Vector3> _actualCam_pos;
        private Traverse<Quaternion> _actualCam_rot;
        private Traverse<Vector3> _rightTopPos_pos;
        private Traverse<Quaternion> _rightTopPos_rot;
        private Traverse<Vector3> _leftTopPos_pos;
        private Traverse<Quaternion> _leftTopPos_rot;
        private Traverse<bool> _right;

        public CameraController cameraController;

        public void Start() {
            cObj = Traverse.Create(cameraController);
            _actualCam_pos = cObj.Field("_actualCam").Property<Vector3>("position");
            _actualCam_rot = cObj.Field("_actualCam").Property<Quaternion>("rotation");
            _rightTopPos_pos = cObj.Field("_rightTopPos").Property<Vector3>("position");
            _rightTopPos_rot = cObj.Field("_rightTopPos").Property<Quaternion>("rotation");
            _leftTopPos_pos = cObj.Field("_leftTopPos").Property<Vector3>("position");
            _leftTopPos_rot = cObj.Field("_leftTopPos").Property<Quaternion>("rotation");
            _right = cObj.Field<bool>("_right");
        }

        public void adjustCameraToGrind(bool backside) {
            
            if (!Main.settings.GetCameraModActive()) {
                return;
            }

            Console.WriteLine("Camera Controller: " + cameraController);
            this.inGrindCamera = true;
            if (SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
                if (PlayerController.Instance.IsSwitch) {
                    if (backside) {
                        _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _rightTopPos_pos.Value, Time.fixedDeltaTime * 4f);
                        _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _rightTopPos_rot.Value, Time.fixedDeltaTime * 4f);
                        _right.Value = true;
                        return;
                    }
                    _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _leftTopPos_pos.Value, Time.fixedDeltaTime * 4f);
                    _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _leftTopPos_rot.Value, Time.fixedDeltaTime * 4f);
                    _right.Value = false;
                    return;
                } else {
                    if (!backside) {
                        _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _rightTopPos_pos.Value, Time.fixedDeltaTime * 4f);
                        _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _rightTopPos_rot.Value, Time.fixedDeltaTime * 4f);
                        _right.Value = true;
                        return;
                    }
                    _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _leftTopPos_pos.Value, Time.fixedDeltaTime * 4f);
                    _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _leftTopPos_rot.Value, Time.fixedDeltaTime * 4f);
                    _right.Value = false;
                    return;
                }
            } else if (PlayerController.Instance.IsSwitch) {
                if (!backside) {
                    _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _rightTopPos_pos.Value, Time.fixedDeltaTime * 4f);
                    _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _rightTopPos_rot.Value, Time.fixedDeltaTime * 4f);
                    _right.Value = true;
                    return;
                }
                _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _leftTopPos_pos.Value, Time.fixedDeltaTime * 4f);
                _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _leftTopPos_rot.Value, Time.fixedDeltaTime * 4f);
                _right.Value = false;
                return;
            } else {
                if (!backside) {
                    _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _leftTopPos_pos.Value, Time.fixedDeltaTime * 4f);
                    _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _leftTopPos_rot.Value, Time.fixedDeltaTime * 4f);
                    _right.Value = false;
                    return;
                }
                _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _rightTopPos_pos.Value, Time.fixedDeltaTime * 4f);
                _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _rightTopPos_rot.Value, Time.fixedDeltaTime * 4f);
                _right.Value = true;
                return;
            }
        }

        public void ResetGrindCamera() {
         
            if (!Main.settings.GetCameraModActive()) {
                return;
            }

            if (PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
                _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _rightTopPos_pos.Value, Time.fixedDeltaTime * 2f);
                _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _rightTopPos_rot.Value, Time.fixedDeltaTime * 2f);
                _right.Value = true;
            }

            if (!PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
                _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _leftTopPos_pos.Value, Time.fixedDeltaTime * 2f);
                _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _leftTopPos_rot.Value, Time.fixedDeltaTime * 2f);
                _right.Value = false;
            }

            if (PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _leftTopPos_pos.Value, Time.fixedDeltaTime * 2f);
                _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _leftTopPos_rot.Value, Time.fixedDeltaTime * 2f);
                _right.Value = false;
            }

            if (!PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _rightTopPos_pos.Value, Time.fixedDeltaTime * 2f);
                _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _rightTopPos_rot.Value, Time.fixedDeltaTime * 2f);
                _right.Value = true;
            }
        }

        public void ChangeCameraToFront() {
            if (!this.inGrindCamera && Main.settings.GetCameraModActive()) {

                if (PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
                    _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _rightTopPos_pos.Value, Time.fixedDeltaTime * 2f);
                    _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _rightTopPos_rot.Value, Time.fixedDeltaTime * 2f);
                    _right.Value = true;
                }
                if (!PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Goofy) {
                    _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _leftTopPos_pos.Value, Time.fixedDeltaTime * 2f);
                    _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _leftTopPos_rot.Value, Time.fixedDeltaTime * 2f);
                    _right.Value = false;
                }
                if (PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                    _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _leftTopPos_pos.Value, Time.fixedDeltaTime * 2f);
                    _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _leftTopPos_rot.Value, Time.fixedDeltaTime * 2f);
                    _right.Value = false;
                }
                if (!PlayerController.Instance.IsSwitch && SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                    _actualCam_pos.Value = Vector3.Lerp(_actualCam_pos.Value, _rightTopPos_pos.Value, Time.fixedDeltaTime * 2f);
                    _actualCam_rot.Value = Quaternion.Slerp(_actualCam_rot.Value, _rightTopPos_rot.Value, Time.fixedDeltaTime * 2f);
                    _right.Value = true;
                }
            }
        }
    }
}
