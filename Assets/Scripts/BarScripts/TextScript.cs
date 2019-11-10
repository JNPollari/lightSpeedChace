using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lightspeed
{

    public class TextScript : MonoBehaviour
    {
        EnergyBarController eroot;
        CargoBarController croot;

        // Start is called before the first frame update
        void Start()
        {
            eroot = transform.parent.parent.GetComponentInParent<EnergyBarController>();
            croot = transform.parent.parent.GetComponentInParent<CargoBarController>();
            if (eroot != null) eroot.SetTextField(gameObject.GetComponent<Text>());
            if (croot != null) croot.SetTextField(gameObject.GetComponent<Text>());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}