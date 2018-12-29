using UnityEngine;
using Harmony12;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using XLShredLoader.Extensions;
using XLShredLoader.Extensions.Components;


namespace XLShredLoader.Patches {
    [HarmonyPatch(typeof(PlayerState_Manualling), "Enter")]
    static class PlayerState_Manualling_Enter_Patch {

        static void Prefix(ref float ____popForce) {
            ____popForce = Main.settings.customManualPopForce;
        }
    }
}
