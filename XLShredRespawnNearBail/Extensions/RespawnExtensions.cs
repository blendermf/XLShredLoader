using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace XLShredRespawnNearBail.Extensions {
    using Components;
    public static class RespawnExtensions {
        public static RespawnData GetExtensionComponent(this Respawn ob) {
            return ob.GetComponent<RespawnData>();
        }
    }
}
