using System.Collections;
using TMPro;
using TBLITO.Gameplay;
using UnityEngine.SceneManagement;
using TBLITO.Audio;
using UnityEngine;

namespace TBLITO.Managers
{
    public class GameManager : Manager<GameManager>
    {
        [Space]
        public Player player;
        public string MusicTrackName;
        public bool EndlessMode;
        int LevelIndex;

        [Header("Time")]
        public TMP_Text TimeText;
        float CurTime;
        bool CountTime;
        public TMP_Text BestTimeText;
        float BestTime;

        [Header("Money Bonus")]
        public TMP_Text MoneyBonusText;
        public int StartMoneyBonus;
        public GameObject ExtraMoneyText;
        int MoneyBonus;
        int FinalMoneyBonus;

        [Header("Endless Mode")]
        public Transform Cam;
        public TMP_Text ScoreText;
        float Score;
        public TMP_Text HIScoreText;
        float HIScore;
        bool HIScoreBeat;
        public GameObject NewHIText;

        [Header("UI")]
        public GameObject PauseMenu;
        [HideInInspector]
        public bool IsPaused;
        [HideInInspector]
        public bool CanPause;
        public GameObject RedFade;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            CanPause = true;
            CountTime = true;
            if(!EndlessMode) { LevelIndex = ProgressManager.Instance.CurLevel; }
            AudioManager.Instance.SetMusicTrack(MusicTrackName);

            if(ProgressManager.Instance.progress.BestTimes.Count >= LevelIndex && !EndlessMode) { BestTime = ProgressManager.Instance.progress.BestTimes[LevelIndex - 1]; }

            if(EndlessMode) { HIScore = ProgressManager.Instance.progress.EndlessModeHI; }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape) && CanPause)
            {
                PlaySelectSound();
                IsPaused = true;
            }
            PauseMenu.SetActive(IsPaused);
            Time.timeScale = (IsPaused) ? 0f : 1f;
            SoundEffectBehaviour SFXBehaviour = (IsPaused) ? SoundEffectBehaviour.Pause : SoundEffectBehaviour.Resume;
            AudioManager.Instance.InteractWithAllSFXOneShot(SFXBehaviour);
            AudioManager.Instance.InteractWithMusic(SFXBehaviour);

            if(!EndlessMode)
            {
                if(CountTime) { CurTime += Time.deltaTime; }
                TimeText.text = "Time:" + CurTime.ToString("00");
                BestTimeText.text = "Best Time:" + BestTime.ToString("00");

                MoneyBonus = Mathf.Clamp(StartMoneyBonus - Mathf.RoundToInt(CurTime), 0, 999999999);
                MoneyBonusText.text = "Money Bonus:" + MoneyBonus.ToString("00");
                MoneyBonusText.color = (MoneyBonus == 0) ? Color.red : Color.white;
            }
            else
            {
                Score = (Cam.position.y - 2f) / 5f;
                if(Score > HIScore)
                {
                    HIScore = Score;
                    if(!HIScoreBeat)
                    {
                        HIScoreBeat = true;
                        NewHIText.SetActive(true);
                        AudioManager.Instance.InteractWithSFX("New Best Time", SoundEffectBehaviour.Play);
                    }
                }

                ScoreText.text = "Score:" + Score.ToString("000");
                HIScoreText.text = "High Score:" + HIScore.ToString("000");

                ProgressManager.Instance.progress.EndlessModeHI = HIScore;
            }
        }

        public void GameOver()
        {
            StartCoroutine(gameOver());
        }
        IEnumerator gameOver()
        {
            CanPause = false;
            CountTime = false;
            AudioManager.Instance.InteractWithSFX("Die", SoundEffectBehaviour.Play);
            FXManager.Instance.SpawnFX("Player Die", player.transform.position);
            Destroy(player.gameObject);
            RedFade.SetActive(true);
            if(EndlessMode)
            {
                FinalMoneyBonus = Mathf.RoundToInt(Score);
                if(HIScoreBeat) { FinalMoneyBonus += 10; }
                ProgressManager.Instance.progress.Money += FinalMoneyBonus;
            }
            yield return new WaitForSeconds(1f);
            Retry();
        }
        public void Resume()
        {
            PlaySelectSound();
            IsPaused = false;
        }
        public void Retry()
        {
            Resume();
            CanPause = false;
            CountTime = false;
            FadeManager.Instance.FadeTo(SceneManager.GetActiveScene().name);
        }
        public void NextLevel()
        {
            StartCoroutine(Next());
        }
        IEnumerator Next()
        {
            CanPause = false;
            player.CanMove = false;
            CountTime = false;
            FinalMoneyBonus = MoneyBonus;
            if(CurTime < BestTime || BestTime == 0f)
            {
                BestTime = CurTime;
                FinalMoneyBonus += 5;
                ExtraMoneyText.SetActive(true);
                AudioManager.Instance.InteractWithSFX("New Best Time", SoundEffectBehaviour.Play);
            }
            AudioManager.Instance.InteractWithSFX("Win", SoundEffectBehaviour.Play);
            yield return new WaitForSeconds(1f);
            int NextLevelIndex = LevelIndex + 1;
            bool FinalLevel = NextLevelIndex > ProgressManager.Progress.LevelCount;
            Progress(NextLevelIndex, FinalLevel);
            if(FinalLevel)
            {
                FadeManager.Instance.FadeTo("Ending");
            }
            else
            {
                ProgressManager.Instance.CurLevel++;
                FadeManager.Instance.FadeTo("Level " + NextLevelIndex.ToString("00"));
            }
        }
        void Progress(int nextLevelIndex, bool IsFinalLevel)
        {
            if(nextLevelIndex > ProgressManager.Instance.progress.LevelIndex && !IsFinalLevel)
            {
                ProgressManager.Instance.progress.LevelIndex = nextLevelIndex;
            }

            if(ProgressManager.Instance.progress.BestTimes.Count < LevelIndex)
            {
                ProgressManager.Instance.progress.BestTimes.Insert(LevelIndex - 1, BestTime);
            }
            else
            {
                ProgressManager.Instance.progress.BestTimes[LevelIndex - 1] = BestTime;
            }

            ProgressManager.Instance.progress.Money += FinalMoneyBonus;
        }
        public void Menu()
        {
            Resume();
            CanPause = false;
            CountTime = false;
            FadeManager.Instance.FadeTo("Menu");
        }
    }
}