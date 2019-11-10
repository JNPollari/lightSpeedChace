using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lightspeed
{

    public class PauseMenu : MonoBehaviour
    {
        MainController mc;

        // Start is called before the first frame update
        void Start()
        {
            mc = GetComponentInParent<MainController>();
            mc.SetPauseMenu(this);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Hides the menu and continues the game
        /// </summary>
        public void ContinueGame()
        {
            Cursor.visible = false;
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("LSC", LoadSceneMode.Single);
            Time.timeScale = 1;
        }

        public void InitiatePausemenu()
        {
            gameObject.SetActive(true);
        }
    }

}