using System;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Managers
{
    public class FXManager : Manager<FXManager>
    {
        [Space]
        public Transform EffectsContainer;
        [System.Serializable]
        public class Effect
        {
            public string Name;
            public GameObject Prefab;
        }
        public Effect[] Effects;

        void Start()
        {
            Init(this);

            if(EffectsContainer == null) { EffectsContainer = new GameObject("FX").transform; }
        }

        public void SpawnFX(string EffectName, Vector3 Position)
        {
            Effect effect = Array.Find(Effects, Effect => Effect.Name == EffectName);
            if(effect == null)
            {
                Debug.LogError("Effect " + EffectName + " Not Found!");
                return;
            }

            GameObject NewEffect = Instantiate(effect.Prefab, Position, effect.Prefab.transform.rotation, EffectsContainer);
            Destroy(NewEffect, NewEffect.GetComponent<ParticleSystem>().main.duration);
        }
    }
}