using UnityEngine;
using System.Collections.Generic;

public enum CreatureRole
{
    AttackDefense,
    Support,
    Conjuror

}

public enum CreatureType
{
    Water,
    Fire,
    Grass
}

public enum CreatureMove
{
    BasicAttack,
    Laser,
    CloseCombat,
    Buff,
    Decoy,
    Tower,
    SoloHeal,
    SmokeScreen,
    Move,
    Direct,
    DualSmack,
    Barrier,
    Tunnel,
    Breakdown,
    ZoneHeal,
    Focus,
    Flood,
    Cloud,
    Aerial,
    Poison,






}


[CreateAssetMenu(fileName = "UnitSO", menuName = "UnitData/UnitSO")]
public class UnitSO : StatListSO
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Basic info")]
    public string name;
    public CreatureType type;
    public CreatureRole role;



    [Header("Moveset")]
    //public SO_BattleMenuContainer actions;
    public AbstractBattleActionSO[] moves;

    public PathTreeContainer pathTree;


    public override string ToString()
    {
        return base.ToString() + "<" + name + ">";
    }




    public UnitSO Clone()
    {
        UnitSO clone = (UnitSO)base.Clone();
        clone.name = name + "_cloned";
        clone.type = type;
        clone.role = role;
        
        clone.moves = (AbstractBattleActionSO[])this.moves.Clone();

        return clone;
    }

    public void CreatePathTree()
    {
        pathTree = new PathTreeContainer { name = $"{name}_root", children = new Dictionary<string, PathTreeNode>() };
        foreach (var path in moves)
        {
            //path.AddBattleMenuPath(pathTree);
            MenuPathFunctions.AddBattleMenuPath(path, pathTree);
        }

    }

    public int GetHP()
    {
        return GetValue(StatType.HP);
    }

    public int GetMana()
    {
        return GetValue(StatType.Mana);
    }

    public int GetAttack()
    {
        return GetValue(StatType.Attack);
    }

    public int GetDefence()
    {
        return GetValue(StatType.PhysicalDefence);
    }
 
}
