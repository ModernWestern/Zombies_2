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

[RequireComponent(typeof(Rigidbody))]

public class Hero : MonoBehaviour
{
    public static float health;

    // Canvas
    Text messages;
    Image bg;
    Color bgCol = new Color(0, 0, 0, .5f);
    CanvasGroup canvasGroup;
    float quiteGood = Hero.health / 2;
    float good = Hero.health / 3;
    float bad = Hero.health / Hero.health;
    // End Canvas

    // Attack
    ZombieProperties zombieProperties;
    // End Attack

    #region Init

    public void Init(GameObject body, string name, int age, Text text, Image image, CanvasGroup blood)
    {
        body.tag = "Player";
        body.name = name.ToUpper() + " " + age; // Hero Name

        #region Components

        // RIGIDBODY
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
        // END SCRIPTS

        // GUN
        GameObject gun = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a Gun
        gun.name = "Gun";
        gun.transform.position = new Vector3(.4f, -.4f, -.02f);
        gun.transform.localScale = new Vector3(.28f, .26f, 1f);
        gun.transform.SetParent(body.transform); // Rig
        gun.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        // END GUN
        #endregion

        HeroSpeed speed = new HeroSpeed(age);
        movement.walk = speed.heroSpeed; // Age Speed

        // CAM
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera"); // Find Main Camera
        cam.name = "Cam";
        cam.transform.SetParent(body.transform);
        // END CAM

        // CANVAS
        messages = text;
        bg = image;
        canvasGroup = blood;
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
            bg.color = bgCol; // Text Color in Regards With Zombie Color

            if (zombie.zombieProperties.bodyPart == " ") // If Zombie bodyPart is "null" Allocate a Taste
            {
                int index = Random.Range(0, (int)Taste.Lenght);
                zombie.zombieProperties.taste = (Taste)index;
                zombie.zombieProperties.bodyPart = zombie.zombieProperties.taste.ToString();
                
                messages.text = "Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart;
                messages.color = zombie.GetComponent<Renderer>().material.color; // Text Color in Regards With Zombie Color

                zombieProperties.behaviour = Behaviour.getReaction;
            }
            else // If Zombie bodyPart is !null Just Print
            {
                messages.text = "Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart;
                messages.color = zombie.GetComponent<Renderer>().material.color; // Text Color in Regards With Zombie Color
                zombieProperties.behaviour = Behaviour.getReaction;
            }
        }
        // End Zombie Taste

        // Citizen Info
        else if (coll.CompareTag("Citizen"))
        {
            bg.color = bgCol;
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
        zombieProperties.behaviour = Behaviour.getIdle;
    }
    #endregion

    #region TakeDamage

    public void BeDamaged(float amount)
    {
        health -= amount;

        if (health > quiteGood)
        {
            StartCoroutine(BloodTime(.2f, .5f, 1)); // Attack, Sustain, Release
            //print("GOOD+");
        }
        else if (health > good && health < quiteGood)
        {
            StartCoroutine(BloodTime(.2f, 1.5f, 1.5f));
            //print("GOOD");
        }
        else if (health > bad -1 && health < good)
        {
            StartCoroutine(BloodTime(.2f, 2.5f, 2));
            //print("BAD");
        }

        if (health <= 0) Die();
    }

    void Die()
    {
        print("I'M DIE");
        GameObject thisComponent = FindObjectOfType(typeof(Hero)) as GameObject;
        Destroy(thisComponent);
    }

    // Canvas

    IEnumerator HitFade(CanvasGroup cg, float a, float b, float time)
    {
        float tLerp = Time.time;
        float tStarted = Time.time - tLerp;
        float complete = tStarted / time;

        while (true)
        {
            tStarted = Time.time - tLerp;
            complete = tStarted / time;

            float currentVal = Mathf.SmoothStep(a, b, complete);
            if (complete >= 1) break;
            cg.alpha = currentVal;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator BloodTime(float a, float s, float r)
    {
        HitIn(a);
        yield return new WaitForSeconds(r);
        HitOut(s);
    }

    void HitIn(float inTime)
    {
        StartCoroutine(HitFade(canvasGroup, canvasGroup.alpha, 1, inTime));
    }

    void HitOut(float outTime)
    {
        StartCoroutine(HitFade(canvasGroup, canvasGroup.alpha, 0, outTime));
    }
    // End Canvas
    #endregion
}