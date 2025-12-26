using TMPro;
using UnityEngine;

public class CreatureDetailPanel : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI roleText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI movesText;

    public void ShowCreatureDetails(UnitSO creature)
    {
        nameText.text = "Name: " + creature.name;
        typeText.text = "Type: " + creature.type;
        roleText.text = "Role: " + creature.role;

        statsText.text = $"HP: {creature.GetHP()}\nAttack: {creature.GetAttack()}\nDefense: {creature.GetDefence()}";

        string moveList = "Moves:\n";
        foreach (var move in creature.moves)
        {
            moveList += "- " + move + "\n";
        }
        movesText.text = moveList;
    }
}
