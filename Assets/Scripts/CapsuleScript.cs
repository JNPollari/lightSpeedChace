using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightspeed
{

    public class CapsuleScript : MonoBehaviour
    {
        //Ship audio
        public AudioClip collectAClip;
        public AudioClip destroyAClip;
        public new AudioSource audio;
        private ParticleSystem pSystem;

        // Capsule variables
        private Rigidbody2D rig;
        private Vector2 vel;
        private BoxCollider2D col;
        private int speed = -20;
        private int rotationSpeed = 20;
        private bool destroyed = false;

        // Start is called before the first frame update
        void Start()
        {
            audio = GetComponent<AudioSource>();
            pSystem = GetComponent<ParticleSystem>();
            col = GetComponent<BoxCollider2D>();
            rig = GetComponent<Rigidbody2D>();
            vel = new Vector2(speed, 0);
            rotationSpeed = Random.Range(-40, 40);

            rig.velocity = vel;
            rig.angularVelocity = rotationSpeed;
            Destroy(gameObject, 20);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player") && !destroyed)
            {
                audio.PlayOneShot(collectAClip);
                col.transform.GetComponent<ShipController>().CollectCapsule();
                Destroy(GetComponent<SpriteRenderer>());
                Destroy(GetComponent<PolygonCollider2D>());
                Destroy(gameObject, 2);
                destroyed = true;
            }
        }

        public void Destroy()
        {
            if (!destroyed)
            {
                audio.PlayOneShot(destroyAClip);
                pSystem.Play();
                Destroy(GetComponent<SpriteRenderer>());
                Destroy(GetComponent<PolygonCollider2D>());
                Destroy(gameObject, 2);
            }
            destroyed = true;
        }
    }

}