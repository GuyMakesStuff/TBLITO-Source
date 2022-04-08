using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Gameplay
{
    public class ConstantMovingPlatform : MonoBehaviour
    {
        public Vector3 Direction;

        // Update is called once per frame
        void Update()
        {
            transform.position += Direction * Time.deltaTime;

            if(Vector3.Distance(Vector3.zero, transform.position) > 30f)
            {
                Destroy(gameObject);
            }
        }
    }
}