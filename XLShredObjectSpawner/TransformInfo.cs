using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XLShredObjectSpawner {

    public class TransformInfo {
        public TransformInfo(Transform t) {
            this.position = t.position;
            this.rotation = t.rotation;
            this.scale = t.localScale;
        }
        
        public void ApplyTo(Transform t) {
            t.position = this.position;
            t.rotation = this.rotation;
            t.localScale = this.scale;
        }
        
        private TransformInfo(Vector3 pos, Quaternion rot, Vector3 scale) {
            this.position = pos;
            this.rotation = rot;
            this.scale = scale;
        }
        
        public static TransformInfo Lerp(TransformInfo a, TransformInfo b, float t) {
            return new TransformInfo(Vector3.Lerp(a.position, b.position, t), Quaternion.Lerp(a.rotation, b.rotation, t), Vector3.Lerp(a.position, b.position, t));
        }
        
        public Vector3 position;
        
        public Quaternion rotation;
        
        public Vector3 scale;
    }
}
