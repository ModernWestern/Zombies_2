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