using UnityEngine;
using System.Collections;

public class AnimationFunctions
{




    public static IEnumerator MoveToCellSequence(BattleFieldMB field, GameObject unit, GameObject cell)
    {
        float stepSize = 0.08f;
        float errorMargin = stepSize * 0.01f;

        UnitMB unitMB = unit.GetComponent<UnitMB>();
        unitMB.disposition = PlacingFunctions.NextDisposition(cell.transform, unitMB.disposition);
        Vector3 endPosition = PlacingFunctions.PositionForCell(cell.transform, unitMB.disposition);
        Debug.Log($"AnimationFunctions.MoveToCellSequence: endPosition {endPosition}, moving unit {unit}, new disp : {unitMB.disposition}");
        
        yield return MoveToTarget(field, unit.transform, endPosition, stepSize, errorMargin);
        //yield return FinishAnimationForBattleField(field, BattleFieldState.SelectingFirstPerformer);
        yield return ReassignParent(unit, cell);
        yield return field.OnUnitDidActionAnimate(unit);

    }

    public static IEnumerator SpawnOnCellItemSequence(BattleFieldMB field,
                                                    //GameObject actor,
                                                    GameObject cell,
                                                    GameObject prefab,
                                                    BattlefieldObjectType spawnType,
                                                    bool unique,
                                                    IEnumerator finisher)
    {
        float stepSize = 0.08f;
        float errorMargin = stepSize * 0.01f;
        //Vector3 unitStartPosition = actor.transform.position;
        //Vector3 unitEndPosition = PlacingFunctions.PositionForCell(cell, disposition);
        Vector3 spawnStartPosition = PlacingFunctions.PositionForItemSpawnStart(cell, spawnType);
        Vector3 spawnEndPosition = PlacingFunctions.PositionForItemSpawnEnd(cell, spawnType);
        GameObjectWrapper spawnedItemWrapper = new GameObjectWrapper();
        //public static IEnumerator SpawnItemOnCellAnimated(GameObject cell, GameObject prefab, out GameObject spawnedItem, BattlefieldObjectType itemType, bool unique)

        yield return SpawnItemOnCellAnimated(cell, prefab, spawnedItemWrapper, spawnStartPosition, spawnType, unique);
        // MoveToTarget(field, unit.transform, start, destination, stepSize, errorMargin);
        //check if spawnedItemWrapper.value is zero
        yield return MoveToTarget(field, spawnedItemWrapper.value.transform, spawnEndPosition, stepSize, errorMargin);
        if (finisher != null)
        {
            yield return finisher;
        }


    }


    public static IEnumerator FlyUpOnCell(BattleFieldMB field, GameObject unit, GameObject cell, SOFlyingStatusEffect flying)
    {
        float stepSize = 0.08f;
        float errorMargin = stepSize * 0.01f;
        //Vector3 start = unit.transform.position;
        Vector3 destination = PlacingFunctions.FlyingPositionForCell(cell);
        yield return InflictAnimation(unit, flying);
        yield return MoveToTarget(field, unit.transform, destination, stepSize, errorMargin);
        yield return field.OnUnitDidActionAnimate(unit);
    }

    public static IEnumerator ToSurfaceOnCell(BattleFieldMB field, GameObject unit, GameObject cell)
    {
        float stepSize = 0.08f;
        float errorMargin = stepSize * 0.01f;
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        Vector3 destination = PlacingFunctions.PositionForCell(cell.transform, unitMB.disposition);
        //yield return InflictAnimation(unit, flying);
        //yield return UpdateSurfaceDispositionAnimated(cell);
        yield return MoveToTarget(field, unit.transform, destination, stepSize, errorMargin);
        yield return field.OnUnitDidActionAnimate(unit);
    }

    public static IEnumerator ToDispositionOnCell(BattleFieldMB field,
                                                    GameObject unit,
                                                    GameObject cell,
                                                    UnitDisposition d,
                                                    IEnumerator finisher, 
                                                    bool setDisposition)
    {
        float stepSize = 0.08f;
        float errorMargin = stepSize * 0.01f;

        //check what disposition it will be and if it is changed.
        Vector3 destination = PlacingFunctions.PositionForCell(cell.transform, d);
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        //yield return InflictAnimation(unit, flying);
        //yield return UpdateSurfaceDispositionAnimated(cell);
        yield return MoveToTarget(field, unit.transform, destination, stepSize, errorMargin);
        if (setDisposition)
        {
            yield return SetDispositionAnimated(unitMB, d);
        }
        yield return finisher;
        //yield return field.OnUnitDidActionAnimate(unit);
    }


