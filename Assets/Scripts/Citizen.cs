using UnityEngine;
using System.Collections;

namespace NPC
{
    namespace Ally
    {
        public class Citizen : MonoBehaviour
        {
            public CitizenProperties citizenProperties;
            float partialTime;
            int pickBehave;
            int whichWay;

            #region Init

            public void Init(GameObject CitizenBody, string name, int age)
            {
                // Collision Message
                CitizenBody.tag = "Citizen";
                CitizenBody.AddComponent<SphereCollider>();
                SphereCollider sc = CitizenBody.GetComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 1f;
                // End Collison Message

                CitizenBody.AddComponent<Rigidbody>();
                Rigidbody rb = CitizenBody.GetComponent<Rigidbody>();
                rb.interpolation = RigidbodyInterpolation.Extrapolate;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.constraints = RigidbodyConstraints.FreezeRotation;

                CitizenBody.transform.localScale = new Vector3(1f, 1f, 1f); // Resize Object
                CitizenBody.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); // Set Object Color
                CitizenBody.name = name + " " + age; // Set Object Name

                citizenProperties.age = age;
                citizenProperties.name = name;
                citizenProperties.info = Message(name, age);
            }
            #endregion

            #region Methods

            string Message(string _name, int _age) // Greetings Generator
            {
                int index = Random.Range(0, (int)Greeting.Lenght);
                citizenProperties.greeting = (Greeting)index;
                string message = citizenProperties.greeting.ToString() + "! I'm " + _name + "and I'm " + _age + "yo"; // CANVAS
                return message;
            }

            float CitizenSpeed(int age) // Use citizen age property to set speed
            {
                float speed = 0;
                if (age >= 70) speed = 2.5f;
                else if (age >= 30 && age < 70) speed = 5f;
                else if (age >= 15 && age < 30) speed = 7.5f;
                return speed;
            }

            IEnumerator RefreshState() // Every 3 seconds call PickState
            {
                yield return new WaitForSeconds(partialTime); 
                PartialTime(out partialTime); // Make PartialTime() a New Value Each Call
                PickState();
            }

            void PickState() // Move or Idle
            {
                citizenProperties.behaviour = (Behaviour)(Random.Range(0, 3)); // Random State

                if (citizenProperties.behaviour == Behaviour.getMove)
                {
                    whichWay = Random.Range(0, 2);
                    StartCoroutine("RefreshState");
                }
                else if (citizenProperties.behaviour == Behaviour.getIdle)
                {
                    whichWay = 2; // Idle Case
                    StartCoroutine("RefreshState");
                }
                else if (citizenProperties.behaviour == Behaviour.getRotate)
                {
                    whichWay = 3; // Rotate Case
                    StartCoroutine("RefreshState");
                }
            }

            void MoveIt() // Move Citizen Randomly
            {
                if (transform.position.x >= -45 && transform.position.x <= 45 && transform.position.z >= -45 && transform.position.z <= 45) // Boundaries
                {
                    switch (whichWay)
                    {
                        case 0:
                            transform.Translate(Vector3.forward * (Time.deltaTime * CitizenSpeed(citizenProperties.age)), Space.Self);
                            break;
                        case 1:
                            transform.Translate(Vector3.back * (Time.deltaTime * CitizenSpeed(citizenProperties.age)), Space.Self);
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

            void PartialTime(out float t) // Faux Delay (less robotic)
            {
                t = Random.Range(3.0f, 3.6f); 
            }
            #endregion

            #region Main Methods

            void Start()
            {
                PartialTime(out partialTime); // Start With Delay (less robotic)
                PickState();
                StartCoroutine("RefreshState");
            }

            void FixedUpdate()
            {
                MoveIt();
            }
            #endregion
        }
    }
}
