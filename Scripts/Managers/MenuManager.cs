using System;
using TBLITO.IO;
using UnityEngine.UI;
using TBLITO.Interface;
using System.Collections.Generic;
using TBLITO.Visuals;
using TBLITO.Audio;
using UnityEngine;
using TMPro;

namespace TBLITO.Managers
{
    public class MenuManager : Manager<MenuManager>
    {
        [Header("Menus")]
        public MenuCamera menuCamera;
        public Transform[] MenuPoints;
        Transform CurrentMenuPoint;

        [Header("Level Select")]
        public GameObject[] LevelButtons;
        public GameObject EndlessButton;

        [Header("Records")]
        public RectTransform RecordsContainer;
        public GameObject RecordText;
        public TMP_Text EndlessModeHIText;

        [Header("Shop")]
        public GameObject ShopMenu;
        public TMP_Text MoneyText;
        public TMP_Text PriceText;
        public PrevNextMenu SkinSelector;
        public GameObject BuyButton;
        public GameObject SelectButton;

        [Header("Settings")]
        public Slider MusicSlider;
        public Slider SFXSlider;
        public TMP_Dropdown QualDropdown;
        public TMP_Dropdown ResDropdown;
        Resolution[] Resolutions;
        int CurResIndex;
        public Toggle FSToggle;
        public Toggle MLToggle;
        [System.Serializable]
        public class Settings : SaveFile
        {
            public float MusicVol;
            public float SFXVol;
            public int QualLevel;
            public int ResIndex;
            public bool FS;
            public bool ML;
        }
        public Settings settings;

        // Start is called before the first frame update
        void Awake()
        {
            Init(this);

            AudioManager.Instance.SetMusicTrack("Lost");
            AudioManager.Instance.InteractWithMusic(SoundEffectBehaviour.Play);

            for (int LB = 0; LB < LevelButtons.Length; LB++)
            {
                LevelButtons[LB].SetActive(LB < ProgressManager.Instance.progress.LevelIndex);
            }
            EndlessButton.SetActive(ProgressManager.Instance.progress.AllLevelsBeat);

            float RecTextHeight = RecordText.GetComponent<RectTransform>().rect.height;
            float RecContainerHeight = 0f;
            for (int R = 0; R < ProgressManager.Instance.progress.BestTimes.Count; R++)
            {
                Vector2 Pos = new Vector2(0, -29.24f - (60f * R));
                GameObject TXT = Instantiate(RecordText, Vector3.zero, Quaternion.identity, RecordsContainer);
                TXT.SetActive(true);
                TXT.transform.localRotation = Quaternion.identity;
                TXT.GetComponent<RectTransform>().localPosition = Pos;
                TXT.GetComponent<TMP_Text>().text = ("Level " + (R + 1).ToString() + "                                         " + ProgressManager.Instance.progress.BestTimes[R].ToString("000"));
                RecContainerHeight = Pos.y - 29.24f;
            }
            RecordsContainer.sizeDelta = new Vector2(RecordsContainer.sizeDelta.x, -RecContainerHeight);
            RecordsContainer.anchoredPosition = new Vector2(RecordsContainer.anchoredPosition.x, -(RecContainerHeight / 2));

            SkinSelector.Value = ProgressManager.Instance.progress.SelectedSkin;
            SkinSelector.MinValue = 0;
            SkinSelector.MaxValue = SkinManager.Instance.Skins.Length - 1;

            InitRes();
            Settings LoadedSettings = Saver.Load(settings) as Settings;
            if(LoadedSettings != null) { settings = LoadedSettings; }
            else { ResetSettings(); }
            FeedSettings();
            ApplySettings();
            UpdateScreen();
        }

        // Update is called once per frame
        void Update()
        {
            EndlessModeHIText.enabled = ProgressManager.Instance.progress.AllLevelsBeat;
            EndlessModeHIText.text = "Endless Mode High Score:" + ProgressManager.Instance.progress.EndlessModeHI.ToString("000");

            SkinManager.Instance.OverrideSelectedSkin = ShopMenu.activeSelf;
            if(ShopMenu.activeSelf) { SkinManager.Instance.DisplaySkinIndex = SkinSelector.Value; }
            MoneyText.text = "Money:" + ProgressManager.Instance.progress.Money.ToString("000");
            PriceText.text = "Price:" + SkinManager.Instance.Skins[SkinSelector.Value].Cost.ToString("000");
            BuyButton.SetActive(!ProgressManager.Instance.progress.SkinsUnlocked[SkinSelector.Value]);
            SelectButton.SetActive(ProgressManager.Instance.progress.SkinsUnlocked[SkinSelector.Value] && SkinSelector.Value != SkinManager.Instance.SelectedSkinIndex);

            settings.MusicVol = MusicSlider.value;
            settings.SFXVol = SFXSlider.value;
            settings.QualLevel = QualDropdown.value;
            settings.ResIndex = ResDropdown.value;
            settings.FS = FSToggle.isOn;
            settings.ML = MLToggle.isOn;
            ApplySettings();
            settings.Save();
        }

