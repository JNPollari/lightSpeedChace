using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lightspeed
{

    public class ShipController : MonoBehaviour
    {

        //Ship audio
        [SerializeField] private AudioClip shootAClip;
        [SerializeField] private AudioClip jumpAClip;
        [SerializeField] private AudioClip jumpReturnAClip;
        [SerializeField] private AudioClip destroyAClip;
        [SerializeField] private AudioClip shieldAClip;
        [SerializeField] private AudioSource audioSource;

        // Ship components
        private Rigidbody2D rig;
        private Transform trf;
        [SerializeField]
        private GameObject twink;
        internal ShipCommunicator comm;
        private ParticleSystem pSystem;
        private ShipEngine engine;
        private ParticleSystem shieldEffect;

        // Ship movement variables
        private Vector2 direction;
        private Vector2 startMomentum;
        private Vector2 accelForce;
        private Vector2 brakeForce;
        private Vector2 jumpgoal;

        private float speed;
        private float height;
        
        [SerializeField, Range(0, 200)] private float topSpeed = 40;
        [SerializeField, Range(0, 50)] private float edgeSpeed = 10;
        [SerializeField, Range(0, 50)] private float startSpeed = 10;
        [SerializeField, Range(0, 100)] private float accelSpeed = 20;
        [SerializeField, Range(0, 300)] private float brakeSpeed = 80;
        [SerializeField, Range(0, 100)] private int routeEdge = 10;
        [SerializeField, Range(0, 300)] private int routeEnd = 30;

        private int jumpspeed = 400;
        private bool jumping = false;
        private bool backJumped = false;
        private bool turning = false;
        private bool turnAfter = false;


        // Ship action variables
        private int hp = 5;
        private int maxHP = 5;
        private int jumpLimit = 1;
        private bool shot = false;
        private float shotRate = 0.2f;
        private Coroutine switchroutine = null;
        private Coroutine shootroutine = null;
        private Coroutine turnroutine = null;
        internal bool chaser = false;
        private bool destroyed = false;

        // Ship ablility variables
        private int maxEnergy = 100;
        private float energy = 100;
        private readonly int maxCargo = 5;
        private int cargo = 0;
        private int deliveredCargo = 0;
        private int cargoToCollect = 3;
        private float energyRegen = 1;

        [SerializeField] private AnimationCurve turnCurve;

        /// <summary>
        /// Initiate ShipController
        /// </summary>
        void Start()
        {
            direction = new Vector2(0, 1);
            accelForce = new Vector2(0, accelSpeed);
            brakeForce = new Vector2(0, brakeSpeed);
            startMomentum = new Vector2(0, startSpeed);

            pSystem = GetComponent<ParticleSystem>();
            audioSource = GetComponent<AudioSource>();
            rig = GetComponent<Rigidbody2D>();
            trf = GetComponent<Transform>();
            rig.velocity = startMomentum;
            startMomentum = new Vector2(0, topSpeed); // Change starting momentum to topspees for later use

            InvokeRepeating("RegenEnergy", 0.1f, 0.1f);
        }

        /// <summary>
        /// Set given gameObject to this ship's engine
        /// </summary>
        /// <param name="gameObject">ship's engine</param>
        internal void SetEngine(ShipEngine eng)
        {
            engine = eng;
        }

        /// <summary>
        /// Set given particlesystem as shield effect
        /// </summary>
        /// <param name="s"></param>
        internal void SetShieldEffect(ParticleSystem s)
        {
            shieldEffect = s;
        }


        /// <summary>
        /// Handle constantly running tasks
        /// </summary>
        void Update()
        {
            // Calculate variables
            height = System.Math.Abs(trf.position.y);
            speed = System.Math.Abs(rig.velocity.y);
            if (speed != 0) direction.Set(0, rig.velocity.y / speed);
            if (speed > topSpeed) rig.velocity = startMomentum * direction; // If ship would go too fast, slow it down

            // Slow down towards the edge
            if (height > routeEdge && speed > edgeSpeed && direction.y * trf.position.y > 0)
            {
                float depthFactor = (height - routeEdge) / (routeEnd - routeEdge);
                rig.AddForce(brakeForce * (direction * -1 * depthFactor));
            }

            // Accelerate away from the edge
            if (direction.y * trf.position.y < 0 && speed < topSpeed && height != 0)
            {
                float depthFactor = (routeEnd / (1 + Math.Abs(height)));
                rig.AddForce(accelForce * direction * depthFactor);
            }

            // Turning at the edge
            if (height > routeEnd && (direction.y * trf.position.y) > 0 && !turning)
            {
                turnroutine = StartCoroutine(Turn());
            }

            // Jump
            if (jumping) Jump();

            // Send updatecommand to energy bar via communicator
            comm.UpdateResources(energy, energyRegen, maxEnergy, cargo, cargoToCollect - deliveredCargo, jumpLimit, hp);

        }

        /////////////////////////////
        // SHIP CONTROLLER METHODS //
        /////////////////////////////


        private void RegenEnergy()
        {
            if (energy < maxEnergy) energy = energy + energyRegen * 4;
        }

        // Shoot
        private void Shoot()
        {
            if (energy >= 100 && !destroyed)
            {
                energy = energy - 100;
                Instantiate(twink, new Vector2(trf.position.x + 2, trf.position.y), Quaternion.identity);
                GetComponent<AudioSource>().PlayOneShot(shootAClip, 0.3f);
            }
        }

        /// <summary>
        /// Empties collected capsules and upgrades ship accordingly
        /// </summary>
        private void HandleCargo()
        {
            energyRegen = energyRegen + (cargo * 0.05f);
            if (!chaser && cargo == jumpLimit && jumpLimit < 4) jumpLimit++;
            if (!chaser && cargo > jumpLimit && jumpLimit > 1) jumpLimit--;

            deliveredCargo = deliveredCargo + cargo;
            while (deliveredCargo >= cargoToCollect)
            {
                deliveredCargo = deliveredCargo - cargoToCollect;
                maxEnergy = maxEnergy + 100;
                cargoToCollect++;
            }
            cargo = 0;
        }

        /// <summary>
        /// Handles moving ship to it's destined position when switch is initiated
        /// </summary>
        private void Jump()
        {
            // Keep coroutines freezed during jumps
            if (switchroutine != null) StopCoroutine(switchroutine);
            if (shootroutine != null) StopCoroutine(shootroutine);

            // Move towards jumpgoal with jumpspeed
            trf.position = Vector2.MoveTowards(trf.position, jumpgoal, jumpspeed * Time.deltaTime);

            // When chased has flied away, handle cargo, teleport it behind and set jumpgoal to chaser position
            if (trf.position.x.Equals(jumpgoal.x) && !backJumped && !chaser)
            {
                jumpgoal = new Vector2(-100, 0);
                HandleCargo();

                trf.position = jumpgoal;
                backJumped = true;
                jumpgoal = new Vector2(-45, 0);
            }

            // When chased reaches chaser's position, quit jumping and change it to chaser
            if (trf.position.x.Equals(jumpgoal.x) && backJumped && !chaser)
            {
                jumping = false;
                backJumped = false;
                chaser = true;
                rig.velocity = startMomentum;
                comm.BringEnergy();
                GetComponent<AudioSource>().PlayOneShot(jumpReturnAClip);
                return;
            }

            // When chaser reaches chased's position, quit jumping and change it to chased
            if (trf.position.x.Equals(jumpgoal.x) && !backJumped && chaser)
            {
                jumping = false;
                chaser = false;
                turnroutine = StartCoroutine(Turn());
            }
        }

        /// <summary>
        /// Start firing twinks every shotRate after 0,2 seconds
        /// </summary>
        /// <returns>WaitForSeconds(shotRate)</returns>
        IEnumerator Fire()
        {
            shot = false;
            yield return new WaitForSeconds(0.2f);
            shot = true;
            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(shotRate);
            }

        }

        /// <summary>
        /// Commence side switch after 2 seconds
        /// </summary>
        /// <returns>WaitForSeconds(2)</returns>
        IEnumerator SideSwitch()
        {
            yield return new WaitForSeconds(1);
            if (cargo >= jumpLimit && !destroyed)
            {
                comm.SwitchSides();
                GetComponent<AudioSource>().PlayOneShot(jumpAClip);
            }
        }

        /// <summary>
        /// Turns the ship around using animation curve. Must likely be restricted to single call per runtime, possibly line another call.
        /// </summary>
        /// <returns></returns>
        IEnumerator Turn()
        {
            turning = true;
            Vector2 turninvelocity = new Vector2();
            turninvelocity = rig.velocity;
            float time = 0;
            while (true)
            {
                yield return new WaitForEndOfFrame();
                time = time + Time.deltaTime;
                rig.velocity = turninvelocity * turnCurve.Evaluate(time);
                if (time >= 0.1f) // Could time be checked from the curve?
                {
                    turning = false;
                    if (turnAfter)
                    {
                        turnAfter = false;
                        turnroutine = StartCoroutine(Turn());
                    }
                    break;
                }
            }
        }

        ////////////////////
        // PUBLIC METHODS //
        ////////////////////

        /// <summary>
        /// Handles ship spesific key inputs
        /// </summary>
        /// <param name="pressed">True if key was pressed, false if released</param>
        public void KeyInput(bool pressed)
        {
            // Initiate shooting coroutine
            if (chaser && pressed)
            {
                if (shootroutine != null) StopCoroutine(shootroutine); // Cornfirm that no shootcoroutine is left on
                shootroutine = StartCoroutine(Fire());
            }

            // Initiate side switch coroutine
            if (!chaser && pressed) switchroutine = StartCoroutine(SideSwitch());

            // End coroutines and turn if hasn't shot
            if (!pressed)
            {
                if (switchroutine != null) StopCoroutine(switchroutine);
                if (shootroutine != null) StopCoroutine(shootroutine);
                if (!shot || !chaser)
                {
                    if (!turning) turnroutine = StartCoroutine(Turn());
                    else turnAfter = !turnAfter;
                }
            }
        }



        /// <summary>
        /// Start switching ships' roles
        /// </summary>
        public void Switch()
        {

            //for both ships, set parametres for switching positions and start jumping
            if (chaser)
            {
                jumpgoal = new Vector2(25, 0);
                jumpspeed = 50;
                jumping = true;
            }
            else
            {
                jumpgoal = new Vector2(1000, 0);
                jumpspeed = 400;
                jumping = true;
            }
        }

        /// <summary>
        /// Upon contact with capsule, the capsule calls this method to register the collection event
        /// </summary>
        public void CollectCapsule()
        {
            if (cargo < maxCargo) cargo++;
            if (chaser)
            {
                if (hp < maxHP) hp++;
                HandleCargo();
            }
        }

        /// <summary>
        /// Attempt to destroy the ship, reducing it's hp and initiating ship's destruction when hp reaches zero 
        /// </summary>
        public void Destroy()
        {
            if (!jumping)
            {
                hp--;
                if (hp > 0)
                {
                    shieldEffect.Play();
                    GetComponent<AudioSource>().PlayOneShot(shieldAClip);
                }
            }

            if (!jumping && hp <= 0)
            {
                if (!destroyed)
                {
                    Destroy(shieldEffect);
                    GetComponent<AudioSource>().PlayOneShot(destroyAClip);
                    pSystem.Play();
                    if (engine != null) engine.Destroy();
                    Destroy(GetComponent<SpriteRenderer>());
                    Destroy(GetComponent<PolygonCollider2D>());
                    StartCoroutine(Reset());
                }
                destroyed = true;
            }
        }

        /// <summary>
        /// Reset the game with 3 second delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(3);
            Cursor.visible = true;
            SceneManager.LoadScene("LSC", LoadSceneMode.Single);
        }
    }

}
