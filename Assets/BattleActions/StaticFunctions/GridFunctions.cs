using UnityEngine;
using System.Collections.Generic;

public class GridFunctions
{

    /// <summary>
    /// Convert (x,y) to 1D index. Row-major: index = y * width + x.
    /// </summary>
    public static int ToIndex(BattleFieldPositioned4Unit unit, int width)
    {
        return ToIndex(unit.lane, unit.zone, width);
    } 

    /// <summary>
    /// Convert (x,y) to 1D index. Row-major: index = y * width + x.
    /// </summary>
    public static int ToIndex(int x, int y, int width)
    {
        return y * width + x;
    } 

    /// <summary>
    /// Convert 1D index to (x,y). Row-major.
    /// </summary>
    public static Vector2Int ToCoord(int index, int width)
    {
        int x = index % width;
        int y = index / width;
        return new Vector2Int(x, y);
    }



    public static bool InBounds(int x, int y, BattleFieldMB field)
    {
        var totalSize = field.grid.Length;
        var index = ToIndex(x, y, field.width);
        return 0 <= index && index < totalSize;
    }


    public static int[] nbr4x = new int[] { -1, 1, 0, 0 };
    public static int[] nbr4y = new int[] { 0, 0, -1, 1 };


    public static List<int> Neighbors4Indices(int x, int y, BattleFieldMB field)
    {
        List<int> ret = new List<int>();
        for (int i = 0; i < nbr4x.Length; i++)
        {
            int nx = x + nbr4x[i];
            int ny = y + nbr4y[i];
            if (InBounds(nx, ny, field))
            {
                ret.Add(ToIndex(nx, ny, field.width));
            }
        }
        return ret;
    }




    public static bool IsNeighbourCell(GameObject unit, GameObject otherCell)
    {
        Transform unitCellTransform = unit.transform.parent;

        CellMB c0 = unitCellTransform.GetComponent<CellMB>();
        CellMB c1 = otherCell.GetComponent<CellMB>();
        return IsNeighbourCell(c0.zone, c0.lane, c1.zone, c1.lane);

    }

    public static bool IsNeighbourCellWithDiagonals(GameObject unit, GameObject otherCell)
    {
        Transform unitCellTransform = unit.transform.parent;

        CellMB c0 = unitCellTransform.GetComponent<CellMB>();
        CellMB c1 = otherCell.GetComponent<CellMB>();
        return IsNeighbourCellWithDiagonals(c0.zone, c0.lane, c1.zone, c1.lane);

    }

    public static bool IsNeighbourCell(int x0, int y0, int x1, int y1)
    {
        int dx = Mathf.Abs(x0 - x1);
        int dy = Mathf.Abs(y0 - y1);
        //    Debug.Log($"Checking neighbour:");
        //    Debug.Log($"x0 = {x0}, y0 = {y0}");
        //    Debug.Log($"x1 = {x1}, y1 = {y1}");
        //    Debug.Log($"dx = {dx}, dy = {dy}");
        bool isNeighbour = (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
        //Debug.Log($"Result: isNeighbour = {isNeighbour}");
        return isNeighbour;
    }

    public static bool IsNeighbourCellWithDiagonals(int x0, int y0, int x1, int y1)
    {
        int dx = Mathf.Abs(x0 - x1);
        int dy = Mathf.Abs(y0 - y1);
        return (dx <= 1 && dy <= 1) && !(dx == 0 && dy == 0);
    }
    

}
