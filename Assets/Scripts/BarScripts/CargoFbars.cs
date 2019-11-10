using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class CargoFbars : MonoBehaviour
    {
        public GameObject hBar;
        private GameObject[] barArray;

        private Transform trf;
        private float drawHeight = 15;
        private int bars = 5;

        // Start is called before the first frame update
        void Start()
        {
            trf = GetComponent<Transform>();
            GetComponentInParent<CargoBarController>().SetFrontBars(this);
        }

        public void DrawHBars(int jumpLimit)
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
            barArray = new GameObject[bars];
            float distance = drawHeight / bars;

            // Instantiate new bars and save them to barArray
            for (int i = 0; i < bars; i++)
            {
                barArray[i] = Instantiate(hBar, new Vector2(trf.position.x, trf.position.y + (distance * i)), Quaternion.identity);

                if (i == jumpLimit)
                {
                    //barArray[i].GetComponent<SpriteRenderer>().color = new Color32(96, 180, 255, 255);
                    barArray[i].GetComponent<SpriteRenderer>().color = Color.red;
                    barArray[i].GetComponent<Transform>().localScale = new Vector3(5, 0.75f, 1);
                }
            }
        }
    }

}