
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

[System.Serializable]
public class ZombieAudioSettings
{
    public AudioClip[] distant;
    public AudioClip[] mid;
    public AudioClip[] close;
}

public class Manager : MonoBehaviour
{
    public string namesFile; // Public Field To Type File Name
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

    // Canvas
    public Text[] texts;
    public Image[] images;
    public CanvasGroup[] canvasGroups;
    Color onlyAlpha;
    // End Canvas

    // Audio
    public ZombieAudioSettings zombieAudioSettings;
    // End Audio

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
        texts[0].text = zombieQ.ToString(); // Canvas
        texts[1].text = citizenQ.ToString(); // Canvas
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

        // HERO
        Hero.health = 20; // Set Health (Static From Hero Class)
        hero = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Create an Object
        hero.AddComponent<Hero>().Init(hero, CharacterNames(), CharacterAge(), texts[2], images[0], canvasGroups, 2, images[2]);
        hero.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Random Position
        allGo.Add(hero);
        // END HERO

        // CITIZEN AND ZOMBIES
        RandomMin = Random.Range(5, 16);
        MinSpawn ms = new MinSpawn(ref RandomMin);
        CoZ(Random.Range(ms.Min, ConstMax)); // Random Amount
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
