using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ActionFunctions
{

    public static bool GetHit(GameObject target, int damage)
    {
        //UnitMB attacker = attackeri.GetComponent<UnitMB>();
        //Debug.Log($"ActionFunctions.GetHit:target :{target}, damage: {damage}");
        UnitMB targetMB = target.GetComponent<UnitMB>();
        //return BasicHit(attacker.data, target.data);
        int damageAfterDefence = damage - targetMB.data.GetDefence();
        if (damageAfterDefence > 0)
        {
            return targetMB.data.ChangeValue(StatType.HP, -damageAfterDefence);
        }
        return false;
    }

    public static bool ManaDrain(UnitMB actor, UnitMB target, int damage)
    {
        int damageAfterDefence = damage - target.data.GetDefence();
        if (damageAfterDefence > 0)
        {
            actor.data.ChangeValue(StatType.Mana, damageAfterDefence);
            target.data.ChangeValue(StatType.Mana, -damageAfterDefence);
            return true;
        }
        return false;
    }

    public static bool BasicHit(GameObject attackerGO, GameObject targetGO)
    {
        UnitMB attackerMB = attackerGO.GetComponent<UnitMB>();
        UnitMB targetMB = targetGO.GetComponent<UnitMB>();
        int damage = attackerMB.data.GetAttack() - targetMB.data.GetDefence();
        if (damage > 0)
        {
            return ChangeHp(targetMB.data, -damage);
        }
        return false;
        
    }

/*    public static bool DefencelessHit(StatListSO target, int damage)
    {
        return ChangeHp(target, -damage);
    }
*/
    public static void Inflict(GameObject target, SOAbstractStatusEffect statusEffect)
    {
        UnitMB unitMB = target.GetComponent<UnitMB>();
        unitMB.AddStatusEffect(statusEffect);
        unitMB.statusEffects = unitMB.statusEffects.OrderByDescending(e => e.priority).ToList();
    }

    /*   public static void HalfDef(GameObject unit)
                   {
                       UnitMB unitMB = unit.GetComponent<UnitMB>();
                       unitMB.data.Defence /= 2;
                   }
               */

    public static bool ChangeHp(StatListSO obj, int damage)
    {
        return obj.ChangeValue(StatType.HP, damage);
    }
    

    public static void FlyOff(GameObject obj, SOFlyingStatusEffect flying)
    {
        //UnitMB unit = obj.GetComponent<UnitMB>();
        //unit.AddStatusEffect(flying);

        Inflict(obj, flying);
        PlacingFunctions.UpdateUnitPlacing(obj);
        //Debug.Log($"FlyOff: is flying now: {unit},");
    }

    public static void ToggleUnitCanAct(GameObject obj)
    {

        Debug.Log($"ToggleUnitCanAct: obj: {obj}"); 
        UnitMB unit = obj.GetComponent<UnitMB>();
        unit.CanAct = !unit.CanAct;
    }

    public static void SetUnitCanAct(GameObject obj, bool value)
    {
        UnitMB unit = obj.GetComponent<UnitMB>();
        unit.CanAct = value;
    }

    public static GameObject SpawnItem(GameObject cell, GameObject itemPrefab, Vector3 position, BattlefieldObjectType itemType, bool unique)
    {
        Debug.Log($"Actionfunctions.SpawnItem: cell {cell}");
        Debug.Log($"Actionfunctions.SpawnItem: itemPrefab {itemPrefab}");
        if (unique)
        {
            CellMB cellMB = cell.GetComponent<CellMB>();
            cellMB.DeleteChildrenByType(itemType);

        }
        //Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
        //Vector3 position = PlacingFunctions.PositionForItemSpawnStart(cell, itemType);

        GameObject spawnedItem = GameObject.Instantiate(itemPrefab, position, Quaternion.identity, cell.transform) as GameObject;
        return spawnedItem;
 

    }

    public static void SpawnWall(GameObject unit, GameObject targetCell, GameObject wallPrefab)
    {
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        if (unitMB.disposition == UnitDisposition.OnSurface)
        {
            unitMB.disposition = UnitDisposition.OnSurfaceBehindWall;
        }


        //remove any previous walls:
        CellMB cellMB = targetCell.GetComponent<CellMB>();
        cellMB.DeleteChildrenByType(BattlefieldObjectType.Wall);
        GameObject spawnedWall = GameObject.Instantiate(wallPrefab, targetCell.transform) as GameObject;
        // Reset local transform to zero
        spawnedWall.transform.localPosition = Vector3.zero;
        spawnedWall.transform.localRotation = Quaternion.identity;
        spawnedWall.transform.localScale = Vector3.one;
    }










    public static GameObject SpawnUnit(GameObject unitPrefab, GameObject cell, UnitSO unitSO)
    {
        //remove any previous walls:
        CellMB cellMB = cell.GetComponent<CellMB>();


        GameObject newUnitMB = GameObject.Instantiate(unitPrefab, cell.transform) as GameObject;
        UnitMB unitMB = newUnitMB.GetComponent<UnitMB>();
        unitMB.SetData(unitSO);

        //cellMB.DeleteChildrenByType(BattlefieldObjectType.Wall);
        //GameObject spawnedWall = GameObject.Instantiate(wallPrefab, targetCell.transform) as GameObject;
        // Reset local transform to zero
        newUnitMB.transform.localPosition = Vector3.zero;
        newUnitMB.transform.localRotation = Quaternion.identity;
        newUnitMB.transform.localScale = Vector3.one;
        return newUnitMB;
    }

    public static void ApplyStatusEffectsAfterTurn(GameObject unit)
    {
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        List<SOAbstractStatusEffect> toRemove = new List<SOAbstractStatusEffect>();
        foreach (SOAbstractStatusEffect effect in unitMB.statusEffects)
        {
            effect.OnTurnEnd(unit);
            effect.TickOnTurnEnd();
            if (effect.Ends())
            {
                toRemove.Add(effect);
            }
        }
        //remove all sstatus effects what have ended
        unitMB.statusEffects = unitMB.statusEffects.Except(toRemove).ToList();
    }

    public static void ConsumeMana(UnitMB unitMB, int manaCost)
    {
        //int newMana = unitMB.data.GetMana() - manaCost;
        //unitMB.data.SetValue(StatType.Mana, newMana);
        unitMB.data.ChangeValue(StatType.Mana,-manaCost);
    }
}
