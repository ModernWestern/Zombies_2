using UnityEngine;
using System.Collections;
using NPC.Ally;
using NPC.Enemy;

public struct SetBehaviour // Store a Behaviour in Regards With a Sort Of Character
{
    public Behaviour behaviour;
    public float speed;
}

[RequireComponent(typeof(Rigidbody))]

public class CharacterBehaviour : MonoBehaviour
{
    public static bool zombiefied; // Check If You've Already been Bitten

    // Display Gizmos
    GameObject target;
    Vector3 direction;
    Color lineColor;
    // End Display Gizmos

    public SetBehaviour setBehaviour;
    public ZombieProperties zombieProperties;
    public CitizenProperties citizenProperties;
    float partialTime;
    int pickBehave;
    int whichWay;

    #region Gizmos

    public void DisplayDrawLine(GameObject obj, Color color)
    {
        target = obj;
        lineColor = color;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        if (Manager.gizmoSwitch == true)
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
    #endregion

    #region Character Behaviour

    void SetRigidBody()
    {        
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    float SpeedPerAge(int age) // Use age property to set speed
    {
        float speed = 0f;
        if (age >= 70) speed = Random.Range (2.5f, 3.6f);
        else if (age >= 30 && age < 70) speed = Random.Range(5f, 6.1f);
        else if (age >= 15 && age < 30) speed = 7.5f;
        return speed;
    }

    public void COZ()
    {
        switch (gameObject.tag)
        {
            case "Zombie":
                setBehaviour.behaviour = zombieProperties.behaviour;
                setBehaviour.speed = SpeedPerAge(zombieProperties.age);
                break;
            case "Citizen":
                setBehaviour.behaviour = citizenProperties.behaviour;
                setBehaviour.speed = SpeedPerAge(citizenProperties.age);
                break;
            default:
                print("Error");
                break;
        }
    }
    #endregion

    #region Movement Core

    IEnumerator RefreshState() // Every 3 seconds call PickState
    {
        yield return new WaitForSeconds(partialTime);
        PartialTime(out partialTime); // Make PartialTime() a New Value Each Call
        PickState();
    }

    void PickState() // Move or Idle
    {
        setBehaviour.behaviour = (Behaviour)(Random.Range(0, 3)); // Random State

        if (setBehaviour.behaviour == Behaviour.getMove)
        {
            whichWay = Random.Range(0, 2); // Forward or Backward
            StartCoroutine("RefreshState");
        }
        else if (setBehaviour.behaviour == Behaviour.getIdle)
        {
            whichWay = 2; // Idle Case
            StartCoroutine("RefreshState");
        }
        else if (setBehaviour.behaviour == Behaviour.getRotate)
        {
            whichWay = 3; // Rotate Case
            StartCoroutine("RefreshState");
        }
        else if (setBehaviour.behaviour == Behaviour.getReaction)
        {
            whichWay = 5; // ******************
            //StopCoroutine("RefreshState"); // *********************
            print("ATTACK"); // ***********************
        }
    }

    void MoveIt() // Move Zombie Randomly
    {
        if (transform.position.x >= -45 && transform.position.x <= 45 && transform.position.z >= -45 && transform.position.z <= 45) // Boundaries
        {
            switch (whichWay)
            {
                case 0:
                    transform.Translate(Vector3.forward * (Time.deltaTime * setBehaviour.speed), Space.Self);
                    break;
                case 1:
                    transform.Translate(Vector3.back * (Time.deltaTime * setBehaviour.speed), Space.Self);
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
                case 5:
                    print("HHHHHHHHH");
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

    #region Citizen And Zombie React

    void React() // Attack/RunAway
    {
        foreach (GameObject go in Manager.coz)
        {
            if (gameObject.tag == "Zombie")
            {
                if (go.GetComponent<Hero>() || go.GetComponent<Citizen>()) // Zombie Attack Citizens and Hero
                {
                    float dist = Vector3.Distance(go.transform.position, transform.position);

                    if (dist <= 15 && dist >= .1f)
                    {
                        StopCoroutine(RefreshState()); // Stop Case Behaviours
                        setBehaviour.behaviour = Behaviour.getReaction;
                        float dynamicSpeed = Mathf.Clamp(dist * 4, 40, 60); // As Much Closer, Faster
                        transform.position = Vector3.MoveTowards(transform.position, go.transform.position, setBehaviour.speed / dynamicSpeed);
                    }
                }
            }
            if (gameObject.tag == "Citizen")
            {
                if (go.GetComponent<Zombie>()) // Citizen Run From zombie
                {
                    float dist = Vector3.Distance(go.transform.position, transform.position);

                    if (dist <= 15 && dist >= .1f)
                    {
                        StopCoroutine(RefreshState()); // Stop Case Behaviours
                        setBehaviour.behaviour = Behaviour.getReaction;
                        float dynamicSpeed = Mathf.Clamp(dist * 4, 40, 60); // As Much Closer, Faster
                        transform.position = Vector3.MoveTowards(transform.position, go.transform.position, -setBehaviour.speed / dynamicSpeed);
                    }
                }
            }
        }
    }
    #endregion

    void Awake()
    {
        SetRigidBody();
    }

    public virtual void Start() // Class Citizen or Class Zombie Gonna Take Control About Start()
    {
        COZ();
        PartialTime(out partialTime); // Start With Delay (less robotic)
        PickState();
        StartCoroutine("RefreshState");
    }

    void FixedUpdate()
    {
        MoveIt(); // Move
        React();
    }
}
