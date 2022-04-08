using System.Collections;
using TBLITO.Audio;
using UnityEngine;

namespace TBLITO.Gameplay
{
    public class TimedSpikes : MonoBehaviour
    {
        public bool Attacking;
        public float AttackDelay;
        float AttakTimer;
        public Animator animator;
        public Obstacle obstacle;

        void Start()
        {
            AttakTimer = AttackDelay;
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetBool("IsAttacked", Attacking);
            obstacle.Enabled = Attacking;

            AttakTimer -= Time.deltaTime;
            if(AttakTimer <= 0f)
            {
                Attacking = !Attacking;
                AttakTimer = AttackDelay;
                AudioManager.Instance.InteractWithSFX("Attack", SoundEffectBehaviour.Play);
            }
        }
    }
}