using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Managers
{
    public class SkinManager : Manager<SkinManager>
    {
        [System.Serializable]
        public class Skin
        {
            public Material SkinMaterial;
            public int Cost;
        }
        public Skin[] Skins = new Skin[10];
        [HideInInspector]
        public int SelectedSkinIndex;
        [HideInInspector]
        public int DisplaySkinIndex;
        [HideInInspector]
        public bool OverrideSelectedSkin;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
        }

        // Update is called once per frame
        void Update()
        {
            if(!OverrideSelectedSkin)
            {
                DisplaySkinIndex = SelectedSkinIndex;
            }
        }
    }
}