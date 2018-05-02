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
    // Find Object
    public GameObject[] allGo;
    // End Find Object

    public SetBehaviour setBehaviour;
    public ZombieProperties zombieProperties;
    public CitizenProperties citizenProperties;

    // Display Gizmos
    GameObject target;
    Vector3 direction;
    Color lineColor;
    // End Display Gizmos
    
    float partialTime;
    bool outOfRange = true;
    bool checkCourutine;
    float dynamicSpeed;
    public static bool zombiefied; // Check If You've Already been Bitten
    public float heroDistance; // Distance Value Between Hero And Zombies (AudioSource Parameter)

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
        if (age >= 70) speed = Random.Range (2.5f, 5.1f);
        else if (age >= 30 && age < 70) speed = Random.Range(5f, 7.6f);
        else if (age >= 15 && age < 30) speed = Random.Range(7.5f, 8.1f);
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

    void Reflection()
    {
        if (setBehaviour.behaviour == Behaviour.goForward) setBehaviour.behaviour = Behaviour.goBackward;
        else if (setBehaviour.behaviour == Behaviour.goBackward) setBehaviour.behaviour = Behaviour.goForward;

        Quaternion reflect = transform.localRotation;
        float angleReflected = reflect.y * -1;
        reflect.y = angleReflected;
        transform.localRotation = reflect;
    }
    #endregion

    #region Movement Core

    IEnumerator RefreshState() // Every 3 seconds call PickState
    {
        PartialTime(out partialTime); // Start With Delay (less robotic)

        if (outOfRange == true) // If Distance Is Higher Than 5 Set a Behaviour, Else It's Bitting/Running
        {
            //checkCourutine = true;
            setBehaviour.behaviour = (Behaviour)Random.Range(0, 4);
        }

        yield return new WaitForSeconds(partialTime);
        //checkCourutine = false;
        PartialTime(out partialTime); // Make PartialTime() a New Value Each Call
        yield return new WaitForSeconds(partialTime);
        StartCoroutine(RefreshState());
    }

    void MoveIt() // Move Zombie Randomly
    {
        if (transform.position.x >= -45 && transform.position.x <= 45 && transform.position.z >= -45 && transform.position.z <= 45) // Boundaries
        {
            switch (setBehaviour.behaviour)
            {
                case Behaviour.goForward:
                    transform.Translate(Vector3.forward * (Time.deltaTime * setBehaviour.speed), Space.Self);
                    //print("FORWARD");
                    break;
                case Behaviour.goBackward:
                    transform.Translate(Vector3.back * (Time.deltaTime * setBehaviour.speed), Space.Self);
                    //print("BAKCWARD");
                    break;
                case Behaviour.getIdle:
                    transform.Translate(Vector3.zero);
                    //print("IDLE");
                    break;
                case Behaviour.getRotate:
                    transform.Rotate(Vector3.up);
                    //print("ROT");
                    break;
                case Behaviour.getReaction:
                    outOfRange = false;
                    //print("REACT");
                    break;
                default:
                    print("Nothing");
                    break;
            }
        }
        else if (transform.position.x < -45) // If Get Stuck
        {
            Vector3 bounce = transform.position;
            bounce.x = -20;
            transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
            Reflection();
        }
        else if (transform.position.x > 45) // If Get Stuck
        {
            Vector3 bounce = transform.position;
            bounce.x = 20;
            transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
            Reflection();
        }
        else if (transform.position.z < -45) // If Get Stuck
        {
            Vector3 bounce = transform.position;
            bounce.z = -20;
            transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
            Reflection();
        }
        else if (transform.position.z > 45) // If Get Stuck
        {
            Vector3 bounce = transform.position;
            bounce.z = 20;
            transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
            Reflection();
        }

        // React
        for (int i = 0; i < Manager.allGo.Count; i++)
        {
            if (gameObject.tag == "Citizen")
            {
                if (allGo[i].GetComponent<Zombie>()) // Citizen Run From zombie
                {
                    float dist = Vector3.Distance(allGo[i].transform.position, transform.position);

                    if (dist <= 5 && dist >= .5) // Ruan Away
                    {
                        setBehaviour.behaviour = Behaviour.getReaction;
                        dynamicSpeed = Mathf.Clamp(dist * 6, 30, 90); // As Much Closer, Faster
                        transform.position = Vector3.MoveTowards(transform.position, allGo[i].transform.position, -setBehaviour.speed / dynamicSpeed);
                    }
                    else
                    {
                        outOfRange = true;
                    }
                }
                else if (allGo[i] != gameObject) // Ignore This Gameobject, And Don't Crash Each Other
                {
                    float dist = Vector3.Distance(allGo[i].transform.position, transform.position);
                    
                    if (dist <= 2.5f && dist >= .5f)
                    {
                        setBehaviour.behaviour = Behaviour.getReaction;
                        dynamicSpeed = Mathf.Clamp(dist * 6, 30, 90); // As Much Closer, Faster
                        transform.position = Vector3.MoveTowards(transform.position, allGo[i].transform.position, -setBehaviour.speed / dynamicSpeed);
                    }
                    else
                    {
                        outOfRange = true;
                    }
                }
            }
            if (gameObject.tag == "Zombie")
            {
                if (allGo[i].GetComponent<Citizen>()) // Zombie Attack Citizens
                {
                    float dist = Vector3.Distance(allGo[i].transform.position, transform.position);

                    if (dist <= 15 && dist >= .5f)
                    {
                        setBehaviour.behaviour = Behaviour.getReaction;
                        dynamicSpeed = Mathf.Clamp(dist * 6, 30, 90); // As Much Closer, Faster
                        transform.position = Vector3.MoveTowards(transform.position, allGo[i].transform.position, setBehaviour.speed / dynamicSpeed);
                    }
                    else
                    {
                        outOfRange = true;
                    }
                }
                if (allGo[i].GetComponent<Hero>()) // Zombie Attack Citizens
                {
                    float dist = Vector3.Distance(allGo[i].transform.position, transform.position);

                    heroDistance = dist; // To Audio Parameter

                    if (dist <= 15 && dist >= .5f)
                    {
                        setBehaviour.behaviour = Behaviour.getReaction;
                        dynamicSpeed = Mathf.Clamp(dist * 6, 30, 90); // As Much Closer, Faster
                        transform.position = Vector3.MoveTowards(transform.position, allGo[i].transform.position, setBehaviour.speed / dynamicSpeed);
                    }
                    else
                    {
                        outOfRange = true;
                    }
                }
            }
        }
        // End React
    }

    void PartialTime(out float t) // Faux Delay (less robotic)
    {
        t = Random.Range(3.0f, 3.6f);
    }
    #endregion

    void Awake()
    {
        SetRigidBody();
    }

    public virtual void Start() // Class Citizen or Class Zombie Gonna Take Control About Start()
    {
        allGo = Manager.allGo.ToArray();
        StartCoroutine(RefreshState());
        COZ();
    }

    void FixedUpdate()
    {
        //print("Courutine: " + checkCourutine);

        Vector3 dontFly = gameObject.transform.position;
        dontFly.y = Mathf.Clamp(dontFly.y, -Mathf.Infinity, 1.5f);
        gameObject.transform.position = dontFly;
        
        MoveIt(); // Move
    }
}
