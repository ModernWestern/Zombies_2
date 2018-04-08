using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    public ZombieProperties zombieProperties;
    WaitForSeconds waitFive = new WaitForSeconds(5);
    int pickBehave;
    int whichWay;

    #region Methods

    public void Init(GameObject zombieBody, Color color) // Zombie Generator
    {
        // Collision Message
        zombieBody.tag = "Zombie";
        zombieBody.AddComponent<SphereCollider>();
        SphereCollider sc = zombieBody.GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 1f;
        // End Collision Message

        zombieBody.AddComponent<Rigidbody>();
        Rigidbody rb = zombieBody.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        zombieBody.GetComponent<Renderer>().material.SetColor("_Color", color); // Set Object Color
    }

    IEnumerator RefreshState() // Every 5 seconds call PickState
    {
        yield return waitFive;
        PickState();
    }

    void PickState() // Move or Idle
    {
        zombieProperties.behaviour = (Behaviour)(Random.Range(0, 2)); // Random State

        if (zombieProperties.behaviour == Behaviour.getMove)
        {
            whichWay = Random.Range(0,8); // Call a Case
            StartCoroutine("RefreshState");
        }
        else if (zombieProperties.behaviour == Behaviour.getIdle)
        {
            whichWay = 8; // Idle Case
            StartCoroutine("RefreshState");
        }
    }

    float ZombieSpeed()
    {
        float speed = 0;
        Color zColor; // Store Color From Object
        zColor = this.gameObject.GetComponent<Renderer>().material.color; // Get Color

        if (zColor == Color.cyan)
        {
            speed = 7.5f;
        }
        if (zColor == Color.magenta)
        {
            speed = 5f;
        }
        if (zColor == Color.green)
        {
            speed = 2.5f;
        }
        return speed;
    }

    void MoveIt() // Move Zombie Randomly
    {
        if (this.transform.position.x >= -45 && this.transform.position.x <= 45 && this.transform.position.z >= -45 && this.transform.position.z <= 45) // Boundaries
        {
            switch (whichWay)
            {
                case 0: 
                    transform.Translate(Vector3.forward * (Time.deltaTime * ZombieSpeed()), Space.World);
                    break;
                case 1:
                    transform.Translate((Vector3.forward + Vector3.right) * (Time.deltaTime * ZombieSpeed()), Space.World);
                    break;
                case 2:
                    transform.Translate((Vector3.forward + Vector3.left) * (Time.deltaTime * ZombieSpeed()), Space.World);
                    break;
                case 3:
                    transform.Translate(Vector3.back * (Time.deltaTime * ZombieSpeed()), Space.World);
                    break;
                case 4:
                    transform.Translate((Vector3.back + Vector3.right) * (Time.deltaTime * ZombieSpeed()), Space.World);
                    break;
                case 5:
                    transform.Translate((Vector3.back + Vector3.left) * (Time.deltaTime * ZombieSpeed()), Space.World);
                    break;
                case 6:
                    transform.Translate(Vector3.right * (Time.deltaTime * ZombieSpeed()), Space.World);
                    break;
                case 7:
                    transform.Translate(Vector3.left * (Time.deltaTime * ZombieSpeed()), Space.World);
                    break;
                case 8:
                    transform.Translate(Vector3.zero);
                    break;
                case 9:
                    transform.Translate(Vector3.up * (.075f * Mathf.Sin(1)));
                    break;
                default:
                    print("Nothing");
                    break;
            }
        }
        else if (this.transform.position.x < -45) // If Get Stuck
        {
            Vector3 bounce = this.transform.position;
            bounce.x = -35;
            this.transform.position = Vector3.Lerp(this.transform.position, bounce, Time.deltaTime);
            whichWay = 9;
        }
        else if (this.transform.position.x > 45) // If Get Stuck
        {
            Vector3 bounce = this.transform.position;
            bounce.x = 35;
            this.transform.position = Vector3.Lerp(this.transform.position, bounce, Time.deltaTime);
            whichWay = 9;
        }
        else if (this.transform.position.z < -45) // If Get Stuck
        {
            Vector3 bounce = this.transform.position;
            bounce.z = -35;
            this.transform.position = Vector3.Lerp(this.transform.position, bounce, Time.deltaTime);
            whichWay = 9;
        }
        else if (this.transform.position.z > 45) // If Get Stuck
        {
            Vector3 bounce = this.transform.position;
            bounce.z = 35;
            this.transform.position = Vector3.Lerp(this.transform.position, bounce, Time.deltaTime);
            whichWay = 9;
        }
    }

    #endregion

    #region Main Methods

    void Start()
    {
        zombieProperties.bodyPart = " "; // "null", No Taste Yet
        PickState();
        StartCoroutine("RefreshState");
    }

    void FixedUpdate()
    {
        MoveIt();
    }

    #endregion
}
