using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class Cbar : MonoBehaviour
    {
        private float maxCargo = 5;
        private Transform trf;


        // Start is called before the first frame update
        void Start()
        {
            trf = GetComponent<Transform>();
            GetComponentInParent<CargoBarController>().SetCbar(this);
        }

        internal void SetCargo(int c)
        {
            Vector3 v = new Vector3(1, c / maxCargo, 1);
            trf.localScale = Vector3.Lerp(transform.localScale, v, 10 * Time.deltaTime);
        }
    }

}