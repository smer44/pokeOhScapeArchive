using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SO_FlyOffAction", menuName = "Scriptable Objects/BattleActions/SO_FlyOffAction")]
public class SO_FlyOffAction : AbstractBattleActionSO
{

    public SOFlyingStatusEffect effectBlueprint;
    public override void Act(BattleFieldMB field)
    {
        GameObject unit = field.mems.CurrentFirstPerformer();
        GameObject cell = unit.transform.parent.gameObject;
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        //ActionFunctions.FlyOff(unit, effectBlueprint);
        //Do not forget to do on unit did action at the end of action.
        //field.OnUnitDidAction(unit);
        IEnumerator flySequence = AnimationFunctions.FlyUpOnCell(field, unit, cell, effectBlueprint);
        unitMB.StartCoroutine(flySequence);
    }

    public override bool IsValidFirstPerformer(BattleFieldMB field, GameObject unit)
    {
        return field.IsFriendlyUnit(unit);
    }


    public override bool CanFinishSelection(BattleFieldMB field)
    {
        //we finish selection for move action, if performer and free cell is selected.
        //assert it is not greater than 0
        return field.mems.CurrentTargets().Count == 0;
        //return field.selectedTargets.Count == 0;
    }

    public override bool IsValidTarget(BattleFieldMB field, GameObject cell)
    {
        //this should not be called 
        return false;
    }

    public override bool CanAct(BattleFieldMB field)
    {
        // is true if unit is not entangled?
        return true;
    }

    public override List<GameObject>  GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        return new List<GameObject>();
    }


}
