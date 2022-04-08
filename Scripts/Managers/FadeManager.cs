using System.Collections;
using TBLITO.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace TBLITO.Managers
{
    public class FadeManager : Manager<FadeManager>
    {
        [Space]
        public Animator FadePanel;
        [HideInInspector]
        public bool IsFaded;

        void Awake()
        {
            Init(this);
        }

        void Update()
        {
            FadePanel.SetBool("IsFaded", IsFaded);
        }

        public void FadeTo(string SceneName)
        {
            StartCoroutine(fadeTo(SceneName));
        }
        IEnumerator fadeTo(string scene)
        {
            IsFaded = true;
            AudioManager.Instance.InteractWithSFX("Fade In", SoundEffectBehaviour.Play);

            yield return new WaitForSeconds(0.75f);

            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            while(!operation.isDone)
            {
                yield return null;
            }

            IsFaded = false;
            AudioManager.Instance.InteractWithSFX("Fade Out", SoundEffectBehaviour.Play);
        }
    }
}