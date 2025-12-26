using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SO_PlaceTowerAction", menuName = "Scriptable Objects/BattleActions/SO_PlaceTowerAction")]
public class SO_PlaceTowerAction : AbstractBattleActionSO
{
    public GameObject towerPrefab;
    public override void Act(BattleFieldMB field)
    {
        GameObject unit = field.mems.CurrentFirstPerformer();
        GameObject cell = field.mems.CurrentTargets().Single();
        //GameObject towerPrefab = field.towerPrefab;
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        CellMB cellMB = cell.GetComponent<CellMB>();
        //ActionFunctions.SpawnTower(unit, targetCell, towerPrefab);
        //PlacingFunctions.UpdateCellPlacing(targetCell);


        IEnumerator onUnitDid = field.OnUnitDidActionAnimate(unit);
        if (cell == unit.transform.parent.gameObject)
        {
            // if unit stays on the same cell, unit should be moved.
            // because the cell does not have tover in it yet we call ToDispositionOnCell, pointing disposition:
            IEnumerator unitSequence = AnimationFunctions.ToDispositionOnCell(field, unit, cell, UnitDisposition.OnTower, null, true);
            unitMB.StartCoroutine(unitSequence);
        }
        else
        {
            UnitMB unitOnTargetCell = cellMB.GetUnitChildIfAny();
            if (unitOnTargetCell)
            {
                IEnumerator unitSequence = AnimationFunctions.ToDispositionOnCell(field, unitOnTargetCell.gameObject, cell, UnitDisposition.OnTower, null, true);
                unitOnTargetCell.StartCoroutine(unitSequence);
            }
        }


        IEnumerator towerSequence = AnimationFunctions.SpawnOnCellItemSequence(field, cell, towerPrefab, BattlefieldObjectType.Tower, true, onUnitDid);
        cellMB.StartCoroutine(towerSequence);
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
            return CheckFunctions.CanPlaceTower(cell);
        }
        return false;
    }

    public override bool CanAct(BattleFieldMB field)
    {
        //TODO: is true if there is free cell where tower can be placed
        return true;
    }    

    public override List<GameObject>  GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        return new List<GameObject>();
    }


}
