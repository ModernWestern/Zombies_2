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
    // Canvas
    Text messages;
    Image bg;
    // End Canvas

    #region Init

    public void Init(GameObject body, string name, int age, Text text, Image image)
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
        bg = image;
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
            bg.color = Color.black; // Text Color in Regards With Zombie Color

            if (zombie.zombieProperties.bodyPart == " ") // If Zombie bodyPart is "null" Allocate a Taste
            {
                int index = Random.Range(0, (int)Taste.Lenght);
                zombie.zombieProperties.taste = (Taste)index;
                zombie.zombieProperties.bodyPart = zombie.zombieProperties.taste.ToString();
                
                messages.text = "Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart;
                messages.color = zombie.GetComponent<Renderer>().material.color; // Text Color in Regards With Zombie Color
            }
            else // If Zombie bodyPart is !null Just Print
            {
                messages.text = "Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart;
                messages.color = zombie.GetComponent<Renderer>().material.color; // Text Color in Regards With Zombie Color
            }
        }
        // End Zombie Taste

        // Citizen Info
        else if (coll.CompareTag("Citizen"))
        {
            bg.color = Color.black;
            Citizen citizen = coll.GetComponent<Citizen>();
            messages.color = citizen.GetComponent<Renderer>().material.color; // Text Color in Regards With Zombie Color
            messages.text = citizen.citizenProperties.info;
        }
        // End Citizen Info
    }

    IEnumerator Cleaner() // Clena Messages and Background Field
    {
        yield return new WaitForSeconds(1);
        messages.text = " ";
        bg.color = new Color(0, 0, 0, 0);
        StopCoroutine("Cleaner");
    }

    void OnTriggerExit(Collider coll)
    {
        StartCoroutine("Cleaner");
    }
    #endregion
}