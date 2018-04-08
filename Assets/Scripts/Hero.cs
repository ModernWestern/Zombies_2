﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public void Init(GameObject body, string name, int age)
    {
        body.tag = "Player";
        body.name = name.ToUpper() + age; // Hero Name

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
        movement.walk = AgeSprint(age); // Age Speed
        // END SCRIPTS

        // GUN
        GameObject gun = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a Gun
        gun.name = "Gun";
        gun.transform.position = new Vector3(.38f, -.4f, .4f);
        gun.transform.localScale = new Vector3(.28f, .26f, 1f);
        gun.transform.SetParent(body.transform); // Rig
        gun.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        // END GUN

        // CAM
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera"); // Find Main Camera
        cam.name = "Cam";
        cam.transform.SetParent(body.transform);
        // END CAM
    }

    float AgeSprint(int age) // Speed Generator
    {
        float speed = 0;
        if (age >= 70) speed = .1f;
        else if (age >= 30 && age < 70) speed = .15f;
        else if (age >= 15 && age < 30) speed = .2f;
        return speed;
    }
    
    void OnTriggerEnter(Collider coll) // Messages
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
                print("Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart);
            }
            else // If Zombie bodyPart is !null Just Print
            {
                print("Roarrr, I'm starving, yummy... " + zombie.zombieProperties.bodyPart);
            }
        }
        // End Zombie Taste

        // Citizen Info
        else if (coll.CompareTag("Citizen"))
        {
            Citizen citizen = coll.GetComponent<Citizen>();
            print(citizen.citizenProperties.info);
        }
        // End Citizen Info
    }
}