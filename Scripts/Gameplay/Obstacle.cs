using System.Collections;
using TBLITO.Managers;
using UnityEngine;

namespace TBLITO.Gameplay
{
    public class Obstacle : MonoBehaviour
    {
        public bool Enabled = true;

        void Update()
        {
            GetComponent<Collider>().enabled = Enabled;
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}