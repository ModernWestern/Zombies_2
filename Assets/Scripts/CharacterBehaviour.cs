using UnityEngine;
using System.Collections;

public struct SetBehaviour // Store a Behaviour in Regards With a Sort Of Character
{
    public Behaviour behaviour;
    public float speed;
}

public class CharacterBehaviour : MonoBehaviour
{
    // Display Gizmos
    GameObject target;
    Vector3 direction;
    Color lineColor;
    string lenght;
    // End Display Gizmos

    public SetBehaviour setBehaviour;
    public ZombieProperties zombieProperties;
    public CitizenProperties citizenProperties;
    float partialTime;
    int pickBehave;
    int whichWay;

    #region Gizmos

    public void DisplayDrawLine(GameObject obj, Color color, string lineLenght)
    {
        target = obj;
        lineColor = color;
        lenght = lineLenght;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        switch (lenght)
        {
            case "Short":
                Gizmos.DrawLine(transform.position, transform.position + direction);
                break;
            case "Long":
                Gizmos.DrawLine(transform.position, target.transform.position);
                break;
            default:
                print("Nothing");
                break;
        }
    }
    #endregion

    #region Character Behaviour

    void SetRigidBody()
    {
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Init(GameObject sortCharacter, float speed)
    {
        switch (sortCharacter.tag)
        {
            case "Zombie":
                setBehaviour.behaviour = zombieProperties.behaviour;
                setBehaviour.speed = speed;
                break;
            case "Citizen":
                setBehaviour.behaviour = citizenProperties.behaviour;
                setBehaviour.speed = speed;
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
            whichWay = Random.Range(0, 2);
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
        else if (setBehaviour.behaviour == Behaviour.setAttack)
        {
            whichWay = 5; // Attack
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
                    direction = Vector3.Normalize(target.transform.position - transform.position);
                    transform.position += direction * .1f;
                    print("ATTACK");
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

    #region BecomeToZombie

    public virtual void Respond()
    {
        foreach(GameObject go in Manager.coz)
        {
            float dist = Vector3.Distance(go.transform.position, transform.position);
            if (dist <= 5f)
            {
                // Nothing
            }
        }
    }
    #endregion

    void FixedUpdate()
    {
        MoveIt();
    }

    void Start()
    {
        //SetRigidBody();
        PartialTime(out partialTime); // Start With Delay (less robotic)
        PickState();
        StartCoroutine("RefreshState");
    }

    void Awake()
    {
        SetRigidBody();
    }
}
