using UnityEngine;

public class Citizen : MonoBehaviour
{
    public CitizenProperties citizenProperties;

	public void Init(GameObject CitizenBody, string name, int age)
	{
        // Collision Message
        CitizenBody.tag = "Citizen";
        CitizenBody.AddComponent<SphereCollider>();
        SphereCollider sc = CitizenBody.GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 1f;
        // End Collison Message

        CitizenBody.transform.localScale = new Vector3 (1f, 1f, 1f); // Resize Object
		CitizenBody.GetComponent<Renderer> ().material.SetColor ("_Color", Color.yellow); // Set Object Color
		CitizenBody.name = name + " " + age; // Set Object Name

        citizenProperties.age = age;
        citizenProperties.name = name;
        citizenProperties.info = Message(name, age);
    }

    string Message(string _name, int _age) // Greetings Generator
    {
        int index = Random.Range(0, (int)Greeting.Lenght);
        citizenProperties.greeting = (Greeting)index;
        string message = citizenProperties.greeting.ToString() + "! I'm " + _name + "and I'm " + _age + "yo";
        return message;
    }
}
