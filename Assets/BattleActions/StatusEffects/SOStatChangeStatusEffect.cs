using UnityEngine;

[CreateAssetMenu(fileName = "SOStatChangeStatusEffect", menuName = "StatusEffects/SOStatChangeStatusEffect")]

public class SOStatChangeStatusEffect : SOAbstractStatusEffect
{
    public StatChangeEntry[] statChanges;

    public void ChangeAll(GameObject unit)
    {
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        UnitSO data = unitMB.data;
        foreach (StatChangeEntry change in statChanges)
        {
            change.UpdateStat(data);
        }        
    }

    public override void OnStart(GameObject unit)
    {
        ChangeAll(unit);
    }
    public override void OnTurnStart(GameObject unit)
    {
        ChangeAll(unit);
    }

    public override void OnEnd(GameObject unit)
    {
    }
    
    public override void OnBattleStart(GameObject unit)
    {
    }
    public override void OnTurnEnd(GameObject unit)
    {
    }
}
