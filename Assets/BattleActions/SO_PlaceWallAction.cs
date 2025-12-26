using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SO_PlaceWallAction", menuName = "BattleActions/SO_PlaceWallAction")]
public class SO_PlaceWallAction : AbstractBattleActionSO
{
    public GameObject wallPrefab;
    public override void Act(BattleFieldMB field)
    {
        GameObject actorGO = field.mems.CurrentFirstPerformer();

        GameObject targetCell = field.mems.CurrentTargets().Single();
        //GameObject wallPrefab = field.wallPrefab;

        UnitMB actorMB = actorGO.GetComponent<UnitMB>();


        ActionFunctions.SpawnWall(actorGO, targetCell, wallPrefab);
        //PlacingFunctions.UpdateCellPlacing(targetCell);
        IEnumerator finisher = field.OnUnitDidActionAnimate(actorGO);
        actorMB.StartCoroutine(finisher);
    }

    public override bool IsValidFirstPerformer(BattleFieldMB field, GameObject unit)
    {
        return field.IsFriendlyUnit(unit);
    }

    public override bool CanFinishSelection(BattleFieldMB field)
    {
        //we finish selection for move action, if performer and free cell is selected.
        //assert it is not greater than 0
        var selectedTargets = field.mems.CurrentTargets();
        return selectedTargets.Count == 1;
    }

    public override bool IsValidTarget(BattleFieldMB field, GameObject cell)
    {
        if (CheckFunctions.IsCell(field, cell))
        {
            return true;
        }
        return false;
    }
    public override bool CanAct(BattleFieldMB field)
    {
        return true;
    }

    public override List<GameObject> GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        return new List<GameObject>();
    }
    

}
