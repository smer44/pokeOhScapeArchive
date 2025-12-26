using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "AttackAndReduceDefActionSO", menuName = "Scriptable Objects/BattleActions/AttackAndReduceDefActionSO")]
public class AttackAndReduceDefActionSO : AbstractBattleActionSO
{
    public override void Act(BattleFieldMB battlefield)
    {
        GameObject attacker = battlefield.mems.CurrentFirstPerformer();
        GameObject target = battlefield.mems.CurrentTargets().Single();
        bool wasDamage = ActionFunctions.BasicHit(attacker, target);

    }

    public override bool IsValidFirstPerformer(BattleFieldMB field, GameObject unit)
    {
        // GameObject firstPerformer = battlefield.mems.CurrentFirstPerformer();
        return field.IsFriendlyUnit(unit);
    }


    public override bool CanFinishSelection(BattleFieldMB field)
    {
        //if one unit is already selected on the battlefield as a target
        return false;
    }


    public override bool IsValidTarget(BattleFieldMB field, GameObject unit)
    {
        //unit have to be an enemy unit
        return true;
    }

    public override bool CanAct(BattleFieldMB field)
    {
        // is true if there is at least one enemy unit
        // return field.CountEnemyTeamMembers() > 0;
        int enemyTeamMembers = QueryBFFunctions.CountEnemyTeamMembersOld(field);
        return enemyTeamMembers > 0;
        
    }
    
    public override List<GameObject>  GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        return QueryBFFunctions.GetEnemyUnitsOfNextTeam(field);
    }

}
