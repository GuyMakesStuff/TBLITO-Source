using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Gameplay
{
    public class MovingPlatform : MonoBehaviour
    {
        public Transform MovingObject;
        public Transform PointA;
        public Transform PointB;
        public float Speed;
        float T;

        // Start is called before the first frame update
        void Start()
        {
            T = 0;
        }

        // Update is called once per frame
        void Update()
        {
            T += Speed * Time.deltaTime;
            Vector3 Pos = Vector3.Lerp(PointA.position, PointB.position, Mathf.PingPong(T, 1f));
            MovingObject.position = Pos;
        }
    }
}