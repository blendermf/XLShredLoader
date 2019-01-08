using System;
using Harmony12;
using RootMotion.Dynamics;
using RootMotion.FinalIK;
using UnityEngine;

namespace XLShredRespawnNearBail.Extensions.Components {
    public class RespawnData : MonoBehaviour {

        public Vector3[] _setTmpPos = new Vector3[8];

        public Quaternion[] _setTmpRot = new Quaternion[8];

        public float lastTmpSave;

        public Coroutine DoBailTmpCoroutine = null;

        private Traverse cObj;
        private Traverse<bool> _canPressField;
        private Traverse<BehaviourPuppet> _behaviourPuppetField;
        private Traverse<bool> _backwardsField;
        private Traverse<FullBodyBipedIK> _finalIkField;
        private Traverse<Vector3> _playerOffsetField;
        private Traverse<string> _idleAnimationField;
        private Traverse<bool> _retryRespawnField;
        private Traverse SetSpawnPosMethod;

        private Respawn _respawnComponent;

        public Respawn RespawnComponent {
            get { return _respawnComponent; }
            set {
                _respawnComponent = value;
                cObj = Traverse.Create(_respawnComponent);
                _canPressField = cObj.Field<bool>("_canPress");
                _behaviourPuppetField = cObj.Field<BehaviourPuppet>("_behaviourPuppet");
                _backwardsField = cObj.Field<bool>("_backwards");
                _finalIkField = cObj.Field<FullBodyBipedIK>("_finalIk");
                _playerOffsetField = cObj.Field<Vector3>("_playerOffset");
                _idleAnimationField = cObj.Field<string>("_idleAnimation");
                _retryRespawnField = cObj.Field<bool>("_retryRespawnField");
                SetSpawnPosMethod = cObj.Method("SetSpawnPos");
            }
        }

        private bool CanPress {
            get { return _canPressField.Value; }
            set { _canPressField.Value = value; }
        }

        private BehaviourPuppet BehaviourPuppetComponent {
            get { return _behaviourPuppetField.Value; }
            set { _behaviourPuppetField.Value = value; }
        }

        private bool Backwards {
            get { return _backwardsField.Value; }
            set { _backwardsField.Value = value; }
        }

        private FullBodyBipedIK FinalIk {
            get { return _finalIkField.Value; }
            set { _finalIkField.Value = value; }
        }

        private Vector3 PlayerOffset {
            get { return _playerOffsetField.Value; }
            set { _playerOffsetField.Value = value; }
        }

        private string IdleAnimation {
            get { return _idleAnimationField.Value; }
            set { _idleAnimationField.Value = value; }
        }

        private bool RetryRespawn {
            get { return _retryRespawnField.Value; }
            set { _retryRespawnField.Value = value; }
        }

        public void DoTmpRespawn() {
            if (CanPress && !RespawnComponent.respawning) {
                PlayerController.Instance.IsRespawning = true;
                RespawnComponent.respawning = true;
                CanPress = false;
                GetTmpSpawnPos();
                PlayerController.Instance.CancelInvoke("DoBail");
                ((MonoBehaviour) _respawnComponent).CancelInvoke("DelayPress");
                ((MonoBehaviour) _respawnComponent).CancelInvoke("EndRespawning");
                ((MonoBehaviour) _respawnComponent).Invoke("DelayPress", 0.4f);
                ((MonoBehaviour) _respawnComponent).Invoke("EndRespawning", 0.25f);
            }
        }

        public void SetSpawnPos() {
            SetSpawnPosMethod.GetValue();
        }

