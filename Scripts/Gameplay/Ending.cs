using System.Collections;
using TBLITO.Managers;
using TBLITO.Audio;
using UnityEngine;


namespace TBLITO.Gameplay
{
    public class Ending : MonoBehaviour
    {
        bool EndingTriggered;
        public Transform HeliLookAtPoint;
        public GameObject gameManager;
        public Player player;
        public Animator HeliAnim;
        public Animator EndUI;

        void Start()
        {
            AudioManager.Instance.MuteMusic();
        }

        void Update()
        {
            if(EndingTriggered)
            {
                HeliLookAtPoint.LookAt(transform);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && !EndingTriggered)
            {
                StartCoroutine(ending());
            }
        }
        IEnumerator ending()
        {
            EndingTriggered = true;
            AudioManager.Instance.SetMusicTrack("Rescue");
            ProgressManager.Instance.progress.AllLevelsBeat = true;
            player.Body.constraints = RigidbodyConstraints.FreezeAll;
            player.transform.SetParent(transform);
            HeliAnim.SetTrigger("HeliFlyOff");
            yield return new WaitForSeconds(15f);
            Destroy(gameManager);
            EndUI.SetTrigger("Appear");
        }

        public void Menu()
        {
            FadeManager.Instance.FadeTo("Menu");
        }
    }
}