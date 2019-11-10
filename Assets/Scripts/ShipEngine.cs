using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{
    public class ShipEngine : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponentInParent<ShipController>().SetEngine(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
