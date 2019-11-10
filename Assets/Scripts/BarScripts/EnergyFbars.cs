using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class EnergyFbars : MonoBehaviour
    {
        public GameObject hBar;
        private GameObject[] barArray;

        private Transform trf;
        private float drawHeight = 15;

        // Start is called before the first frame update
        void Start()
        {
            trf = GetComponent<Transform>();
            GetComponentInParent<EnergyBarController>().SetFrontBars(this);
        }

        /// <summary>
        /// Draws horizontal bars on top of energy meter, splitting it to equal parts
        /// </summary>
        /// <param name="h">amount of parts the bar is separated to</param>
        public void DrawHBars(int h)
        {
            // Destroy previous bars
            if (barArray != null)
            {
                for (int i = 0; i < barArray.Length; i++)
                {
                    Destroy(barArray[i]);
                }
            }

            // Initialize new bar array and calculate distance between horizontal bars
            barArray = new GameObject[h];
            float distance = drawHeight / h;

            // Instantiate new bars and save them to barArray
            for (int i = 0; i < h; i++)
            {
                barArray[i] = Instantiate(hBar, new Vector2(trf.position.x, trf.position.y + (distance * i)), Quaternion.identity);
            }
        }
    }

}