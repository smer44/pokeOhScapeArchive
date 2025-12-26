using UnityEngine;

public class WallCellModificatorMB : CellModificatorMB
{
    public Direction direction;
    public GameObject wallVisualsPrefab;


    void Start()
    {

        InstantiateWallVisual();
    }

    public void InstantiateWallVisual()
    {
        Quaternion rotation = GridCalc.ToRotation(direction);
        Instantiate(wallVisualsPrefab, transform.position, rotation, transform);

    }



}



