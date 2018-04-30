﻿using UnityEngine;
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

    // Display Gizmos
    GameObject target;
    Vector3 direction;
    Color lineColor;
    // End Display Gizmos

    public SetBehaviour setBehaviour;
    public ZombieProperties zombieProperties;
    public CitizenProperties citizenProperties;
    int fob; // Forward Or Backward
    float partialTime;
    bool checkCourutine;
    public static bool zombiefied; // Check If You've Already been Bitten

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
        PartialTime(out partialTime); // Start With Delay (less robotic)
        checkCourutine = true;
        setBehaviour.behaviour = (Behaviour)Random.Range(0, 3);
        fob = Random.Range(0, 2); // Forward Or Backward
        yield return new WaitForSeconds(partialTime);
        checkCourutine = false;
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
                case Behaviour.getMove:
                    if (fob == 0) transform.Translate(Vector3.forward * (Time.deltaTime * setBehaviour.speed), Space.Self);
                    else if (fob == 1) transform.Translate(Vector3.back * (Time.deltaTime * setBehaviour.speed), Space.Self);
                    //print("MOV" + fob);
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
                    print("REACT");
                    break;
                default:
                    print("Nothing");
                    break;
            }
        }
        else if (transform.position.x < -45) // If Get Stuck
        {
            Vector3 bounce = transform.position;
            bounce.x = -30;
            transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
        }
        else if (transform.position.x > 45) // If Get Stuck
        {
            Vector3 bounce = transform.position;
            bounce.x = 30;
            transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
        }
        else if (transform.position.z < -45) // If Get Stuck
        {
            Vector3 bounce = transform.position;
            bounce.z = -30;
            transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
        }
        else if (transform.position.z > 45) // If Get Stuck
        {
            Vector3 bounce = transform.position;
            bounce.z = 30;
            transform.position = Vector3.Lerp(transform.position, bounce, Time.deltaTime);
        }

        // React
        for (int i = 0; i < Manager.allGo.Count; i++)
        {
            if (gameObject.tag == "Zombie")
            {
                if (allGo[i].GetComponent<Citizen>() || allGo[i].GetComponent<Hero>()) // Zombie Attack Citizens or Hero
                {
                    float dist = Vector3.Distance(allGo[i].transform.position, transform.position);

                    if (dist <= 15 && dist >= .1f)
                    {
                        //StopCoroutine(refreshState); // Stop Case Behaviours
                        setBehaviour.behaviour = Behaviour.getReaction;
                        float dynamicSpeed = Mathf.Clamp(dist * 4, 30, 60); // As Much Closer, Faster
                        transform.position = Vector3.MoveTowards(transform.position, allGo[i].transform.position, setBehaviour.speed / dynamicSpeed);
                    }
                }
            }
            if (gameObject.tag == "Citizen")
            {
                if (allGo[i].GetComponent<Zombie>()) // Citizen Run From zombie
                {
                    float dist = Vector3.Distance(allGo[i].transform.position, transform.position);

                    if (dist <= 15 && dist >= .1f)
                    {
                        //StopCoroutine(refreshState); // Stop Case Behaviours
                        setBehaviour.behaviour = Behaviour.getReaction;
                        float dynamicSpeed = Mathf.Clamp(dist * 4, 30, 60); // As Much Closer, Faster
                        transform.position = Vector3.MoveTowards(transform.position, allGo[i].transform.position, -setBehaviour.speed / dynamicSpeed);
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
        //React();
    }
}
