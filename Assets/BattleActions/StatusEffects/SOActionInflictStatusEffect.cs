using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SOActionInflictStatusEffect", menuName = "StatusEffects/SOActionInflictStatusEffect")]
public class SOActionInflictStatusEffect : AbstractBattleActionSO
{

    public SOAbstractStatusEffect effectBlueprint;
    public override void Act(BattleFieldMB field)
    {
        GameObject attackerGO = field.mems.CurrentFirstPerformer();
        GameObject targetGO = field.mems.CurrentTargets().Single();
        UnitMB attackerMB = attackerGO.GetComponent<UnitMB>();
        //bool wasDamage = ActionFunctions.BasicHit(attacker, target);
        IEnumerator attackAnimation = AnimationFunctions.InflictAnimation(targetGO, effectBlueprint);
        //AnimationFunctions.AnimateInflictTarget(battlefield, attacker, target, effectBlueprint, this.manaCost);
        IEnumerator attackSequence = AnimationFunctions.AttackCellSequence(field, attackerGO, targetGO,attackAnimation,  this.manaCost);
        attackerMB.StartCoroutine(attackSequence);

    }

    public override bool IsValidFirstPerformer(BattleFieldMB field, GameObject unit)
    {
        return field.IsFriendlyUnit(unit);
    }

    public override bool CanFinishSelection(BattleFieldMB field)
    {
        GameObject selectedFirstPerformer = field.mems.CurrentFirstPerformer();
        var selectedTargets = field.mems.CurrentTargets();
        //if one unit is already selected on the battlefield as a target
        return selectedFirstPerformer != null && selectedTargets.Count == 1;
    }

    public override bool IsValidTarget(BattleFieldMB field, GameObject unit)
    {

        if (CheckFunctions.IsUnit(field, unit))
        {
            //currently, we can not attack unit what is in current team. 
            return !CheckFunctions.IsUnitOfCurrentTeam(field, unit);
        }
        return false;
    } 

    public override bool CanAct(BattleFieldMB field)
    {
        // is true if there is at least one enemy unit
        int enemyTeamMembers = QueryBFFunctions.CountEnemyTeamMembersOld(field);
        return enemyTeamMembers > 0;
    }        

    //TODO - currently, possible targets are all enemy units, what can be not always the case in inflict status effect action
    public override List<GameObject> GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        return QueryBFFunctions.GetEnemyUnitsOfNextTeam(field);
    }

}
