using UnityEngine;

public class PlacingFunctions
{
    private static float flyingHeight = 2.0f;
    private static float towerHeight = 1.0f;

    public static bool CheckFlying(GameObject unit, UnitMB unitMB)
    {
        //UnitMB unitMB = unit.GetComponent<UnitMB>();
        if (unitMB.disposition == UnitDisposition.Flying)
        {
            Transform t = unit.transform.parent;

            unit.transform.position = new Vector3(t.position.x, t.position.y + flyingHeight, t.position.z);
            return true;
        }
        return false;
    }

    public static Vector3 FlyingPositionForCell(GameObject cell)
    {
        Transform t = cell.transform;
        return new Vector3(t.position.x, t.position.y + flyingHeight, t.position.z);
    }

    public static Vector3 PositionForCell(Transform t, UnitDisposition disposition)
    {
        //Transform t = cell.transform;
        if (disposition == UnitDisposition.Flying)
        {
            return new Vector3(t.position.x, t.position.y + flyingHeight, t.position.z);
        }

        if (disposition == UnitDisposition.OnTower)
        {
            return new Vector3(t.position.x, t.position.y + towerHeight, t.position.z);
        }
        return t.position;

    }

    public static Vector3 PositionForItemSpawnStart(GameObject cell, BattlefieldObjectType spawnType)
    {
        Transform t = cell.transform;
        if (spawnType == BattlefieldObjectType.Tower)
        {
            return new Vector3(t.position.x, t.position.y - 1f, t.position.z);
        }
        return t.position;
    }

    public static Vector3 PositionForItemSpawnEnd(GameObject cell, BattlefieldObjectType spawnType)
    {
        Transform t = cell.transform;
        if (spawnType == BattlefieldObjectType.Tower)
        {
            return t.position;
        }
        return t.position;
    }


    public static bool CheckTower(GameObject unit, UnitMB unitMB)
    {
        if (unitMB.disposition == UnitDisposition.OnTower)
        {
            Transform t = unit.transform.parent;
            unit.transform.position = new Vector3(t.position.x, t.position.y + towerHeight, t.position.z);
            return true;
        }
        return false;
    }

    public static bool CheckOnSurface(GameObject unit, UnitMB unitMB)
    {
        if (unitMB.disposition == UnitDisposition.OnSurface || unitMB.disposition == UnitDisposition.OnSurfaceBehindWall)
        {
            Transform t = unit.transform.parent;
            unit.transform.position = new Vector3(t.position.x, t.position.y, t.position.z);
            return true;
        }
        return false;
    }

    public static void UpdateCellPlacing(GameObject cell)
    {
        CellMB cellMB = cell.GetComponent<CellMB>();
        UnitMB unitIfAny = cellMB.GetUnitChildIfAny();
        if (unitIfAny != null)
        {
            UpdateUnitPlacing(unitIfAny.gameObject);
        }
    }

    public static void UpdateUnitPlacing(GameObject obj)
    {
        UnitMB unitMB = obj.GetComponent<UnitMB>();
        //Flying has more priority than on tower.
        if (CheckFlying(obj, unitMB))
        {
            return;
        }
        if (CheckTower(obj, unitMB))
        {
            return;
        }
        if (CheckOnSurface(obj, unitMB))
        {
            return;
        }
        throw new System.Exception($"PlacingFunctions.UpdateUnitPlacing: unsupported unit disposition: {unitMB.name}, {unitMB.disposition}");
    }

    public static void UpdateAllUnitsPlacing(BattleFieldMB battleFieldMB)
    {
        foreach (var teamEntry in battleFieldMB.teams)
        {
            foreach (GameObject unit in teamEntry.Value)
            {
                UpdateUnitPlacing(unit);

            }

        }
    }


    public static UnitDisposition NextDisposition(Transform cell, UnitDisposition oldDisposition)
    {

        if (oldDisposition == UnitDisposition.Flying)
        {
            return UnitDisposition.Flying;
        }

        if (oldDisposition == UnitDisposition.Buried)
        {
            return UnitDisposition.Buried;
        }
        //Transform cell = unit.transform.parent;

        //Debug.Log($"Actionfunctions.UnitDisposition: cell {cell}");
        //Debug.Log($"Actionfunctions.UnitDisposition: unit {unit}");
        if (CheckFunctions.CheckHasInSameCell(cell, BattlefieldObjectType.Tower))
        {
            return UnitDisposition.OnTower;
        }
        if (CheckFunctions.CheckHasInSameCell(cell, BattlefieldObjectType.Wall))
        {
            return UnitDisposition.OnSurfaceBehindWall;
        }
        return UnitDisposition.OnSurface;
    }
    
    public static void UpdateSurfaceDisposition(GameObject unit, Transform cell)
    {
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        unitMB.disposition = NextDisposition(cell,unitMB.disposition);
    }
}
