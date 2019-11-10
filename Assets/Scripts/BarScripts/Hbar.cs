using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class Hbar : MonoBehaviour
    {
        private float maxHP = 5;
        private Transform trf;


        // Start is called before the first frame update
        void Start()
        {
            trf = GetComponent<Transform>();
            EnergyBarController e = GetComponentInParent<EnergyBarController>();
            if (e != null) e.SethpBar(this);

            CargoBarController c = GetComponentInParent<CargoBarController>();
            if (c != null) c.SethpBar(this);
        }

        public void SetHP(float hp)
        {
            Vector3 v = new Vector3(1, hp / (maxHP * 2), 1);  //maxHP times 2, for the bar is only half height
            trf.localScale = Vector3.Lerp(transform.localScale, v, 5 * Time.deltaTime);
        }
    }

}