using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lightspeed
{

    public class EnergyBarController : MonoBehaviour
    {
        private EnergyFbars feBar;
        private Ebar eBar;
        private Hbar hpBar;
        private Transform trf;
        private Vector2 shiftGoal;
        private int shiftspeed = 50;
        private bool shifting = false;
        private float regenSpeed = 1;
        private bool initiated = false;
        private Text textField;

        internal void SetTextField(Text text)
        {
            textField = text;
        }

        public bool chaser = false;

        // Start is called before the first frame update
        void Start()
        {
            trf = gameObject.GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (shifting) ShiftBar();
            textField.text = regenSpeed.ToString(".00");
        }

        private void ShiftBar()
        {
            trf.position = Vector2.MoveTowards(trf.position, shiftGoal, shiftspeed * Time.deltaTime);

            if (trf.position.x.Equals(shiftGoal.x))
            {
                shifting = false;
                if (initiated) chaser = !chaser;
                else initiated = true;
            }
        }

        public void SetFrontBars(EnergyFbars bars)
        {
            feBar = bars;
        }

        public void SeteBar(Ebar bar)
        {
            eBar = bar;
        }

        public void SethpBar(Hbar bar)
        {
            hpBar = bar;
        }

        public void SetEnergy(float e, float eRegen, int max, int hp)
        {
            regenSpeed = eRegen;
            eBar.setEnergy(e);
            eBar.setMaxEnergy(max);
            feBar.DrawHBars(max / 100);
            hpBar.SetHP(hp);

        }

        public void Switch()
        {
            if (initiated == false) initiated = true;
            if (chaser)
            {
                shiftGoal = new Vector2(-90, -25);
                shifting = true;
            }
        }

        public void Bring()
        {
            shiftGoal = new Vector2(-60, -25);
            shifting = true;
        }
    }

}