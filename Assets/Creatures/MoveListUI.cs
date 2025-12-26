using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MoveListUI : MonoBehaviour
{
    public GameObject moveItemPrefab; // Prefab: MoveItemA/D
    public Transform moveListParent; //Content inside of MovesListPanel
    public MoveDatabaseSO moveDatabase;
    public GameObject movePanel;

    public void ShowPanel(List<MoveData> moves)
    {
        movePanel.SetActive(true);
        PopulateList(moves);
    }

    public void PopulateList(List<MoveData> moves)
    {
        foreach (Transform child in moveListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var move in moves)
        {
            GameObject item = Instantiate(moveItemPrefab, moveListParent);
            item.transform.Find("MoveName").GetComponent<TextMeshProUGUI>().text = move.moveName;
            item.transform.Find("MoveDamage").GetComponent<TextMeshProUGUI>().text = move.damage.ToString();
            item.transform.Find("MoveEffect").GetComponent<TextMeshProUGUI>().text = move.impact;
        }
    }

    public void ShowADMoves()
    {
        PopulateList(moveDatabase.adMoves);
        movePanel.SetActive(true);
        Debug.Log("Tamanho da lista: " + moveDatabase.adMoves.Count);
    }

    public void ShowSupportMoves()
    {
        PopulateList(moveDatabase.supMoves);
        movePanel.SetActive(true);
        Debug.Log("Tamanho da lista: " + moveDatabase.supMoves.Count);
    }

    public void ShowConjurorMoves()
    {
        PopulateList(moveDatabase.conjurorMoves);
        movePanel.SetActive(true);
        Debug.Log("Tamanho da lista: " + moveDatabase.conjurorMoves.Count);
    }

    public void HidePanel()
    {
        movePanel.SetActive(false);
    }
}
