using UnityEngine;
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
    ManagerUI managerUI;
    Color bgCol = new Color(0, 0, 0, .5f);
    // End Canvas

    // Health
    float quiteGood = health / 2;
    float good = health / 3;
    float bad = health / health;
    // End Health

    // Attack
    ZombieProperties zombieProperties;
    // End Attack

    #region Init

    public void Init(GameObject body, string name, int age)
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

        body.AddComponent<AudioManager>(); // Set AudioManager Class
        AudioSource heroAS = GetComponent<AudioSource>(); // Get AudioSource Componente Created By AudioManager Class
        heroAS.playOnAwake = false; // Set AudioSource PlayOnAwake Parameter
        heroAS.volume = .05f;
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

        managerUI = FindObjectOfType<ManagerUI>(); // Call Class
    }
    #endregion

    #region Messages

    void OnTriggerEnter(Collider coll)
    {
        // Zombie Taste
        if (coll.CompareTag("Zombie"))
        {
            Zombie zombie = coll.GetComponent<Zombie>(); // Call (Get) Zombie Class
            managerUI.images[0].color = bgCol; // Text Color in Regards With Zombie Color

            if (zombie.zombieProperties.bodyPart == " ") // If Zombie bodyPart is "null" Allocate a Taste
            {
                int index = Random.Range(0, (int)Taste.DUMMY_ELEMENT);
                zombie.zombieProperties.taste = (Taste)index;
                zombie.zombieProperties.bodyPart = zombie.zombieProperties.taste.ToString();
                
                managerUI.texts[2].text = "Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart;
                managerUI.texts[2].color = zombie.GetComponent<Renderer>().material.color; // Text Color in Regards With Zombie Color

                zombieProperties.behaviour = Behaviour.getReaction;
            }
            else // If Zombie bodyPart is !null Just Print
            {
                managerUI.texts[2].text = "Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart;
                managerUI.texts[2].color = zombie.GetComponent<Renderer>().material.color; // Text Color in Regards With Zombie Color
                zombieProperties.behaviour = Behaviour.getReaction;
            }
        }
        // End Zombie Taste

        // Citizen Info
        else if (coll.CompareTag("Citizen"))
        {
            managerUI.images[0].color = bgCol;
            Citizen citizen = coll.GetComponent<Citizen>();
            managerUI.texts[2].color = citizen.GetComponent<Renderer>().material.color; // Text Color in Regards With Zombie Color
            managerUI.texts[2].text = citizen.citizenProperties.info;
        }
        // End Citizen Info
    }

    IEnumerator Cleaner() // Clena Messages and Background Field
    {
        yield return new WaitForSeconds(1);
        managerUI.texts[2].text = " ";
        managerUI.images[0].color = new Color(0, 0, 0, 0);
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

        managerUI.sliders[0].value = health; // Update Health Bar

        if (health <= 0) Die();
    }

    void Die()
    {
        managerUI.images[1].enabled = true;
        Destroy(gameObject.GetComponent<Hero>()); // Stop Chasing Me
        Destroy(gameObject.GetComponent<FPS_move>()); // Don't Move

        for (int i = 0; i < managerUI.canvasGroups.Length; i++) // Active All Feedbacks
        {
            managerUI.canvasGroups[i].alpha = 1;
        }

        managerUI.texts[2].text = " "; // Clean Texts
        managerUI.images[0].color = new Color(0, 0, 0, 0); // Clean Background
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
        for (int i = 0; i < managerUI.canvasGroups.Length; i++)
        {
            StartCoroutine(HitFade(managerUI.canvasGroups[i], managerUI.canvasGroups[i].alpha, 1, inTime));
        }
    }

    void HitOut(float outTime)
    {
        for (int i = 0; i < managerUI.canvasGroups.Length; i++)
        {
            StartCoroutine(HitFade(managerUI.canvasGroups[i], managerUI.canvasGroups[i].alpha, 0, outTime));
        }
    }
    // End Canvas
    #endregion
}