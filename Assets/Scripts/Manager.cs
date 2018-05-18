
using System.Collections.Generic;
using UnityEngine;
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
    public bool cursorLock = false;
    public static List<GameObject> allGo = new List<GameObject>(); // Store Game Objects

    // Gizmos
    public static bool gizmoSwitch; 
    public bool displayGizmos; 
    // End Gizmos

    // Scene Characters
    GameObject hero;
    GameObject zObject;
    GameObject cObject;
    // End Scene Characters

    [Range(20,50)]
    public int heroHealth;

    // Canvas
    UIManager UImanager;
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
        foreach (GameObject gameObjecs in allGo)
        {
            if (gameObjecs.tag == "Zombie") zombieQ++;
            else if (gameObjecs.tag == "Citizen") citizenQ++;
        }
        UImanager.texts[0].text = zombieQ.ToString(); // Canvas
        UImanager.texts[1].text = citizenQ.ToString(); // Canvas
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

    string CharacterNames() // Name Generator
    {
        FirstName firstName;
        LastName lastName;
        string fullName = " ";
        firstName = (FirstName)Random.Range(0, (int)FirstName.DUMMY_ELEMENT);
        lastName = (LastName)Random.Range(0, (int)LastName.DUMMY_ELEMENT);
        fullName = firstName.ToString() + " " + lastName.ToString();
        return fullName;
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
                    zObject.AddComponent<Zombie>().Init(zObject, CharacterAge());
                    zObject.transform.SetParent(zombieManager.transform); // Parenting to Manager
                    zObject.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Cube Random Positipon
                    allGo.Add(zObject); // Fill List (Display Quantity Canvas)
                    break;
                case 1:
                    cObject = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create Cube
                    cObject.AddComponent<Citizen>().Init(cObject, CharacterNames(), CharacterAge()); // Citizen Info*
                    cObject.transform.SetParent(citizenManager.transform); // Parenting to Manager
                    cObject.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Cube Random Positipon
                    allGo.Add(cObject); // Fill List (Display Quantity Canvas)
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


        UImanager = FindObjectOfType<UIManager>(); // Call Class

        // HERO
        Hero.health = heroHealth; // Set Health (Static From Hero Class)
        UImanager.sliders[0].maxValue = heroHealth; // Set Max Health
        hero = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Create an Object
        hero.AddComponent<Hero>().Init(hero, CharacterNames(), CharacterAge());
        hero.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Random Position
        allGo.Add(hero);
        // END HERO

        // CITIZEN AND ZOMBIES
        RandomMin = Random.Range(5, 16);
        MinSpawn ms = new MinSpawn(ref RandomMin);
        CoZ(Random.Range(ms.Min, ConstMax)); // Random Amount
        //CoZ(10); // TEST
        // END CITIZEN AND ZOMBIES
    }

    void Update()
    {
        gizmoSwitch = displayGizmos; // Inspector

        // CANVAS
        DisplayQuantity();
        // END CANVAS
    }
    #endregion
}
