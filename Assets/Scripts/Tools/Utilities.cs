public enum FirstName
{
    Michale,
    Roderick,
    Emile,
    Ernesto,
    Logan,
    Miquel,
    Shirley,
    Delmer,
    Johnathan,
    Herman,
    Trey,
    Christopher,
    Bobbie,
    Ken,
    Boyd,
    Dante,
    Rogelio,
    Lucas,
    Morris,
    Fermin,
    Dennis,
    Shelby,
    Jonathan,
    Byron,
    Cyril,
    Ellis,
    Amado,
    Graham,
    Gerardo,
    Isiah,
    Heather,
    Valeri,
    Elena,
    Carmela,
    Lorine,
    Dorthey,
    Carolyne,
    Leana,
    Armandina,
    Trula,
    Marissa,
    Willetta,
    Mildred,
    Nola,
    Pasty,
    Nichol,
    Emmy,
    Rosanne,
    Basilia,
    Awilda,
    Gregoria,
    Greta,
    Nicola,
    Lakia,
    Diane,
    Marivel,
    Torie,
    Dorie,
    Tomiko,
    Sonya,
    DUMMY_ELEMENT
}

public enum LastName
{
    Snelgrove,
    Munden,
    Davison,
    Boyette,
    Osorio,
    Flett,
    Churchill,
    Reindl,
    Brightwell,
    Mask,
    Ashby,
    Halsell,
    Ellers,
    Hafner,
    Obermiller,
    Lerner,
    Sabatino,
    Redding,
    Darner,
    Leavens,
    Kostka,
    Rappa,
    Ospina,
    Watrous,
    Flournoy,
    Buchler,
    Sullins,
    Ketelsen,
    Gasca,
    Pennington,
    Bott,
    Passarelli,
    Bosch,
    Cousin,
    Augustyn,
    Galliher,
    Marcello,
    Goin,
    Moxley,
    Brainard,
    Crumley,
    Roosevelt,
    Couture,
    Russum,
    Bailes,
    Vasser,
    Chau,
    Camille,
    Beeson,
    Straw,
    Kresge,
    Boehmer,
    Greenland,
    Capp,
    Gallagher,
    Meder,
    Urbina,
    Bracey,
    Marchese,
    Francis,
    DUMMY_ELEMENT
}

public enum Behaviour
{
    goForward,
    goBackward,
    getIdle,
    getRotate,
    getReaction
}

public enum Taste
{
    Brain,
    Legs,
    Arms,
    Eyes,
    Neck,
    DUMMY_ELEMENT
}

public enum Greeting
{
    Hej,
    Hi,
    Sup,
    Hey,
    Emm,
    Dude,
    Mate,
    Easy,
    DUMMY_ELEMENT

}

public enum SFX
{
    DistantRoar,
    MidRoar,
    CloseRoar
}

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