using System.Collections;
using System.Collections.Generic;
using TBLITO.IO;
using UnityEngine;

namespace TBLITO.Managers
{
    public class ProgressManager : Manager<ProgressManager>
    {
        [System.Serializable]
        public class Progress : SaveFile
        {
            public int LevelIndex;
            [System.NonSerialized]
            public const int LevelCount = 15;
            public List<float> BestTimes;
            public int Money;
            public bool[] SkinsUnlocked;
            public int SelectedSkin;
            public bool AllLevelsBeat;
            public float EndlessModeHI;
        }
        public readonly Progress StartProgress = new Progress()
        {
            FileName = "Player Progress",
            LevelIndex = 1,
            BestTimes = new List<float>(),
            Money = 0,
            SkinsUnlocked = new bool[10] { true, false, false, false, false, false, false, false, false, false, },
            AllLevelsBeat = false,
            EndlessModeHI = 0,
        };
        public Progress progress;
        [HideInInspector]
        public int CurLevel;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            
            Progress LoadedProgress = Saver.Load(progress) as Progress;
            if(LoadedProgress != null) { progress = LoadedProgress; }
            else { progress = StartProgress; }

            FindObjectOfType<SkinManager>().SelectedSkinIndex = progress.SelectedSkin;

            FadeManager.Instance.FadeTo("Menu");
        }

        // Update is called once per frame
        void Update()
        {
            progress.Save();
        }

        public void ResetProgress()
        {
            progress = StartProgress;
            progress.Save();
        }
    }
}