using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;

namespace Assets.Scripts.Misc
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        float z = -10;//Used to maintain the camera's z level, sometimes important for rendering
        public float x_offset = 0, y_offset = 0;

        void Start()
        {
            if (!target)
            {
                enabled = false;
                DebugLogger.LogWarning(DebugData.DebugType.Graphics, "No target for camera");
            }
        }

        void Update()
        {
            z = transform.position.z;
            Vector3 v = target.position;
            v.z = z;
            v.x += x_offset;
            v.y += y_offset;
            transform.position = v;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}