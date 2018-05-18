using UnityEngine;
using NPC.Ally;

namespace NPC
{
    namespace Enemy
    {
        [RequireComponent(typeof(SphereCollider))]

        public class Zombie : CharacterBehaviour
        {
            Hero hero; // Set Damage To Hero
            GameObject[] goCitizens;

            // Audio
            AudioSystem audioManagerZombie;
            // End Audio

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

            Color ZombieColor() // Color In Regards With Age
            {
                Color color = new Color();
                if (zombieProperties.age >= 70) color = Color.green;
                if (zombieProperties.age >= 30 && zombieProperties.age < 70) color = Color.magenta;
                if (zombieProperties.age >= 15 && zombieProperties.age < 30) color = Color.cyan;
                return color;
            }
            #endregion

            #region Audio

            void AudioClip(float distance)
            {
                if (distance > 10 && distance < 15)
                {
                    audioManagerZombie.SetClip(SFX.DistantRoar);
                }
                if (distance > 5 && distance < 10)
                {
                    audioManagerZombie.SetClip(SFX.MidRoar);
                }
                if (distance > 0 && distance < 5)
                {
                    audioManagerZombie.SetClip(SFX.CloseRoar);
                }
            }
            #endregion

            #region Attack

            void OnCollisionEnter(Collision collision) // Cast
            {
                if (collision.gameObject.GetComponent<Citizen>())
                {
                    Citizen c = collision.gameObject.GetComponent<Citizen>();
                    Zombie z = c;
                }

                if (collision.gameObject.GetComponent<Hero>()) // Damage
                {
                    hero = collision.gameObject.GetComponent<Hero>();

                    if (gameObject.GetComponent<Renderer>().material.color == Color.cyan)
                    {
                        hero.BeDamaged(2);
                    }
                    else if (gameObject.GetComponent<Renderer>().material.color == Color.magenta)
                    {
                        hero.BeDamaged(1);
                    }
                    else if (gameObject.GetComponent<Renderer>().material.color == Color.green)
                    {
                        hero.BeDamaged(.5f);
                    }
                }
            }
            #endregion

            #region Main Methods

            public override void Start()
            {
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", ZombieColor()); // Set Object Color

                // Name
                if (zombiefied == false) gameObject.name = ZombieName(ZombieColor()); // Natural Zombie Name
                else if (zombiefied == true) gameObject.name = gameObject.name + " (" + ZombieName(gameObject.GetComponent<Renderer>().material.color) + ")"; // Keep Citizen Name
                // End Name

                // Collision Message
                gameObject.tag = "Zombie";
                SphereCollider sc = gameObject.GetComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 5f; // Zombie Sight Range (5 units)
                zombieProperties.bodyPart = " "; // "null", No Taste Yet
                // End Collision Message

                // Audio
                GameObject heroAs = GameObject.FindGameObjectWithTag("Player") as GameObject;
                audioManagerZombie = heroAs.GetComponent<AudioSystem>() as AudioSystem;
                // End Audio

                base.Start(); // Start Start() from CharacterBehaviour.cs
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

                AudioClip(CharacterBehaviour.heroToZombie); // Use dist From CharacterBehaviour Class
            }
            #endregion
        }
    }
}