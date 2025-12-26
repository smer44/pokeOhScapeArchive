using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SOActionSelfBuff", menuName = "StatusEffects/SOActionSelfBuff")]
public class SOActionSelfBuff : AbstractBattleActionSO
{

    public SOAbstractStatusEffect effectBlueprint;
    public override void Act(BattleFieldMB field)
    {
        GameObject attackerGO = field.mems.CurrentFirstPerformer();
        //GameObject targetGO = field.mems.CurrentTargets().Single();
        UnitMB attackerMB = attackerGO.GetComponent<UnitMB>();
        //bool wasDamage = ActionFunctions.BasicHit(attacker, target);
        //IEnumerator attackAnimation = AnimationFunctions.InflictAnimation(targetGO, effectBlueprint);
        //AnimationFunctions.AnimateInflictTarget(battlefield, attacker, target, effectBlueprint, this.manaCost);
        //IEnumerator attackSequence = AnimationFunctions.AttackCellSequence(field, attackerGO, targetGO,attackAnimation,  this.manaCost);
        IEnumerator finisher = field.OnUnitDidActionAnimate(attackerGO);
        IEnumerator buffSequence = AnimationFunctions.SelfBuffAnimation(attackerGO, effectBlueprint, finisher);
        attackerMB.StartCoroutine(buffSequence);

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
        return selectedFirstPerformer != null && selectedTargets.Count == 0;
    }

    public override bool IsValidTarget(BattleFieldMB field, GameObject unit)
    {

        //should not be called
        return false;
    } 

    public override bool CanAct(BattleFieldMB field)
    {
        // is true if there is at least one enemy unit
        return true; //field.CountEnemyTeamMembers() > 0;
    }        

    public override List<GameObject>  GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        return QueryBFFunctions.GetPerformer(field);
    }

}