    public static IEnumerator SpawnItemOnCellAnimated(GameObject cell,
                                                        GameObject prefab,
                                                        GameObjectWrapper spawnedItemWrapper,
                                                        Vector3 position,
                                                        BattlefieldObjectType itemType,
                                                        bool unique)
    {
        Debug.Log($"SpawnItemOnCellAnimated: cell = {cell}");
        spawnedItemWrapper.value = ActionFunctions.SpawnItem(cell, prefab, position, itemType, unique);
        yield return null;
    }

    public static IEnumerator AttackCellSequence(BattleFieldMB field,
                                                GameObject movingObject,
                                                GameObject destinationObject,
                                                IEnumerator attackAnimation,
                                                int manaCost)
    {
        float stepSize = 0.2f;
        float errorMargin = stepSize * 0.01f;

        Vector3 startPosition = movingObject.transform.position;
        Vector3 destinationPosition = destinationObject.transform.position;
        //Debug.Log($"AttackCellSequence: damage {damage}");
        UnitMB actorMB = movingObject.GetComponent<UnitMB>();
        yield return AnimateSpendMana(actorMB, manaCost);
        yield return MoveToTarget(field, movingObject.transform, destinationPosition, stepSize, errorMargin);
        yield return attackAnimation;
        yield return MoveToTarget(field, movingObject.transform, startPosition, stepSize, errorMargin);
        //yield return FinishAnimationForBattleField(field, BattleFieldState.SelectingFirstPerformer);
        yield return field.OnUnitDidActionAnimate(movingObject);
        //yield return FinishMovement(movingObject, destinationObject);

    }

    public static IEnumerator AttackAnimation(GameObject target, int damage)
    {
        //Debug.Log($"AttackAnimation:target :{target}, damage: {damage}");
        ActionFunctions.GetHit(target, damage);
        yield return null;
    }

    public static IEnumerator ChangeHpAnimated(UnitMB unit, int amount)
    {
        //unit.data.ChangeValue(StatType.HP, amount);
        ActionFunctions.ChangeHp(unit.data, amount);
        yield return null;
    }

    public static IEnumerator AnimateSpendMana(UnitMB unitMB, int manaCost)
    {

        ActionFunctions.ConsumeMana(unitMB, manaCost);
        yield return null;
    }

    public static IEnumerator AnimateManaDrain(UnitMB actor, UnitMB target, int damage)
    {

        ActionFunctions.ManaDrain(actor, target, damage);
        yield return null;
    }

    public static IEnumerator InflictAnimation(GameObject target,
                                                SOAbstractStatusEffect statusEffect)
    {
        ActionFunctions.Inflict(target, statusEffect);
        yield return null;
    }

    public static IEnumerator SelfBuffAnimation(GameObject target,
                                                SOAbstractStatusEffect statusEffect,
                                                IEnumerator finisher)
    {
        yield return InflictAnimation(target, statusEffect);
        yield return finisher;
    }

    public static IEnumerator ChangeHpAnimation(UnitMB unit, int difference)
    {
        ActionFunctions.ChangeHp(unit.data, difference);
        yield return null;
    }

    public static IEnumerator ChangeHpSequence(UnitMB actor,
                                                UnitMB target,
                                                int healingPower,
                                                int manaCost,
                                                IEnumerator finisher)
    {
        yield return AnimateSpendMana(actor, manaCost);
        yield return ChangeHpAnimation(target, healingPower);
        yield return finisher;
    }

    public static IEnumerator NextDispositionAnimated(UnitMB unit, GameObject cell)
    {
        UnitDisposition newDisposition = PlacingFunctions.NextDisposition(cell.transform, unit.disposition);
        unit.disposition = newDisposition;
        yield return null;
    }

    public static IEnumerator SetDispositionAnimated(UnitMB unit, UnitDisposition d)
    {
        unit.disposition = d;
        yield return null;
    }

    public static IEnumerator MoveToTarget(BattleFieldMB field, Transform movingTransform, Vector3 destinationPosition, float stepSize, float errorMargin)
    {
        //Transform movingTransform = movingObject.transform;
        //Transform destinationTransform = destinationObject.transform;
        Debug.Log($"MoveToTarget: movingTransform = |{movingTransform}|");
        bool loop = true;
        do
        {
            Vector3 currentPosition = movingTransform.position;
            Vector3 direction = destinationPosition - currentPosition;
            float distance = direction.magnitude;
            loop = distance > errorMargin;
            float step = Mathf.Min(stepSize, distance);
            Vector3 stepVector = direction.normalized * step;
            movingTransform.position += stepVector;

            yield return new WaitForSeconds(0.025f);
        }
        while (loop);

        //after animation end, we select first performer

    }



    /*    public static IEnumerator FinishMovement(GameObject unit, GameObject targetCell)
        {
            ActionFunctions.BasicMove(unit, targetCell);
            yield return null;
        }
    */

    public static IEnumerator ReassignParent(GameObject unit, GameObject parent)
    {
        unit.transform.SetParent(parent.transform);
        yield return null;
    }

}
