using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBLITO.Visuals
{
    public class MenuCameraPoint : MonoBehaviour
    {
        MenuCamera MenuCam;
        public GameObject AssociatedMenu;

        void Start()
        {
            MenuCam = FindObjectOfType<MenuCamera>();
        }

        // Update is called once per frame
        void Update()
        {
            if(AssociatedMenu != null)
            {
                AssociatedMenu.SetActive(MenuCam.Target == transform);
            }
        }
    }
}