using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class MainController : MonoBehaviour
    {
        private MainMenu mMenu;
        private PauseMenu pMenu;
        private ChaseController cController;
        private SpawnController spwnController;
        private StarSpawner starSpawn;
        private GameObject tutor;

        internal void SetTutor(GameObject tutorialScript)
        {
            tutor = tutorialScript;
        }

        private Coroutine dimmingroutine = null;
        private Coroutine flashroutine = null;

        private SpriteRenderer srd;
        private float alpha = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            srd = gameObject.GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Initiates Flas coroutine
        /// </summary>
        public void Flash()
        {
            flashroutine = StartCoroutine(Flashroutine());
        }

        /// <summary>
        /// Flashes the screen bright for a moment, during which spawncontroller is reseted and star are made to shift
        /// </summary>
        /// <returns></returns>
        IEnumerator Flashroutine()
        {
            alpha = 0f;
            bool brighten = true;
            bool dimmen = true;
            while (brighten || dimmen)
            {
                yield return new WaitForSeconds(0.01f);
                srd.color = new Color(0.5f, 0.5f, 0.5f, alpha);
                if (brighten) alpha = alpha + 0.06f;
                else alpha = alpha - 0.06f;
                if (alpha >= 1)
                {
                    brighten = false;
                    spwnController.Reset();
                }
                if (alpha <= 0)
                {
                    srd.color = new Color(0.5f, 0.5f, 0.5f, alpha);
                    dimmen = false;
                }


            }
        }

        IEnumerator DimmingRoutine()
        {
            alpha = 0.5f;
            bool visible = true;
            while (visible)
            {
                yield return new WaitForSeconds(0.05f);
                srd.color = new Color(0f, 0f, 0f, alpha);
                alpha = alpha - 0.05f;
                if (alpha <= 0)
                {
                    visible = false;
                    cController.LaunchGame();
                }
            }
        }

        public void SetMainMenu(MainMenu m)
        {
            mMenu = m;
        }

        public void SetPauseMenu(PauseMenu m)
        {
            pMenu = m;
        }

        public void SetChaseController(ChaseController cc)
        {
            cController = cc;
        }

        public void SetSpawnController(SpawnController sc)
        {
            spwnController = sc;
        }

        public void SetStarSpawner(StarSpawner ss)
        {
            starSpawn = ss;
        }

        internal void Launch(bool tutorial)
        {
            if (tutorial) tutor.SetActive(true);
            dimmingroutine = StartCoroutine(DimmingRoutine());
            spwnController.Reset();
        }

        public void PauseGame()
        {
            if (!mMenu.IsActive())
            {
                Time.timeScale = 0;
                pMenu.InitiatePausemenu();
            }
        }
    }
}