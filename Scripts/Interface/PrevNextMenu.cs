using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TBLITO.Audio;

namespace TBLITO.Interface
{
    public class PrevNextMenu : MonoBehaviour
    {
        public Button PrevButton;
        public Button NextButton;
        public int MinValue;
        public int MaxValue;
        [HideInInspector]
        public int Value;

        // Start is called before the first frame update
        void Start()
        {
            PrevButton.onClick.AddListener(new UnityAction(Prev));
            NextButton.onClick.AddListener(new UnityAction(Next));
        }

        // Update is called once per frame
        void Update()
        {
            Value = Mathf.Clamp(Value, MinValue, MaxValue);
            PrevButton.gameObject.SetActive((Value != MinValue));
            NextButton.gameObject.SetActive((Value != MaxValue));
        }

        public void Prev()
        {
            AudioManager.Instance.InteractWithSFX("Select", SoundEffectBehaviour.Play);
            Value--;
        }
        public void Next()
        {
            AudioManager.Instance.InteractWithSFX("Select", SoundEffectBehaviour.Play);
            Value++;
        }
    }
}