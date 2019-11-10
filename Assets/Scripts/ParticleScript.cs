using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class ParticleScript : MonoBehaviour
    {
        // Audio
        public AudioClip particulateAClip;
        public new AudioSource audio;

        private Rigidbody2D rig;
        private Vector2 vel;
        private BoxCollider2D col;
        private float volumeD = 0.005f;

        private readonly int speed = -1000;

        // Start is called before the first frame update
        void Start()
        {
            audio = GetComponent<AudioSource>();
            col = GetComponent<BoxCollider2D>();
            rig = GetComponent<Rigidbody2D>();
            vel = new Vector2(speed, 0);
            rig.velocity = vel;
            audio.PlayOneShot(particulateAClip);
            audio.pitch = 1.0f;
            InvokeRepeating("VolumeDown", 0.01f, 0.01f);

            Destroy(gameObject, 2);
        }

        private void VolumeDown()
        {
            //audio.volume = audio.volume - volumeD;
            volumeD = volumeD + 0.0001f;
        }



        /// <summary>
        /// Destroy all upon collision
        /// </summary>
        /// <param name="col">colliding object</param>
        void OnTriggerEnter2D(Collider2D col)
        {
            // check if target is Destructible or Player
            if ((col.CompareTag("Destructible") || col.CompareTag("Player")))
            {
                ShipController sc = col.transform.GetComponent<ShipController>();
                if (sc != null) sc.Destroy();

                CapsuleScript cc = col.transform.GetComponent<CapsuleScript>();
                if (cc != null) cc.Destroy();
            }
        }
    }
}