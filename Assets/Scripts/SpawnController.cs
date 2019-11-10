using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class SpawnController : MonoBehaviour
    {
        public GameObject capsule;
        public GameObject particulate;

        private MainController mc;
        private float capsuleRate = 5;
        private float particulateRate = 10;
        private float startingparticulateRate = 10;

        private Coroutine capsuleroutine = null;
        private Coroutine particulateroutine = null;
        private Coroutine delayroutine = null;
        private bool run = true;

        private GameObject[] capsules;
        private int nextCapsule = 0;
        private GameObject incomingPariculate;

        // Start is called before the first frame update
        void Start()
        {
            mc = GetComponentInParent<MainController>();
            mc.SetSpawnController(this);

            capsules = new GameObject[5];
            //delayroutine = StartCoroutine(Startdelay());
        }

        IEnumerator Startdelay()
        {
            yield return new WaitForSeconds(5);
            run = true;
            capsuleroutine = StartCoroutine(CapsuleRoutine());
            particulateroutine = StartCoroutine(ParticulateRoutine());
        }

        IEnumerator CapsuleRoutine()
        {
            while (run)
            {
                yield return new WaitForSeconds(capsuleRate);
                capsules[nextCapsule] = Instantiate(capsule, new Vector2(80, UnityEngine.Random.Range(-25, 25)), Quaternion.identity);
                nextCapsule++;
                if (nextCapsule > 4) nextCapsule = 0;
            }
        }

        IEnumerator ParticulateRoutine()
        {
            while (run)
            {
                yield return new WaitForSeconds(particulateRate);
                incomingPariculate = Instantiate(particulate, new Vector2(0, UnityEngine.Random.Range(-30, 30)), Quaternion.identity);
                if (particulateRate > 1) particulateRate = 0.8f * particulateRate;
            }
        }

        internal void Reset()
        {
            run = false;
            StopAllCoroutines();
            Destroy(incomingPariculate);
            for (int i = 0; i < capsules.Length; i++)
            {
                if (capsules[i] != null) Destroy(capsules[i]);
            }
            particulateRate = startingparticulateRate;

            delayroutine = StartCoroutine(Startdelay());
        }
    }

}