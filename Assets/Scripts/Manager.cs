
using UnityEngine;

public class Manager : MonoBehaviour
{
    public string namesFile; // Public Field To Type File Name
    public bool cursorLock = false;

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

        for(int i = 0; i < amount; i++)
        {
            int CoZ = Random.Range(0, 2); // Citizen or Zombie

            switch (CoZ)
            {
                case 0:
                    GameObject zObject = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create Cube
                    zObject.AddComponent<Zombie>().Init(zObject, ZombieColor());
                    zObject.transform.SetParent(zombieManager.transform); // Parenting to Manager
                    zObject.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Cube Random Positipon
                    break;
                case 1:
                    GameObject cObject = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create Cube
                    cObject.AddComponent<Citizen>().Init(cObject, CharacterNames(), CharacterAge()); // Citizen Info*
                    cObject.transform.SetParent(citizenManager.transform); // Parenting to Manager
                    cObject.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Cube Random Positipon
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
        //new Names().CharacterNames();

        if (cursorLock == true) // Hide Cursor
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Scene();
        CoZ(Random.Range(10, 21)); // Random Amount

        // HERO
        GameObject hero;
        hero = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Create an Object
        hero.AddComponent<Hero>().Init(hero, CharacterNames(), CharacterAge());
        hero.transform.position = new Vector3(Random.Range(-40, 40), .5f, Random.Range(-40, 40)); // Random Position
        // END HERO
    }

    #endregion
}
