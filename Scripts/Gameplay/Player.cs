using System.Collections;
using TBLITO.Managers;
using UnityEngine;

namespace TBLITO.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody Body;
        public float Speed;
        public bool PCBuild;
        [HideInInspector]
        public bool CanMove;
        float X;
        MeshRenderer meshRenderer;

        // Start is called before the first frame update
        void Start()
        {
            Body = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<MeshRenderer>();
            CanMove = true;
        }

        // Update is called once per frame
        void Update()
        {
            float PCInput = Input.GetAxisRaw("Horizontal");
            float MobileInput = GetMobileInput();
            X = ((PCBuild) ? PCInput : MobileInput) * Speed;

            meshRenderer.sharedMaterial = SkinManager.Instance.Skins[SkinManager.Instance.DisplaySkinIndex].SkinMaterial;
        }
        float GetMobileInput()
        {
            float Result = 0f;
            if(Input.touchCount > 0)
            {
                Result = Mathf.Sign(Input.touches[0].position.x - (Screen.width / 2));
            }
            else
            {
                Result = 0f;
            }

            return Result;
        }

        void FixedUpdate()
        {
            if(CanMove)
            {
                Body.velocity = new Vector3(X, Body.velocity.y, 0f);
                Body.angularVelocity = new Vector3(0f, 0f, -(X * 360));
            }
            else
            {
                Body.velocity = new Vector3(0f, Body.velocity.y, 0f);
                Body.angularVelocity = Vector3.zero;
            }
        }
    }
}