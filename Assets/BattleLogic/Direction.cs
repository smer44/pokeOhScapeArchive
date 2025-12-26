using UnityEngine;
public enum Direction
{
    Forward,
    Backward,
    Left,
    Right

}

public class GridCalc
{

    private static readonly Quaternion[] directionRotations = new Quaternion[]
        {
            Quaternion.Euler(0, 0, 0),    // Forward
            Quaternion.Euler(0, 180, 0),  // Backward
            Quaternion.Euler(0, 270, 0),  // Left
            Quaternion.Euler(0, 90, 0)    // Right
        };

    public static Quaternion ToRotation(Direction direction)
    {
        return directionRotations[(int)direction];
    }


}





