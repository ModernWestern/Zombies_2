using UnityEngine;
using NPC.Ally;

namespace NPC
{
    namespace Enemy
    {
        public class Zombie : CharacterBehaviour
        {
            GameObject[] goCitizens;
            string zName;

            #region Init

            public void Init(GameObject zombieBody, int age) // Zombie Generator
            {
                // Collision Message
                zombieBody.name = zName;
                zombieBody.tag = "Zombie";
                zombieBody.AddComponent<SphereCollider>();
                SphereCollider sc = zombieBody.GetComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 5f; // Zombie Sight Range (5 units)
                // End Collision Message

                zombieProperties.age = age;
                zombieBody.GetComponent<Renderer>().material.SetColor("_Color", ZombieColor()); // Set Object Color
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
                if (this.zombieProperties.age >= 70) color = Color.green;
                if (this.zombieProperties.age >= 30 && this.zombieProperties.age < 70) color = Color.magenta;
                if (this.zombieProperties.age >= 15 && this.zombieProperties.age < 30) color = Color.cyan;
                return color;
            }

            public override void React()
            {
                foreach (GameObject go in Manager.coz)
                {
                    if (go.GetComponent<Hero>() || go.GetComponent<Citizen>()) // Zombie Attack Citizens and Hero
                    {
                        float dist = Vector3.Distance(go.transform.position, transform.position);

                        if (dist <= 5)
                        {
                            zombieProperties.behaviour = Behaviour.setAttack;
                            transform.position = Vector3.MoveTowards(transform.position, go.transform.position, .1f);
                        }
                    }
                }
            }
            #endregion

            #region Main Methods

            public override void Start()
            {
                zName = ZombieName(ZombieColor());
                zombieProperties.bodyPart = " "; // "null", No Taste Yet
                Init(gameObject); // Movement
                base.Start(); // Start Start() from CharacterBehaviour()
            }

            void Update()
            {
                // Gizmos
                goCitizens = FindObjectsOfType(typeof(GameObject)) as GameObject[];
                float distMin = 1000;
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

                DisplayDrawLine(goCitizens[index], ZombieColor(), "Long"); // Gizmos
                // End Gizmos
            }
            #endregion
        }
    }
}