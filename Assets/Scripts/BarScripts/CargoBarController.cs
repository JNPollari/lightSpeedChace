using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lightspeed
{

    public class CargoBarController : MonoBehaviour
    {
        private CargoFbars fcBar;
        private Cbar cBar;
        private Hbar hpBar;
        private Transform trf;
        Camera cam;
        private Vector2 shiftGoal;
        private int shiftspeed = 50;
        private bool shifting = false;
        private bool initiated = false;

        private int cToCollect = 3;
        public bool chaser = false;
        private Text textField;

        internal void SetTextField(Text text)
        {
            textField = text;
        }

        // Start is called before the first frame update
        void Start()
        {
            trf = gameObject.GetComponent<Transform>();
            cam = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (shifting) ShiftBar();
            textField.text = cToCollect.ToString();
        }

        public void SetFrontBars(CargoFbars bars)
        {
            fcBar = bars;
        }

        public void SethpBar(Hbar bar)
        {
            hpBar = bar;
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

        public void SetCbar(Cbar bar)
        {
            cBar = bar;
        }

        public void SetCargo(int c, int cR, int jumpLimit, int hp)
        {
            cBar.SetCargo(c);
            cToCollect = cR;
            fcBar.DrawHBars(jumpLimit);
            hpBar.SetHP(hp);
        }

        public void Switch()
        {
            if (initiated == false) initiated = true;
            if (!chaser)
            {
                shiftGoal = new Vector2(90, -25);
                shifting = true;
            }
        }

        public void Bring()
        {
            shiftGoal = new Vector2(60, -25);
            shifting = true;
        }
    }
}