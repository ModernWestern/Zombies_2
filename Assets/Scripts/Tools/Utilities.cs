public enum Behaviour { getMove, getIdle, getRotate, getReaction }
public enum Taste { Brain, Legs, Arms, Eyes, Neck, Lenght }

public struct ZombieProperties
{
    public int age;
    public Taste taste;
    public string bodyPart;
    public Behaviour behaviour;
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