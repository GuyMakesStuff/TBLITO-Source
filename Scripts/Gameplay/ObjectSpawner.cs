using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Gameplay
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Min(0f)]
        public float SpawnDelay;
        public GameObject Object;
        float SpawnTimer;

        // Start is called before the first frame update
        void Start()
        {
            SpawnTimer = SpawnDelay;
        }

        // Update is called once per frame
        void Update()
        {
            SpawnTimer -= Time.deltaTime;
            if(SpawnTimer <= 0f)
            {
                SpawnTimer = SpawnDelay;
                Instantiate(Object, transform.position, Object.transform.rotation);
            }
        }
    }
}