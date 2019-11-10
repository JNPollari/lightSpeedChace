using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class StarSpawner : MonoBehaviour
    {
        public GameObject star;

        private Transform trf;

        // Start is called before the first frame update
        void Start()
        {
            GetComponentInParent<MainController>().SetStarSpawner(this);
            trf = GetComponent<Transform>();
            InvokeRepeating("Starfall", 0.0005f, 0.0005f);
        }

        /// <summary>
        /// Coninuously spawn stars to fly acorss the screen
        /// </summary>
        private void Starfall()
        {
            int limit = Random.Range(0, 99);
            int depth = Random.Range(limit, 100);
            Instantiate(star, new Vector3(100 + depth * 10, Random.Range(-60 - depth * 5, 60 + depth * 5), depth), Quaternion.identity);
        }
    }

}