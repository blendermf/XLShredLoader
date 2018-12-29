using UnityEngine;
using Harmony12;
using System;

namespace XLShredLoader.Extensions {

    using Components;
    public static class PlayerControllerExtensions {
        public static void TurnLeftModified(this PlayerController ob, float p_value, InputController.TurningMode p_turningMode) {

            switch (p_turningMode) {
                case InputController.TurningMode.Grounded:
                    ob.boardController.AddTurnTorque(-p_value);
                    ob.skaterController.AddTurnTorque(-p_value * ob.torsoTorqueMult);
                    return;
                case InputController.TurningMode.PreWind:
                    ob.boardController.AddTurnTorque(-(p_value / 5f));
                    return;
                case InputController.TurningMode.InAir:
                    if (Main.settings.spinVelocityEnabled) {
                        ob.skaterController.AddTurnTorque(-p_value + PlayerControllerData.Instance.getSpinVelocity());
                        PlayerControllerData.Instance.addSpinVelocity("left");
                    } else {
                        ob.skaterController.AddTurnTorque(-p_value);
                    }
                    return;
                case InputController.TurningMode.FastLeft:
                    if (SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                        ob.skaterController.AddTurnTorque(-p_value, true);
                        return;
                    }
                    ob.skaterController.AddTurnTorque(-p_value);
                    return;
                case InputController.TurningMode.FastRight:
                    if (SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                        ob.skaterController.AddTurnTorque(-p_value);
                        return;
                    }
                    ob.skaterController.AddTurnTorque(-p_value, true);
                    return;
                case InputController.TurningMode.Manual:
                    ob.boardController.AddTurnTorqueManuals(-p_value);
                    return;
                default:
                    return;
            }
        }

        public static void TurnRightModified(this PlayerController ob, float p_value, InputController.TurningMode p_turningMode) {
            switch (p_turningMode) {
                case InputController.TurningMode.Grounded:
                    ob.boardController.AddTurnTorque(p_value);
                    ob.skaterController.AddTurnTorque(p_value * ob.torsoTorqueMult);
                    return;
                case InputController.TurningMode.PreWind:
                    ob.boardController.AddTurnTorque(p_value / 5f);
                    return;
                case InputController.TurningMode.InAir:
                    if (Main.settings.spinVelocityEnabled) {
                        ob.skaterController.AddTurnTorque(p_value + PlayerControllerData.Instance.getSpinVelocity());
                        PlayerControllerData.Instance.addSpinVelocity("right");
                        return;
                    }
                    ob.skaterController.AddTurnTorque(p_value);
                    return;
                case InputController.TurningMode.FastLeft:
                    if (SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                        ob.skaterController.AddTurnTorque(p_value);
                        return;
                    }
                    ob.skaterController.AddTurnTorque(p_value, true);
                    return;
                case InputController.TurningMode.FastRight:
                    if (SettingsManager.Instance.stance == SettingsManager.Stance.Regular) {
                        ob.skaterController.AddTurnTorque(p_value, true);
                        return;
                    }
                    ob.skaterController.AddTurnTorque(p_value);
                    return;
                case InputController.TurningMode.Manual:
                    ob.boardController.AddTurnTorqueManuals(p_value);
                    return;
                default:
                    return;
            }
        }
    }
}
