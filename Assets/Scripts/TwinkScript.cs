using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class TwinkScript : MonoBehaviour
    {
        private Rigidbody2D rig;
        private Vector2 vel;
        private BoxCollider2D col;

        private int speed = 60;
        private bool twinkReady = false;

        // Start is called before the first frame update
        void Start()
        {
            col = GetComponent<BoxCollider2D>();
            rig = GetComponent<Rigidbody2D>();
            vel = new Vector2(speed, 0);
            rig.velocity = vel;

            Destroy(gameObject, 5);
            StartCoroutine(TwinkDelay());
        }

        /// <summary>
        /// Set delay to twink activation
        /// </summary>
        /// <returns>WaitForSeconds(0.5f);</returns>
        IEnumerator TwinkDelay()
        {
            yield return new WaitForSeconds(0.5f);
            twinkReady = true;
        }

        /// <summary>
        /// Destroy collider and twink upon collision
        /// </summary>
        /// <param name="col">colliding object</param>
        void OnTriggerEnter2D(Collider2D col)
        {
            // check if target is Destructible or Player
            if ((col.CompareTag("Destructible") || col.CompareTag("Player")) && twinkReady)
            {
                ShipController sc = col.transform.GetComponent<ShipController>();
                if (sc != null) sc.Destroy();

                CapsuleScript cc = col.transform.GetComponent<CapsuleScript>();
                if (cc != null) cc.Destroy();

                Destroy(gameObject);
            }
        }
    }

}