using UnityEngine;
using System.Linq;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SO_MoveAction", menuName = "Scriptable Objects/BattleActions/SO_MoveAction")]
public class SO_MoveAction : AbstractBattleActionSO
{

    public override void Act(BattleFieldMB field)
    {
        GameObject unit = field.mems.CurrentFirstPerformer();
        GameObject cell = field.mems.CurrentTargets().Single();

        UnitMB unitMB = unit.GetComponent<UnitMB>();
        unitMB.StartCoroutine(AnimationFunctions.MoveToCellSequence(field, unit, cell));


    }




    public override bool IsValidFirstPerformer(BattleFieldMB field, GameObject unit)
    {
        return field.IsFriendlyUnit(unit);
    }

    public override bool CanFinishSelection(BattleFieldMB field)
    {
        //we finish selection for move action, if performer and free cell is selected.
        var selectedTargets = field.mems.CurrentTargets();
        return selectedTargets.Count == 1;
    }


    public override bool IsValidTarget(BattleFieldMB field, GameObject cell)
    {
        if (CheckFunctions.IsCell(field, cell))
        {
            GameObject firstPerformer = field.mems.CurrentFirstPerformer();
            return (!CheckFunctions.IsObstackle(field, cell)) && CheckFunctions.IsCurrentTeamCell(field, cell) && GridFunctions.IsNeighbourCell(firstPerformer, cell);

        }
        return false;
    }

    public override bool CanAct(BattleFieldMB field)
    {
        // is true if there is free cell around performer
        return true;
    }
    
    public override List<GameObject>  GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        //TODO - should be free cells around performer
        GameObject cell = performer.transform.parent.gameObject;
        return QueryBFFunctions.GetNeighborsCells(field, cell);
    }



}
