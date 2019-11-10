using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class ShipCommunicator : MonoBehaviour
    {
        public GameObject ShipPrefab;
        public GameObject EnergyPrefab;
        public GameObject CargoPrefab;

        private GameObject theShip;
        private GameObject theEnergy;
        private GameObject theCargo;
        private Coroutine barDelayRoutine = null;
        public ChaseController chase;

        private ShipController ship;
        private EnergyBarController energy;
        private CargoBarController cargo;
        private bool cargoLock = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void KeyInput(bool pressed)
        {
            if (ship != null) ship.KeyInput(pressed);
        }

        /// <summary>
        /// Initiates the ship and it's energy and cargo meters
        /// </summary>
        /// <param name="chaser">wether the initiated ship should start as chaser</param>
        public void InitiateShip(bool toBeChaser)
        {
            GameObject shipHost;
            if (toBeChaser) shipHost = Instantiate(ShipPrefab, new Vector2(-45, 50), Quaternion.identity);
            else shipHost = Instantiate(ShipPrefab, new Vector2(25, -50), Quaternion.identity);
            ship = shipHost.GetComponent<ShipController>();
            ship.comm = this;

            if (toBeChaser) ship.chaser = true;

            barDelayRoutine = StartCoroutine(RecourcebarDelay(toBeChaser));

        }

        IEnumerator RecourcebarDelay(bool toBeChaser)
        {
            yield return new WaitForSeconds(2);
            InitiateEnergy(toBeChaser);
            InitiateCargo(toBeChaser);
        }

        private void InitiateEnergy(bool toBeChaser)
        {
            GameObject energyHost;
            if (toBeChaser) energyHost = Instantiate(EnergyPrefab, new Vector2(-90, -25), Quaternion.identity);
            else energyHost = Instantiate(EnergyPrefab, new Vector2(-90, -25), Quaternion.identity);
            energy = energyHost.GetComponent<EnergyBarController>();

            if (toBeChaser)
            {
                energy.chaser = true;
                energy.Bring();
            }
        }

        private void InitiateCargo(bool toBeChaser)
        {
            GameObject cargoHost;
            if (toBeChaser) cargoHost = Instantiate(CargoPrefab, new Vector2(90, -25), Quaternion.identity);
            else cargoHost = Instantiate(CargoPrefab, new Vector2(90, -25), Quaternion.identity);
            cargo = cargoHost.GetComponent<CargoBarController>();

            if (toBeChaser) cargo.chaser = true;
            else cargo.Bring();
        }

        /// <summary>
        /// Command ChaseConroller to Invoke role change
        /// </summary>
        public void SwitchSides()
        {
            chase.SwitchSides();
        }

        /// <summary>
        /// Invoke position change to all components
        /// </summary>
        public void SwitchAll()
        {
            ship.Switch();
            energy.Switch();
            cargo.Switch();
        }

        internal void UpdateResources(float e, float eRegen, int eMax, int c, int cR, int jumpLimit, int hp)
        {
            if (energy != null) energy.SetEnergy(e, eRegen, eMax, hp);
            if (cargo != null) cargo.SetCargo(c, cR, jumpLimit, hp);
        }

        /// <summary>
        /// Calls CargoBarController's method, which bring cargobar to view for chased ship
        /// </summary>
        internal void BringCargo()
        {
            if (!cargoLock) cargo.Bring();
            cargoLock = false;
        }

        /// <summary>
        /// Brings energymeter to view and calls for ChaseController to call other ship's cargometer to view
        /// </summary>
        internal void BringEnergy()
        {
            energy.Bring();
            cargoLock = true;
            chase.BringCargo();
        }
    }
}