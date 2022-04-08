using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Visuals
{
    public class Water : MonoBehaviour
    {
        Material Mat;
        Vector2 MainOffset;
        public Vector2 MainWaveSpeed;
        Vector2 SecondaryOffset;
        public Vector2 SecondaryWaveSpeed;

        // Start is called before the first frame update
        void Start()
        {
            Mat = GetComponent<MeshRenderer>().sharedMaterial;

        }

        // Update is called once per frame
        void Update()
        {
            MainOffset += MainWaveSpeed * Time.deltaTime;
            SecondaryOffset += SecondaryWaveSpeed * Time.deltaTime;
            Mat.SetTextureOffset("_MainTex", MainOffset);
            Mat.SetTextureOffset("_DetailAlbedoMap", SecondaryOffset);
        }
    }
}