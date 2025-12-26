using UnityEngine;

[CreateAssetMenu(fileName = "SOPoisonStatusEffect", menuName = "StatusEffects/SOPoisonStatusEffect")]
public class SOPoisonStatusEffect : SOAbstractStatusEffect
{

    public override void OnStart(GameObject unit)
    {    
        Debug.Log($"Poisoned unit initially : {unit}, takes {power} damage");
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        //TODO: - this has to be replaced with basic attack. 
        // If basic attack does any damage, poison is triggered.
        ActionFunctions.ChangeHp(unitMB.data, -power);    
    }

    public override void OnEnd(GameObject unit)
    {        
    }

    public override void OnTurnStart(GameObject unit)
    {
        
    }

    public override void OnBattleStart(GameObject unit)
    {
    }

    public override void OnTurnEnd(GameObject unit)
    {
        //Debug.Log($"Poisoned unit {unit}, takes {power} damage");
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        ActionFunctions.ChangeHp(unitMB.data, -power);

    }
}
