using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace TBLITO.Managers
{
    public class MouseManager : Manager<MouseManager>
    {
        [Space]
        public bool HideMouseWhilePlaying;
        bool LockMouse;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
        }

        // Update is called once per frame
        void Update()
        {
            bool InLevel = IsInLevel();
            LockMouse = (InLevel && HideMouseWhilePlaying);

            Cursor.lockState = (LockMouse) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !LockMouse;
        }

        bool IsInLevel()
        {
            bool Result = false;
            GameManager gameManager = FindObjectOfType<GameManager>();
            bool GameManagerIsNull = gameManager == null;
            if(GameManagerIsNull) { Result = false; }
            else
            {
                Result = (!gameManager.IsPaused);
            }
            return Result;
        }
    }
}