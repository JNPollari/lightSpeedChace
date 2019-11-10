using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class TutorialScript : MonoBehaviour
    {
        private MainController mc;

        // Start is called before the first frame update
        void Start()
        {
            mc = GetComponentInParent<MainController>();
            mc.SetTutor(gameObject);
            gameObject.SetActive(false);
        }
    }

}