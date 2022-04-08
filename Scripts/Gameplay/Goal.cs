using System.Collections;
using TBLITO.Managers;
using UnityEngine;

namespace TBLITO.Gameplay
{
    public class Goal : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                GetComponent<Animator>().SetTrigger("GoDown");
                FXManager.Instance.SpawnFX("Win", transform.position);
                GameManager.Instance.NextLevel();
            }
        }
    }
}