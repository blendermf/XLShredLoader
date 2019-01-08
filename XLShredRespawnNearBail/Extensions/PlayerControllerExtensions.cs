using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

namespace XLShredRespawnNearBail.Extensions {
    public static class PlayerControllerExtensions {
        public static IEnumerator DoBailTmp(this PlayerController ob) {
            yield return new WaitForSeconds(2.5f);
            PlayerController.Instance.respawn.GetExtensionComponent().DoTmpRespawn();
        }
    }
}
