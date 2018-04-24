using UnityEngine;
using NPC.Ally;

namespace NPC
{
    namespace Enemy
    {
        public class Zombie : CharacterBehaviour
        {
            Color zColor;
            GameObject[] goCitizens;
            GameObject atStake;

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

                //zombieBody.AddComponent<Rigidbody>();
                //Rigidbody rb = zombieBody.GetComponent<Rigidbody>();
                //rb.interpolation = RigidbodyInterpolation.Extrapolate;
                //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                //rb.constraints = RigidbodyConstraints.FreezeRotation;

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
                float speed = 0f;
                zColor = gameObject.GetComponent<Renderer>().material.color; // Get Color
                if (zColor == Color.cyan) speed = 7.5f;
                if (zColor == Color.magenta) speed = 5f;
                if (zColor == Color.green) speed = 2.5f;
                return speed;
            }
            #endregion

            #region Main Methods

            void Start()
            {
                zombieProperties.bodyPart = " "; // "null", No Taste Yet

                goCitizens = FindObjectsOfType(typeof(GameObject)) as GameObject[];

                foreach (GameObject go in goCitizens)
                {
                    Component cComp = go.GetComponent(typeof(Hero)); // Any Object With This Component Gonna Be Chased
                    if (cComp != null) atStake = go;
                }

                Init(gameObject, ZombieSpeed()); // Movement
                DisplayDrawLine(atStake, zColor , "Long"); // Gizmos
            }
            #endregion
        }
    }
}