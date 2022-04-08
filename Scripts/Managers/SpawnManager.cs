using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Managers
{
    public class SpawnManager : Manager<SpawnManager>
    {
        [Space]
        public Transform CamMaxHeight;
        public Transform CamMinHeight;

        [Header("Chuncks")]
        public GameObject TrackChunckPrefab;
        public int MaxChunckCount;
        List<GameObject> Chuncks;
        float BaseChunckPos;
        float PrevChunckPos;

        [Header("Spikes")]
        public GameObject SpikesPrefab;
        public int MaxGap;
        public int MinGap;
        int Gap;
        float RoundSpawnHeight;
        float PrevRoundSpawnHeight;
        bool PrevLeftRight;
        int SpikesSpawned;
        int PrevSpikesSpawned;
        List<GameObject> Spikes;

        [Header("Difficulty")]
        public int MinMaxGap;
        public int SpikeCountPerSection;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            Chuncks = new List<GameObject>();
            Spikes = new List<GameObject>();
            BaseChunckPos = CamMaxHeight.position.y;
            Gap = Random.Range(MinGap, MaxGap + 1);
            SpawnChunck();
        }

        // Update is called once per frame
        void Update()
        {
            RoundSpawnHeight = Mathf.Round(CamMaxHeight.position.y);
            if(RoundSpawnHeight > PrevRoundSpawnHeight + Gap)
            {
                SpawnSpikes();
            }

            if(CamMaxHeight.position.y > PrevChunckPos - 2f)
            {
                SpawnChunck();
            }

            GameObject LastSpikes = Spikes[0];
            if(LastSpikes != null)
            {
                if(LastSpikes.transform.position.y < CamMinHeight.position.y)
                {
                    Spikes.Remove(LastSpikes);
                    Destroy(LastSpikes);
                }
            }
        }

        void SpawnChunck()
        {
            PrevChunckPos += 10f;
            Chuncks.Add(Instantiate(TrackChunckPrefab, new Vector3(0f, PrevChunckPos, 0f), Quaternion.identity));
            if(Chuncks.Count > MaxChunckCount)
            {
                GameObject LastChunck = Chuncks[0];
                Chuncks.Remove(LastChunck);
                Destroy(LastChunck);
            }
        }

        void SpawnSpikes()
        {
            Gap = Random.Range(MinGap, MaxGap + 1);
            PrevRoundSpawnHeight = RoundSpawnHeight;
            
            // Flase-Left, True-Right
            bool LeftRight = (Random.value >= 0.5f);
            if(LeftRight == PrevLeftRight) { LeftRight = !LeftRight; }
            PrevLeftRight = LeftRight;
            float Rot = (LeftRight) ? 90f : -90f;
            float XPos = (LeftRight) ? 0.95f : -0.95f;
            Vector3 Pos = new Vector3(XPos, PrevRoundSpawnHeight, 0f);
            Spikes.Add(Instantiate(SpikesPrefab, Pos, Quaternion.Euler(0, 0, Rot)));
            SpikesSpawned++;

            if(SpikesSpawned > PrevSpikesSpawned + SpikeCountPerSection)
            {
                PrevSpikesSpawned = SpikesSpawned;
                if(MaxGap > MinMaxGap)
                {
                    MaxGap--;
                }
            }
        }
    }
}