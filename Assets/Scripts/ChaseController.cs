using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class ChaseController : MonoBehaviour
    {
        public GameObject red;
        public GameObject yellow;
        private MainController mc;

        private GameObject redShip;
        private GameObject yellowShip;
        private ShipCommunicator redCommunicator;
        private ShipCommunicator yellowCommunicator;

        // Start is called before the first frame update
        void Start()
        {
            mc = GetComponentInParent<MainController>();
            mc.SetChaseController(this);

        }

        // Track for key inputs for each ship
        void Update()
        {
            if (Input.GetKeyDown("w") || Input.GetKeyDown("s") || Input.GetKeyDown("a") || Input.GetKeyDown("d"))
            {
                if (redCommunicator != null) redCommunicator.KeyInput(true);
            }

            if (Input.GetKeyUp("w") || Input.GetKeyUp("s") || Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            {
                if (redCommunicator != null) redCommunicator.KeyInput(false);
            }

            if (Input.GetKeyDown("up") || Input.GetKeyDown("down") || Input.GetKeyDown("left") || Input.GetKeyDown("right"))
            {
                if (yellowCommunicator != null) yellowCommunicator.KeyInput(true);
            }

            if (Input.GetKeyUp("up") || Input.GetKeyUp("down") || Input.GetKeyUp("left") || Input.GetKeyUp("right"))
            {
                if (yellowCommunicator != null) yellowCommunicator.KeyInput(false);
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Cursor.visible = true;
                MainController mc = GetComponentInParent<MainController>();
                if (mc != null) mc.PauseGame();
            }
        }

        public void LaunchGame()
        {
            redShip = Instantiate(red, new Vector2(0, 0), Quaternion.identity);
            yellowShip = Instantiate(yellow, new Vector2(0, 0), Quaternion.identity);

            bool startChasing = (UnityEngine.Random.value > 0.5f);

            redCommunicator = redShip.GetComponent<ShipCommunicator>();
            yellowCommunicator = yellowShip.GetComponent<ShipCommunicator>();

            redCommunicator.InitiateShip(startChasing);
            yellowCommunicator.InitiateShip(!startChasing);

            redCommunicator.chase = this;
            yellowCommunicator.chase = this;
        }

        // Initiate side switch for each ship
        public void SwitchSides()
        {
            mc.Flash();
            redCommunicator.SwitchAll();
            yellowCommunicator.SwitchAll();
        }

        /// <summary>
        /// Calls for shipcommunicators to bring cargobars to view, from where the command is relayed to chased ship
        /// </summary>
        public void BringCargo()
        {
            redCommunicator.BringCargo();
            yellowCommunicator.BringCargo();
        }
    }
}