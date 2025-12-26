using UnityEngine;


public enum BattlefieldObjectType
{
    Unit,
    Cell,
    Thing,
    Tower,
    Wall
}

public class MB_TypedBattlefieldObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BattlefieldObjectType battlefieldObjectType;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
