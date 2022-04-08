using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Visuals
{
    public class MenuCamera : MonoBehaviour
    {
        public Transform Target;
        public float SmoothTime;

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 Pos = Vector3.Lerp(transform.position, Target.position, SmoothTime * Time.deltaTime);
            Quaternion Rot = Quaternion.Lerp(transform.rotation, Target.rotation, SmoothTime * Time.deltaTime);
            transform.position = Pos;
            transform.rotation = Rot;
        }
    }
}