using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class Ebar : MonoBehaviour
    {
        private float maxEnergy = 100;
        private Transform trf;


        // Start is called before the first frame update
        void Start()
        {
            trf = GetComponent<Transform>();
            GetComponentInParent<EnergyBarController>().SeteBar(this);
        }

        public void setEnergy(float e)
        {
            Vector3 v = new Vector3(1, e / maxEnergy, 1);
            trf.localScale = Vector3.Lerp(transform.localScale, v, 5 * Time.deltaTime);
        }

        public void setMaxEnergy(float e)
        {
            maxEnergy = e;
        }
    }

}