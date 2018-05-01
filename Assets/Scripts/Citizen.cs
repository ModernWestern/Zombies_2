using UnityEngine;
using NPC.Enemy;

namespace NPC
{
    namespace Ally
    {
        [RequireComponent(typeof(SphereCollider))]

        public class Citizen : CharacterBehaviour
        {
            GameObject[] goZombies;
            
            #region Init

            public void Init(GameObject CitizenBody, string name, int age)
            {
                CitizenBody.transform.localScale = new Vector3(1f, 1f, 1f); // Resize Object
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

            public static implicit operator Zombie(Citizen citizen) // Become Zombie ***
            {
                zombiefied = true; // Yeah, You're a Zombie Already
                Zombie zombie = citizen.gameObject.AddComponent<Zombie>();
                zombie.zombieProperties.age = citizen.citizenProperties.age; // Keep Citizen Age After Be Bitten
                zombie.zombieProperties.name = citizen.citizenProperties.name; // Keep Citizen Name After Be Bitten
                Destroy(citizen); // Ain't Citizen Anymore
                return zombie;
            }

            #endregion

            #region Main Methods

            public override void Start()
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); // Set Object Color

                // Collision Message
                gameObject.tag = "Citizen";
                SphereCollider sc = gameObject.GetComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 1f;
                // End Collison Message
                
                base.Start(); // Start Start() from CharacterBehaviour.cs
            }

            void Update()
            {
                // Gizmos
                goZombies = FindObjectsOfType(typeof(GameObject)) as GameObject[];
                float distMin = 1000; // Minimum Value Gonna Be This Until Something Become Smaller
                int index = 0;

                for (int i = 0; i < goZombies.Length; i++)
                {
                    if (goZombies[i].GetComponent<Zombie>()) // Find Zombies
                    { 
                        if (distMin > Vector3.Distance(transform.position, goZombies[i].transform.position))
                        {
                            distMin = Vector3.Distance(transform.position, goZombies[i].transform.position);
                            index = i;
                        }
                    }
                }

                DisplayDrawLine(goZombies[index], Color.yellow); // Gizmos
                // End Gizmos
            }
            #endregion
        }
    }
}
