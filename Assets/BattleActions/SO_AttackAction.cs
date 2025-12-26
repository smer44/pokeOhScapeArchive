using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BasicAttackAction", menuName = "BattleActions/BasicAttackAction")]
public class SO_AttackAction : AbstractBattleActionSO
{
    public int bonusDamage;

    public override void Act(BattleFieldMB field)
    {
        GameObject attackerGO = field.mems.CurrentFirstPerformer();
        GameObject targetGO = field.mems.CurrentTargets().Single();
        //bool wasDamage = ActionFunctions.BasicHit(attacker, target);
        UnitMB attackerMB = attackerGO.GetComponent<UnitMB>();
        //UnitMB targetMB = targetGO.GetComponent<UnitMB>();

        int damage = attackerMB.data.GetAttack() + bonusDamage;
        IEnumerator attackAnimation = AnimationFunctions.AttackAnimation(targetGO, damage);

        IEnumerator attackSequence = AnimationFunctions.AttackCellSequence(field, attackerGO, targetGO, attackAnimation, this.manaCost);
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
    

    public override List<GameObject>  GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        return QueryBFFunctions.GetEnemyUnitsOfNextTeam(field);
    }
}
