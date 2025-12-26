using UnityEngine;
using System.Linq;

public class DummyAI
{

    public GameObject PickRandomCurrentTeamUnit(bool onlyUnitsThatCanAct = true)
    {
        var bf = BattleFieldMB.Instance;
        var currentTeamUnits = bf.teams[bf.currentTeam];
        if (onlyUnitsThatCanAct)
        {
            currentTeamUnits = currentTeamUnits.Where(u => CheckFunctions.IsUnitCanAct(bf, u)).ToList();
        }
        if (currentTeamUnits.Count == 0)
        {
            //Debug.LogWarning($"DummyAI.PickRandomCurrentTeamUnit: No units can act in team {bf.currentTeam}");
            return null;
        }
        int idx = Random.Range(0, currentTeamUnits.Count);
        return currentTeamUnits[idx];
    }

    public GameObject PickRandomEnemyUnit(bool onlyUnitsThatCanAct = true)
    {
        var bf = BattleFieldMB.Instance;
        var enemyTeamName = bf.teamNames[(bf.teamNamePos + 1) % bf.teamNames.Length];
        var enemyUnits = bf.teams[enemyTeamName];
        if (onlyUnitsThatCanAct)
        {
            enemyUnits = enemyUnits
            .Where(u => CheckFunctions.IsUnitCanAct(bf, u))
            .ToList();
        }
        int idx = Random.Range(0, enemyUnits.Count);
        return enemyUnits[idx];
    }


    public void DoRandomAITurn()
    {
        Debug.Log("DummyAI.DoRandomAITurn");
        var loop = true;
        while (loop)
        {
            loop = DoRandomUnitMoveOnce();
        }
    }

    public bool DoRandomUnitMoveOnce()
    {
        var bf = BattleFieldMB.Instance;
        var performer = PickRandomCurrentTeamUnit();
        if (performer == null)
        {
            //Debug.LogWarning($"DummyAI.DoRandomAITurn: No available performer in team {bf.currentTeam}");
            //bf.EndTurn();
            return false;
        }
        bf.SelectFirstPerformer(performer);

        //var action = bf.GetAllActionsOfUnit(performer).OrderBy(a => a.actionPriority).First();
        UnitMB unitMB = performer.GetComponent<UnitMB>();
        //select random action from available actions of the unit:
        AbstractBattleActionSO[] moves = unitMB.data.moves;
        int idx = Random.Range(0, moves.Length);
        AbstractBattleActionSO action = moves[idx];


        bf.SelectAction(action);

        var targets = action.GetPossibleTargets(bf, performer);
        idx = Random.Range(0, targets.Count);
        var target = targets[idx];
        bf.SelectTarget(target);
        bf.PerformSelectedAction();

        return true;
    }



}
