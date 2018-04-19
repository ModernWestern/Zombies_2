//using System.IO;

//public class Names
//{
//    public void CharacterNames() // Name Generator
//    {
//        string[] characterNames; // Store Names
//        var txt = Resources.Load("Names", typeof(TextAsset)) as TextAsset; // Names File
//        characterNames = txt.text.Split('\n'); // Load Names And Split By Spaces

//        string path = "Assets/Scripts/EnumNames.cs";

//        using (StreamWriter stw = new StreamWriter(path))
//        {
//            stw.WriteLine("public enum CharacterNames");
//            stw.WriteLine("{");
//            for (int i = 0; i < characterNames.Length; i++)
//            {
//                stw.WriteLine("\t" + characterNames[i] + ",");
//            }
//            stw.WriteLine("Lenght");
//            stw.WriteLine("}");
//        }
//    }
//}

public enum Behaviour { getMove, getIdle, getRotate, getRun }
public enum Taste { Brain, Legs, Arms, Eyes, Neck, Lenght }

public struct ZombieProperties
{
    public Behaviour behaviour;
    public Taste taste;
    public string bodyPart;
}

public enum Greeting { Hej, Hi, Sup, Hey, Emm, Dude, Mate, Easy, Lenght }

public struct CitizenProperties
{
    public int age;
    public string name;
    public Greeting greeting;
    public string info;
    public Behaviour behaviour;
}