        public void SetMenuPoint(string MenuPointName)
        {
            Transform menuPoint = Array.Find(MenuPoints, Transform => Transform.gameObject.name == MenuPointName);
            if(menuPoint == null)
            {
                Debug.LogError("Menu Point " + MenuPointName + " Is Unavialble!");
                return;
            }

            menuCamera.Target = menuPoint;

            PlaySelectSound();
        }
        public void Back()
        {
            SetMenuPoint("Main");
        }

        public void LoadLevel(int LevelNum)
        {
            PlaySelectSound();
            AudioManager.Instance.MuteMusic();
            ProgressManager.Instance.CurLevel = LevelNum;
            FadeManager.Instance.FadeTo("Level " + LevelNum.ToString("00"));
        }
        public void EndlessMode()
        {
            PlaySelectSound();
            AudioManager.Instance.MuteMusic();
            FadeManager.Instance.FadeTo("Endless Mode");
        }

        public void Buy()
        {
            if(ProgressManager.Instance.progress.Money >= SkinManager.Instance.Skins[SkinSelector.Value].Cost)
            {
                ProgressManager.Instance.progress.SkinsUnlocked[SkinSelector.Value] = true;
                ProgressManager.Instance.progress.Money -= SkinManager.Instance.Skins[SkinSelector.Value].Cost;
                AudioManager.Instance.InteractWithSFX("Buy", SoundEffectBehaviour.Play);
            }
            else
            {
                AudioManager.Instance.InteractWithSFX("Cant Buy", SoundEffectBehaviour.Play);
            }
        }
        public void Select()
        {
            ProgressManager.Instance.progress.SelectedSkin = SkinSelector.Value;
            SkinManager.Instance.SelectedSkinIndex = SkinSelector.Value;
            PlaySelectSound();
        }

        void InitRes()
        {
            Resolutions = Screen.resolutions;
            List<string> Res2String = new List<string>();
            Resolution CurScreenRes = Screen.currentResolution;
            int curResIndex = 0;
            for (int R = 0; R < Resolutions.Length; R++)
            {
                Resolution Res = Resolutions[R];
                string String = Res.width + "x" + Res.height;
                Res2String.Add(String);

                if(Res.width == CurScreenRes.width && Res.height == CurScreenRes.height)
                {
                    curResIndex = R;
                }
            }
            ResDropdown.ClearOptions();
            ResDropdown.AddOptions(Res2String);
            CurResIndex = curResIndex;
        }
        void FeedSettings()
        {
            MusicSlider.value = settings.MusicVol;
            SFXSlider.value = settings.SFXVol;
            QualDropdown.value = settings.QualLevel;
            ResDropdown.value = settings.ResIndex;
            FSToggle.isOn = settings.FS;
            MLToggle.isOn = settings.ML;
        }
        void ResetSettings()
        {
            settings.MusicVol = AudioManager.Instance.GetMusicVolume();
            settings.SFXVol = AudioManager.Instance.GetSFXVolume();
            settings.QualLevel = QualitySettings.GetQualityLevel();
            settings.ResIndex = CurResIndex;
            settings.FS = Screen.fullScreen;
            settings.ML = MouseManager.Instance.HideMouseWhilePlaying;
        }
        void ApplySettings()
        {
            AudioManager.Instance.SetMusicVolume(MusicSlider.value);
            AudioManager.Instance.SetSFXVolume(SFXSlider.value);
            QualitySettings.SetQualityLevel(QualDropdown.value);
            MouseManager.Instance.HideMouseWhilePlaying = MLToggle.isOn;
        }
        public void UpdateScreen()
        {
            Resolution Res = Resolutions[ResDropdown.value];
            Screen.SetResolution(Res.width, Res.height, FSToggle.isOn);
        }
        public void ResetProgress()
        {
            ProgressManager.Instance.ResetProgress();
            QuitGame();
        }

        public void ReplayEnding()
        {
            FadeManager.Instance.FadeTo("Ending");
        }

        public void QuitGame()
        {
            PlaySelectSound();
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
            #endif
        }
    }
}