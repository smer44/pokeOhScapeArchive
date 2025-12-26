using UnityEngine;


public abstract class SO_AbstractBattleMenuItem : ScriptableObject
{
    public string name;
    public abstract SO_AbstractBattleMenuItem DeepClone();

}


[CreateAssetMenu(fileName = "BattleMenuItem", menuName = "Scriptable Objects/BattleActionMenu/BattleMenuItem")]
public class SO_BattleMenuItem : SO_AbstractBattleMenuItem
{
    public AbstractBattleActionSO action;

    public override  SO_AbstractBattleMenuItem DeepClone()
    {
        var clone = (SO_BattleMenuItem)this.MemberwiseClone();
        clone.action = action;
        return clone;
    }

}





[CreateAssetMenu(fileName = "BattleMenuContainer", menuName = "Scriptable Objects/BattleActionMenu/BattleMenuContainer")]
public class SO_BattleMenuContainer : SO_AbstractBattleMenuItem
{
    public SO_AbstractBattleMenuItem[] items;

    public override SO_AbstractBattleMenuItem DeepClone()
    {
        var clone = (SO_BattleMenuContainer)this.MemberwiseClone();
        if (items != null)
        {
            clone.items = new SO_AbstractBattleMenuItem[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    clone.items[i] = items[i].DeepClone();

                }

            }


        }
        return clone;

    }



}
