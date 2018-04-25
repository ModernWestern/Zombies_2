using UnityEngine;
using NPC.Enemy;

namespace NPC
{
    namespace Ally
    {
        public class Citizen : CharacterBehaviour
        {
            GameObject[] goZombies;
            
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

            public override void React()
            {
                foreach (GameObject go in Manager.coz)
                {
                    if(go.GetComponent<Zombie>()) // Citizen Run From zombie
                    {
                        float dist = Vector3.Distance(go.transform.position, transform.position);

                        if(dist <= 5)
                        {
                            citizenProperties.behaviour = Behaviour.setAttack;
                            transform.position = Vector3.MoveTowards(transform.position, go.transform.position, -1f);
                        }
                    }
                }
            }
            #endregion

            #region Main Methods

            public override void Start()
            {
                Init(gameObject); // Movement
                base.Start(); // Start Start() from CharacterBehaviour()
            }

            void Update()
            {
                // Gizmos
                goZombies = FindObjectsOfType(typeof(GameObject)) as GameObject[];
                float distMin = 1000;
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

                DisplayDrawLine(goZombies[index], Color.yellow, "Long"); // Gizmos
                // End Gizmos
            }
            #endregion
        }
    }
}
