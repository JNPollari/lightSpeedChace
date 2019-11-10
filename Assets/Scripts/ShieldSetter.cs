using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class ShieldSetter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ParticleSystem s = gameObject.GetComponent<ParticleSystem>();
            gameObject.GetComponentInParent<ShipController>().SetShieldEffect(s);
        }
    }

}