        public void GetTmpSpawnPos() {
            ((MonoBehaviour) _respawnComponent).CancelInvoke("DoRespawn");
            PlayerController.Instance.CancelRespawnInvoke();
            RespawnComponent.puppetMaster.FixTargetToSampledState(1f);
            RespawnComponent.puppetMaster.FixMusclePositions();
            BehaviourPuppetComponent.StopAllCoroutines();
            FinalIk.enabled = false;
            for (int i = 0; i < RespawnComponent.getSpawn.Length; i++) {
                RespawnComponent.getSpawn[i].position = _setTmpPos[i];
                RespawnComponent.getSpawn[i].rotation = _setTmpRot[i];
            }
            RespawnComponent.bail.bailed = false;
            PlayerController.Instance.playerSM.OnRespawnSM();
            PlayerController.Instance.ResetIKOffsets();
            PlayerController.Instance.cameraController._leanForward = false;
            PlayerController.Instance.cameraController._pivot.rotation = PlayerController.Instance.cameraController._pivotCentered.rotation;
            PlayerController.Instance.comController.COMRigidbody.velocity = Vector3.zero;
            PlayerController.Instance.boardController.boardRigidbody.velocity = Vector3.zero;
            PlayerController.Instance.boardController.boardRigidbody.angularVelocity = Vector3.zero;
            PlayerController.Instance.boardController.frontTruckRigidbody.velocity = Vector3.zero;
            PlayerController.Instance.boardController.frontTruckRigidbody.angularVelocity = Vector3.zero;
            PlayerController.Instance.boardController.backTruckRigidbody.velocity = Vector3.zero;
            PlayerController.Instance.boardController.backTruckRigidbody.angularVelocity = Vector3.zero;
            PlayerController.Instance.skaterController.skaterRigidbody.velocity = Vector3.zero;
            PlayerController.Instance.skaterController.skaterRigidbody.angularVelocity = Vector3.zero;
            PlayerController.Instance.skaterController.skaterRigidbody.useGravity = false;
            PlayerController.Instance.boardController.IsBoardBackwards = Backwards;
            PlayerController.Instance.SetBoardToMaster();
            PlayerController.Instance.SetTurningMode(InputController.TurningMode.Grounded);
            PlayerController.Instance.ResetAllAnimations();
            PlayerController.Instance.animationController.ForceAnimation("Riding");
            PlayerController.Instance.boardController.firstVel = 0f;
            PlayerController.Instance.boardController.secondVel = 0f;
            PlayerController.Instance.boardController.thirdVel = 0f;
            PlayerController.Instance.skaterController.ResetRotationLerps();
            PlayerController.Instance.SetLeftIKLerpTarget(0f);
            PlayerController.Instance.SetRightIKLerpTarget(0f);
            PlayerController.Instance.SetMaxSteeze(0f);
            PlayerController.Instance.AnimSetPush(false);
            PlayerController.Instance.CrossFadeAnimation("Riding", 0.05f);
            PlayerController.Instance.cameraController.ResetAllCamera();
            RespawnComponent.puppetMaster.targetRoot.position = _setTmpPos[1] + PlayerOffset;
            RespawnComponent.puppetMaster.targetRoot.rotation = _setTmpRot[0];
            RespawnComponent.puppetMaster.angularLimits = false;
            RespawnComponent.puppetMaster.Resurrect();
            RespawnComponent.puppetMaster.state = PuppetMaster.State.Alive;
            RespawnComponent.puppetMaster.targetAnimator.Play(IdleAnimation, 0, 0f);
            BehaviourPuppetComponent.SetState(BehaviourPuppet.State.Puppet);
            RespawnComponent.puppetMaster.Teleport(_setTmpPos[1] + PlayerOffset, _setTmpRot[0], true);
            PlayerController.Instance.SetIKOnOff(1f);
            PlayerController.Instance.skaterController.skaterRigidbody.useGravity = false;
            PlayerController.Instance.skaterController.skaterRigidbody.constraints = RigidbodyConstraints.None;
            FinalIk.enabled = true;
            RetryRespawn = false;
        }
        
        public void SetTmpSpawnPos() {
            Quaternion quaternion = Quaternion.LookRotation(RespawnComponent.getSpawn[0].rotation * Vector3.forward, Vector3.up);
            Backwards = PlayerController.Instance.GetBoardBackwards();
            for (int i = 0; i < _setTmpPos.Length; i++) {
                if ((float)i == 0f) {
                    _setTmpPos[i] = RespawnComponent.getSpawn[1].position + PlayerOffset;
                    _setTmpRot[i] = quaternion;
                } else if (i == 5) {
                    _setTmpPos[i] = RespawnComponent.getSpawn[i].position;
                    _setTmpRot[i] = quaternion;
                } else if (i == 7) {
                    _setTmpPos[i] = RespawnComponent.getSpawn[1].position + PlayerOffset;
                    _setTmpRot[i] = quaternion;
                } else {
                    _setTmpPos[i] = RespawnComponent.getSpawn[i].position;
                    _setTmpRot[i] = RespawnComponent.getSpawn[i].rotation;
                }
            }
        }
    }
}
