using UnityEngine;

[CreateAssetMenu(fileName = "SOFlyingStatusEffect", menuName = "Scriptable Objects/SOFlyingStatusEffect")]
public class SOFlyingStatusEffect : SOAbstractStatusEffect
{

    public override void OnStart(GameObject unit)
    {
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        unitMB.disposition = UnitDisposition.Flying;
    }

    public override void OnTurnEnd(GameObject unit)
    {

    }
    
    public override void OnTurnStart(GameObject unit)
    {
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        if (unitMB.disposition != UnitDisposition.Flying)
        {
            //myke flying move
            unitMB.disposition = UnitDisposition.Flying;
        }
        
    }

    public override void OnEnd(GameObject unit)
    {
        Transform cell = unit.transform.parent;
        Debug.Log($"OnEnd : cell = {cell}");
        // make disposition land on surface
        //ActionFunctions.UpdateSurfaceDisposition(unit, cell);
        //TODO - this should actually trigger an animation ? 

    }

    public override void OnBattleStart(GameObject unit)
    {
        
    }
}
