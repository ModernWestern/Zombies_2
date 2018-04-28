using UnityEngine;
using NPC.Ally;

namespace NPC
{
    namespace Enemy
    {
        [RequireComponent(typeof(SphereCollider))]

        public class Zombie : CharacterBehaviour
        {
            GameObject[] goCitizens;

            #region Init

            public void Init(GameObject zombieBody, int age) // Zombie Generator
            {
                zombieProperties.age = age;
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

            Color ZombieColor()
            {
                Color color = new Color();
                if (zombieProperties.age >= 70) color = Color.green;
                if (zombieProperties.age >= 30 && zombieProperties.age < 70) color = Color.magenta;
                if (zombieProperties.age >= 15 && zombieProperties.age < 30) color = Color.cyan;
                return color;
            }

            void OnCollisionEnter(Collision collision) // Bite ***
            {
                if (collision.gameObject.GetComponent<Citizen>())
                {
                    Citizen c = collision.gameObject.GetComponent<Citizen>();
                    Zombie z = c;
                }
            }
            #endregion

            #region Main Methods

            public override void Start()
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", ZombieColor()); // Set Object Color

                // Name
                if (zombiefied == false) gameObject.name = ZombieName(ZombieColor());
                else if (zombiefied == true) gameObject.name = gameObject.name + " (" + ZombieName(gameObject.GetComponent<Renderer>().material.color) + ")";
                // End Name

                // Collision Message
                gameObject.tag = "Zombie";
                SphereCollider sc = gameObject.GetComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 5f; // Zombie Sight Range (5 units)
                zombieProperties.bodyPart = " "; // "null", No Taste Yet
                // End Collision Message
                
                base.Start(); // Start Start() from CharacterBehaviour()
            }

            void Update()
            {
                // Gizmos
                goCitizens = FindObjectsOfType(typeof(GameObject)) as GameObject[];
                float distMin = 1000; // Minimum Value Gonna Be This Until Something Become Smaller
                int index = 0;

                for (int i = 0; i < goCitizens.Length; i++)
                {
                    if (goCitizens[i].GetComponent<Citizen>() || goCitizens[i].GetComponent<Hero>()) // Find Citizens or Hero
                    {
                        if (distMin > Vector3.Distance(transform.position, goCitizens[i].transform.position))
                        {
                            distMin = Vector3.Distance(transform.position, goCitizens[i].transform.position);
                            index = i;
                        }
                    }
                }

                DisplayDrawLine(goCitizens[index], ZombieColor()); // Gizmos
                // End Gizmos
            }
            #endregion
        }
    }
}