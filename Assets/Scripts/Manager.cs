﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC.Ally;
using NPC.Enemy;

#region ReadOnly

public class MinSpawn
{
    public readonly int Min;
    public MinSpawn(ref int min)
    {
        Min = min;
    }
}
#endregion

public class Manager : MonoBehaviour
{
    public string namesFile; // Public Field To Type File Name
    public bool cursorLock = false;

    // Scene Characters
    GameObject hero;
    GameObject zObject;
    GameObject cObject;
    // End Scene Characters

    // Canvas
    public static List<GameObject> coz = new List<GameObject>(); // Store Game Objects
    public Text[] text;
    public Image[] images;
    Color onlyAlpha;
    // End Canvas

    // Spawn
    int RandomMin;
    const int ConstMax = 25;
    // End Spawn

    #region Canvas

    void DisplayQuantity()
    {
        int zombieQ = 0;
        int citizenQ = 0;
        foreach (GameObject gameObjecs in coz)
        {
            if (gameObjecs.tag == "Zombie") zombieQ++;
            else if (gameObjecs.tag == "Citizen") citizenQ++;
        }
        text[0].text = zombieQ.ToString(); // Canvas
        text[1].text = citizenQ.ToString(); // Canvas
    }
    #endregion
    
    #region Methods

    GameObject Scene() // Plane Generator
    {
        GameObject planeScene = GameObject.CreatePrimitive(PrimitiveType.Plane); // Create a GameObject
        planeScene.name = "Surface";
        planeScene.transform.position = new Vector3(0f, 0f, 0f);
        planeScene.transform.localScale = new Vector3(10f, 10f, 10f);
        planeScene.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        return planeScene;
    }

    Color ZombieColor() // Color Generator
    {
        Color zombie;
        int setColor = Random.Range(0, 3); // Random Color
        switch (setColor)
        {
            case 0:
                zombie = Color.cyan;
                break;
            case 1:
                zombie = Color.magenta;
                break;
            case 2:
                zombie = Color.green;
                break;
            default:
                zombie = Color.white;
                break;
        }
        return zombie;
    }

    string CharacterNames() // Name Generator
    {
        string[] characterNames; // Store Names
        var txt = Resources.Load(namesFile, typeof(TextAsset)) as TextAsset; // Names File
        characterNames = txt.text.Split('\n'); // Load Names And Split By Spaces
        int selectedName = Random.Range(0, characterNames.Length); // Select a Name From Names File
        return characterNames[selectedName]; // Return a Name
    }

    int CharacterAge() // Age Generator
    {
        int age;
        age = Random.Range(15, 101); // Random Age
        return age;
    }

    void CoZ(int amount) // Cubes Generator
    {
        GameObject zombieManager = new GameObject(); // Manager
        zombieManager.name = "ZombieManager";
        GameObject citizenManager = new GameObject(); // Manager
        citizenManager.name = "CitizenManager";

        for (int i = 0; i < amount; i++)
        {
            int CoZ = Random.Range(0, 2); // Citizen or Zombie

            switch (CoZ)
            {
                case 0:
                    zObject = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create Cube
                    zObject.AddComponent<Zombie>().Init(zObject, ZombieColor());
                    zObject.transform.SetParent(zombieManager.transform); // Parenting to Manager
                    zObject.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Cube Random Positipon
                    coz.Add(zObject); // Fill List (Display Quantity Canvas)
                    break;
                case 1:
                    cObject = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create Cube
                    cObject.AddComponent<Citizen>().Init(cObject, CharacterNames(), CharacterAge()); // Citizen Info*
                    cObject.transform.SetParent(citizenManager.transform); // Parenting to Manager
                    cObject.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Cube Random Positipon
                    coz.Add(cObject); // Fill List (Display Quantity Canvas)
                    break;
                default:
                    print("Nothing");
                    break;
            }
        }
    }
    #endregion

    #region Main Methods

    void Awake()
    {
        if (cursorLock == true) // Hide Cursor
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Scene();

        // HERO
        hero = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Create an Object
        hero.AddComponent<Hero>().Init(hero, CharacterNames(), CharacterAge(), text[2], images[0]);
        hero.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Random Position
        // END HERO

        // CITIZEN AND ZOMBIES
        RandomMin = Random.Range(5, 16);
        MinSpawn ms = new MinSpawn(ref RandomMin);
        CoZ(Random.Range(ms.Min, ConstMax)); // Random Amount
        // END CITIZEN AND ZOMBIES

        // CANVAS
        DisplayQuantity();
        // END CANVAS
    }
    #endregion
}
