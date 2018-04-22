using UnityEngine;
using NPC.Enemy;

namespace NPC
{
    namespace Ally
    {
        public class Citizen : MonoBehaviour
        {
            public CitizenProperties citizenProperties;
            GameObject[] goZombies;
            GameObject atStake;
            
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
                float speed = 0f;
                if (age >= 70) speed = 2.5f;
                else if (age >= 30 && age < 70) speed = 5f;
                else if (age >= 15 && age < 30) speed = 7.5f;
                return speed;
            }
            #endregion

            #region Main Methods

            void Start()
            {
                goZombies = FindObjectsOfType(typeof(GameObject)) as GameObject[];

                foreach (GameObject go in goZombies)
                {
                    Component zComp = go.GetComponent(typeof(Zombie)); // Any Object With This Component Gonna Be Chased
                    if (zComp != null) atStake = go;
                }

                gameObject.AddComponent<CharacterBehaviour>();
                gameObject.GetComponent<CharacterBehaviour>().Init(gameObject, CitizenSpeed(citizenProperties.age)); // Movement
                gameObject.GetComponent<CharacterBehaviour>().DisplayDrawLine(atStake, Color.yellow, "Long"); // Gismoz
            }
            #endregion
        }
    }
}
