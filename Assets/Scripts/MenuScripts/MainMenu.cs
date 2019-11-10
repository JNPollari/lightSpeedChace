using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class MainMenu : MonoBehaviour
    {
        MainController mc;

        // Start is called before the first frame update
        void Start()
        {
            mc = GetComponentInParent<MainController>();
            mc.SetMainMenu(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void NewGame()
        {
            Cursor.visible = false;
            gameObject.SetActive(false);
            Time.timeScale = 1;
            mc.Launch(false);
        }

        public void TutorialGame()
        {
            Cursor.visible = false;
            gameObject.SetActive(false);
            Time.timeScale = 1;
            mc.Launch(true);
        }

        public bool IsActive()
        {
            if (gameObject.activeInHierarchy) return true;
            else return false;
        }

        public void QuitGame()
        {
            Application.Quit();
        }


    }

}