using TMPro;
using UnityEngine;

public class MoveDisplayItemAD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dmgText;
    public TextMeshProUGUI impactText;

    public void Setup(string name, int dmg, string impact)
    {
        if (nameText != null)
        {
            nameText.text = name;
        }

        if (dmgText != null)
        {
            dmgText.text = $"{dmg} DMG";
        }

        if (impactText != null)
        {
            impactText.text = impact;
        }
    }

    private void OnValidate()
    {
        if (nameText == null || dmgText == null || impactText == null)
        {
            Debug.LogWarning("Some text field was not attributed.", this);
        }
    }
}

