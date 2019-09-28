using UnityEngine;
using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameManagement;
using XLShredLib;
using System.Reflection;

namespace XLShredMenuMod.Patches {
    [HarmonyPatch(typeof(GameStateMachine), "Update")]
    static class GameStateMachine_Update_Patch {
  
        static bool Prefix(GameStateMachine __instance) {
            if (Main.enabled) {
                //__instance.CurrentState.OnUpdate();
                return true;
            } else {
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(GameState), "CanDoTransitionTo")]
    static class PauseState_CanDoTransitionTo_Patch {
        private static string NullableToString(object obj) {
            return obj == null ? "(null)" : obj.ToString();
        }
        static bool Prefix(GameState __instance, Type targetState, ref bool __result, Type[] ___availableTransitions) {
            if (Main.enabled) {
                Console.WriteLine($"Instance: {NullableToString(__instance)}");
                
                switch (__instance.GetType().Name) {
                    case "PauseState":
                        Console.WriteLine($"MODIFIED Pause CAN DO TRANSITION TO Instance: {NullableToString(__instance)} targetState: {NullableToString(targetState)} __result: {NullableToString(__result)} ___availableTransitions: {NullableToString(___availableTransitions)}");
                        __result = ___availableTransitions.Contains(targetState) || PauseStateModInfo.Instance.CanDoTransitionToExtra(targetState);
                        break;
                    case "PlayState":
                        Console.WriteLine($"MODIFIED Play CAN DO TRANSITION TO Instance: {NullableToString(__instance)} targetState: {NullableToString(targetState)} __result: {NullableToString(__result)} ___availableTransitions: {NullableToString(___availableTransitions)}");
                        __result = ___availableTransitions.Contains(targetState) || PlayStateModInfo.Instance.CanDoTransitionToExtra(targetState);
                        break;
                    case "GearSelectionState":
                        Console.WriteLine($"MODIFIED GearSelection CAN DO TRANSITION TO Instance: {NullableToString(__instance)} targetState: {NullableToString(targetState)} __result: {NullableToString(__result)} ___availableTransitions: {NullableToString(___availableTransitions)}");
                        __result = ___availableTransitions.Contains(targetState) || GearSelectionStateModInfo.Instance.CanDoTransitionToExtra(targetState);
                        break;
                    case "LevelSelectionState":
                        Console.WriteLine($"MODIFIED LevelSelection CAN DO TRANSITION TO Instance: {NullableToString(__instance)} targetState: {NullableToString(targetState)} __result: {NullableToString(__result)} ___availableTransitions: {NullableToString(___availableTransitions)}");
                        __result = ___availableTransitions.Contains(targetState) || LevelSelectionStateModInfo.Instance.CanDoTransitionToExtra(targetState);
                        break;
                    case "PinMovementState":
                        Console.WriteLine($"MODIFIED PinMovement CAN DO TRANSITION TO Instance: {NullableToString(__instance)} targetState: {NullableToString(targetState)} __result: {NullableToString(__result)} ___availableTransitions: {NullableToString(___availableTransitions)}");
                        __result = ___availableTransitions.Contains(targetState) || PinMovementStateModInfo.Instance.CanDoTransitionToExtra(targetState);
                        break;
                    case "ReplayState":
                        Console.WriteLine($"MODIFIED Replay CAN DO TRANSITION TO Instance: {NullableToString(__instance)} targetState: {NullableToString(targetState)} __result: {NullableToString(__result)} ___availableTransitions: {NullableToString(___availableTransitions)}");
                        __result = ___availableTransitions.Contains(targetState) || ReplayStateModInfo.Instance.CanDoTransitionToExtra(targetState);
                        break;
                    case "TutorialState":
                        Console.WriteLine($"MODIFIED Tutorial CAN DO TRANSITION TO Instance: {NullableToString(__instance)} targetState: {NullableToString(targetState)} __result: {NullableToString(__result)} ___availableTransitions: {NullableToString(___availableTransitions)}");
                        __result = ___availableTransitions.Contains(targetState) || TutorialStateModInfo.Instance.CanDoTransitionToExtra(targetState);
                        break;
                    default:
                        __result = true;
                        break;
                }
                

                return false;
            } else {
                return true;
            }
        }
    }
}
