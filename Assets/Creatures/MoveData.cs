using UnityEngine;

public enum MoveCategory { AD, Support, Conjuror};


[System.Serializable]
public class MoveData
{
    public string moveName;
    public int damage;
    public string impact;
    public MoveCategory category;
}

