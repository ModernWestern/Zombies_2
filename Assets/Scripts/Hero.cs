using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NPC.Ally;
using NPC.Enemy;

#region ReadOnly

public class HeroSpeed
{
    public readonly float heroSpeed;

    public HeroSpeed(int age) // Speed Generator
    {
        if (age >= 70) heroSpeed = .1f;
        else if (age >= 30 && age < 70) heroSpeed = .15f;
        else if (age >= 15 && age < 30) heroSpeed = .2f;
    }
}
#endregion

public class Hero : MonoBehaviour
{
    Text messages;

    #region Init

    public void Init(GameObject body, string name, int age, Text text)
    {
        body.tag = "Player";
        body.name = name.ToUpper() + " " + age; // Hero Name

        // RIGIDBODY
        body.AddComponent<Rigidbody>();
        Rigidbody rb = body.GetComponent<Rigidbody>();
        rb.mass = 60f;
        rb.drag = 6f;
        rb.angularDrag = .05f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        // END RIGIDBODY

        // COLLIDER
        CapsuleCollider cc = body.GetComponent<CapsuleCollider>();
        cc.radius = .5f;
        cc.height = 2f;
        cc.direction = 1; // 0 = x, 1 = y, 2 = z
        // END COLLIDER 

        // SCRIPTS
        body.AddComponent<CustomGravity>();
        CustomGravity cg = body.GetComponent<CustomGravity>();
        cg.gravityScale = 15;

        body.AddComponent<FPS_cam>();
        body.AddComponent<FPS_move>();
        FPS_move movement = body.GetComponent<FPS_move>();

        HeroSpeed speed = new HeroSpeed(age);
        movement.walk = speed.heroSpeed; // Age Speed
        // END SCRIPTS

        // GUN
        GameObject gun = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a Gun
        gun.name = "Gun";
        gun.transform.position = new Vector3(.4f, -.4f, -.02f);
        gun.transform.localScale = new Vector3(.28f, .26f, 1f);
        gun.transform.SetParent(body.transform); // Rig
        gun.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        // END GUN

        // CAM
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera"); // Find Main Camera
        cam.name = "Cam";
        cam.transform.SetParent(body.transform);
        // END CAM

        // CANVAS
        messages = text;
        // END CANVAS
    }
    #endregion

    #region Messages

    void OnTriggerEnter(Collider coll)
    {
        // Zombie Taste
        if (coll.CompareTag("Zombie"))
        {
            Zombie zombie = coll.GetComponent<Zombie>(); // Call (Get) Zombie Class

            if (zombie.zombieProperties.bodyPart == " ") // If Zombie bodyPart is "null" Allocate a Taste
            {
                int index = Random.Range(0, (int)Taste.Lenght);
                zombie.zombieProperties.taste = (Taste)index;
                zombie.zombieProperties.bodyPart = zombie.zombieProperties.taste.ToString();

                messages.text = "Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart;
            }
            else // If Zombie bodyPart is !null Just Print
            {
                messages.text = "Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart;
            }
        }
        // End Zombie Taste

        // Citizen Info
        else if (coll.CompareTag("Citizen"))
        {
            Citizen citizen = coll.GetComponent<Citizen>();
            messages.text = citizen.citizenProperties.info;
        }
        // End Citizen Info
    }

    void PartialTime(out float t)
    {
        t = Random.Range(3.0f, 3.6f); // Faux Delay
    }

    IEnumerator Cleaner()
    {
        yield return new WaitForSeconds(1);
        messages.text = " ";
        StopCoroutine("Cleaner");
    }

    void OnTriggerExit(Collider coll)
    {
        StartCoroutine("Cleaner");
    }
    #endregion
}