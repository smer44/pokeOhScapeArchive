using UnityEngine;
using UnityEngine.Assertions;

public class CheckFunctions
{

    public static bool IsUnit(BattleFieldMB field, GameObject unit)
    {
        MB_TypedBattlefieldObject typeScript = unit.GetComponent<MB_TypedBattlefieldObject>();
        BattlefieldObjectType objType = typeScript.battlefieldObjectType;
        if (objType == BattlefieldObjectType.Unit)
        {
            return true;
        }
        return false;

    }

    public static bool IsCell(BattleFieldMB field, GameObject unit)
    {
        MB_TypedBattlefieldObject typeScript = unit.GetComponent<MB_TypedBattlefieldObject>();
        BattlefieldObjectType objType = typeScript.battlefieldObjectType;
        if (objType == BattlefieldObjectType.Cell)
        {
            return true;
        }
        return false;

    }

    public static bool IsObstackle(BattleFieldMB field, GameObject obj)
    {
        CellMB cellMB = obj.GetComponent<CellMB>();
        Assert.IsNotNull(cellMB, $"IsObstackle: called with obj = {obj}, not having CellMB");
        foreach (Transform child in obj.transform)
        {
            MB_TypedBattlefieldObject typedObject = child.GetComponent<MB_TypedBattlefieldObject>();
            // You have to do this check, because amoung children of the cell could be different objects,
            //some game units other visuals without TypedBattleObject
            if (typedObject != null &&
                (typedObject.battlefieldObjectType == BattlefieldObjectType.Unit ||
                 typedObject.battlefieldObjectType == BattlefieldObjectType.Thing))
            {
                return true;
            }
        }
        return false;

    }

    public static bool CanPlaceTower(GameObject cell)
    {
        CellMB cellMB = cell.GetComponent<CellMB>();
        Assert.IsNotNull(cellMB, $"IsObstackle: called with obj = {cell}, not having CellMB");
        foreach (Transform child in cell.transform)
        {
            MB_TypedBattlefieldObject typedObject = child.GetComponent<MB_TypedBattlefieldObject>();
            // You have to do this check, because amoung children of the cell could be different objects,
            //some game units other visuals without TypedBattleObject
            if (typedObject != null &&
                (typedObject.battlefieldObjectType == BattlefieldObjectType.Tower ||
                 typedObject.battlefieldObjectType == BattlefieldObjectType.Thing))
            {
                return false;
            }
        }
        return true;
    }


    public static bool HasTower(GameObject cell)
    {
        CellMB cellMB = cell.GetComponent<CellMB>();
        Assert.IsNotNull(cellMB, $"IsObstackle: called with obj = {cell}, not having CellMB");
        foreach (Transform child in cell.transform)
        {
            MB_TypedBattlefieldObject typedObject = child.GetComponent<MB_TypedBattlefieldObject>();
            // You have to do this check, because amoung children of the cell could be different objects,
            //some game units other visuals without TypedBattleObject
            if (typedObject != null &&
                (typedObject.battlefieldObjectType == BattlefieldObjectType.Tower))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsOnTower(GameObject unit)
    {
        GameObject cell = unit.transform.parent.gameObject;
        return HasTower(cell);
    }


    public static bool IsUnitOfCurrentTeam(BattleFieldMB field, GameObject obj)
    {
        UnitMB unit = obj.GetComponent<UnitMB>();
        if (field.currentTeam.Equals(unit.team))
        {
            return true;
        }
        return false;

    }

    public static bool IsUnitCanAct(BattleFieldMB field, GameObject obj)
    {
        UnitMB unit = obj.GetComponent<UnitMB>();
        return unit.CanAct && unit.IsAlive();
    }

    public static bool IsCurrentTeamCell(BattleFieldMB field, GameObject obj)
    {
        CellMB cellMB = obj.GetComponent<CellMB>();
        return cellMB.teamName == field.currentTeam;

    }

    public static bool HasMana(UnitMB unit, int mana)
    {
        return unit.data.GetMana() >= mana;
    }
    

    public static bool CheckHasInSameCell(Transform cell, BattlefieldObjectType type)
    {

        foreach (Transform childTr in cell)
        {
            MB_TypedBattlefieldObject childMB = childTr.GetComponent<MB_TypedBattlefieldObject>();
            //children inside a cell can not have MB_TypedBattlefieldObject set
            if (childMB != null && childMB.battlefieldObjectType == type)
            {
                return true;
            }
        }
        return false;
    }    


}
