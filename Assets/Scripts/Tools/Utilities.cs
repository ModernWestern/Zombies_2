public enum Behaviour { goForward, goBackward, getIdle, getRotate, getReaction }
public enum Taste { Brain, Legs, Arms, Eyes, Neck, Lenght }
public enum Greeting { Hej, Hi, Sup, Hey, Emm, Dude, Mate, Easy, Lenght }
public enum SFX { DistantRoar, MidRoar, CloseRoar}

public struct ZombieProperties
{
    public int age;
    public string name;
    public Taste taste;
    public string bodyPart;
    public Behaviour behaviour;
}

public struct CitizenProperties
{
    public int age;
    public string name;
    public Greeting greeting;
    public string info;
    public Behaviour behaviour;
}

public struct ClipBox
{
    public UnityEngine.AudioClip[] distant;
    public UnityEngine.AudioClip[] mid;
    public UnityEngine.AudioClip[] close;
}