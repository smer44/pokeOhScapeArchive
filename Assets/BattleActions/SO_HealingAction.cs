using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SO_HealingAction", menuName = "BattleActions/SO_HealingAction")]
public class SO_HealingAction : AbstractBattleActionSO
{
    public int healingPower;
    public override void Act(BattleFieldMB field)
    {
        GameObject actorGO = field.mems.CurrentFirstPerformer();
        GameObject targetGO = field.mems.CurrentTargets().Single();
        //bool wasDamage = ActionFunctions.BasicHit(attacker, target);
        UnitMB actorMB = actorGO.GetComponent<UnitMB>();
        UnitMB targetMB = targetGO.GetComponent<UnitMB>();

        IEnumerator finisher = field.OnUnitDidActionAnimate(actorGO);
        //IEnumerator attackAnimation = AnimationFunctions.AttackAnimation(targetGO, damage);

        IEnumerator healingSequence = AnimationFunctions.ChangeHpSequence(actorMB, targetMB, healingPower, this.manaCost, finisher);
        actorMB.StartCoroutine(healingSequence);

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
            //heal unit what is of current team and is alive. 
            return CheckFunctions.IsUnitOfCurrentTeam(field, unit);
        }
        return false;
    }

    public override bool CanAct(BattleFieldMB field)
    {
        //TODO - should not be possible if everyone are healthy?
        return true;
    }

    public override List<GameObject>  GetPossibleTargets(BattleFieldMB field, GameObject performer)
    {
        return QueryBFFunctions.GetCurrentTeamUnits(field);
    }
}
