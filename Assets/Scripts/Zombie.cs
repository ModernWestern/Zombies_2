using UnityEngine;
using System.Collections;
using NPC.Ally;

namespace NPC
{
    namespace Enemy
    {
        public class Zombie : MonoBehaviour
        {
            public ZombieProperties zombieProperties;
            float partialTime;
            int pickBehave;
            int whichWay;
            GameObject onSightCitizen;

            #region Init

            public void Init(GameObject zombieBody, Color color) // Zombie Generator
            {
                // Collision Message
                zombieBody.name = ZombieName(color);
                zombieBody.tag = "Zombie";
                zombieBody.AddComponent<SphereCollider>();
                SphereCollider sc = zombieBody.GetComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 5f; // Zombie Sight Range (5 units)
                // End Collision Message

                zombieBody.AddComponent<Rigidbody>();
                Rigidbody rb = zombieBody.GetComponent<Rigidbody>();
                rb.interpolation = RigidbodyInterpolation.Extrapolate;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.constraints = RigidbodyConstraints.FreezeRotation;

                zombieBody.GetComponent<Renderer>().material.SetColor("_Color", color); // Set Object Color
            }
            #endregion

            #region Methods

            string ZombieName(Color color)
            {
                string name = " ";
                if (color == Color.cyan) name = "Runner";
                if (color == Color.magenta) name = "Fatty";
                if (color == Color.green) name = "Snorer";
                return name;
            }

            float ZombieSpeed()
            {
                float speed = 0;
                Color zColor; // Store Color From Object
                zColor = gameObject.GetComponent<Renderer>().material.color; // Get Color
                if (zColor == Color.cyan) speed = 7.5f;
                if (zColor == Color.magenta) speed = 5f;
                if (zColor == Color.green) speed = 2.5f;
                return speed;
            }

            IEnumerator RefreshState() // Every 3 seconds call PickState
            {
                yield return new WaitForSeconds(partialTime);
                PartialTime(out partialTime);
                PickState();
            }

            void PickState() // Move or Idle
            {
                zombieProperties.behaviour = (Behaviour)(Random.Range(0, 3)); // Random State

                if (zombieProperties.behaviour == Behaviour.getMove)
                {
                    whichWay = Random.Range(0, 2);
                    StartCoroutine("RefreshState");
                }
                else if (zombieProperties.behaviour == Behaviour.getIdle)
                {
                    whichWay = 2; // Idle Case
                    StartCoroutine("RefreshState");
                }
                else if (zombieProperties.behaviour == Behaviour.getRotate)
                {
                    whichWay = 3; // Rotate Case
                    StartCoroutine("RefreshState");
                }
            }

            void MoveIt() // Move Zombie Randomly
            {
                if (transform.position.x >= -45 && transform.position.x <= 45 && transform.position.z >= -45 && transform.position.z <= 45) // Boundaries
                {
                    switch (whichWay)
                    {
                        case 0:
                            transform.Translate(Vector3.forward * (Time.deltaTime * ZombieSpeed()), Space.Self);
                            break;
                        case 1:
                            transform.Translate(Vector3.back * (Time.deltaTime * ZombieSpeed()), Space.Self);
                            break;
                        case 2:
                            transform.Translate(Vector3.zero);
                            break;
                        case 3:
                            transform.Rotate(Vector3.up);
                            break;
                        case 4:
                            transform.Translate(Vector3.up * (.075f * Mathf.Sin(1)));
                            break;
                        default:
                            print("Nothing");
                            break;
                    }
                }
                else if (transform.position.x < -45) // If Get Stuck
                {
                    Vector3 bounce = transform.position;
                    bounce.x = -35;
                    transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
                    whichWay = 4;
                }
                else if (transform.position.x > 45) // If Get Stuck
                {
                    Vector3 bounce = transform.position;
                    bounce.x = 35;
                    transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
                    whichWay = 4;
                }
                else if (transform.position.z < -45) // If Get Stuck
                {
                    Vector3 bounce = transform.position;
                    bounce.z = -35;
                    transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
                    whichWay = 4;
                }
                else if (transform.position.z > 45) // If Get Stuck
                {
                    Vector3 bounce = transform.position;
                    bounce.z = 35;
                    transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
                    whichWay = 4;
                }
            }

            void PartialTime(out float t)
            {
                t = Random.Range(3.0f, 3.6f); // Faux Delay
            }
            #endregion

            #region Main Methods

            void Start()
            {
                zombieProperties.bodyPart = " "; // "null", No Taste Yet
                PartialTime(out partialTime);
                PickState();
                StartCoroutine("RefreshState");
            }

            void FixedUpdate()
            {
               MoveIt();
                print(partialTime);
            }
            #endregion
        }
    }
}