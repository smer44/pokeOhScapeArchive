using UnityEngine;
using UnityEngine.UI;

public class CreatureButton : MonoBehaviour 
{
    public UnitSO creatureData;
    public CreatureDetailPanel detailPanel;
    public PlayMenuManager menuManager;

    public void OnClick()
    {
        detailPanel.ShowCreatureDetails(creatureData);
        menuManager.ShowCreatureDetails(); 
    }
}
