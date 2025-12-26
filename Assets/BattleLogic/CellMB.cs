using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;


public class CellMB : MonoBehaviour
{
    public int zone;
    public int lane;
    public string teamName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public UnitMB GetUnitChildIfAny()
    {
        UnitMB[] unitMBChildren = GetComponentsInChildren<UnitMB>();
        Assert.IsTrue(unitMBChildren.Length <= 1, "Multiple children with UnitMB script detected.");
        //Assert.IsTrue(unitMBChildren.Length == 1, "No children with UnitMB script detected.");

        return unitMBChildren.Length == 0 ? null : unitMBChildren[0];
    }

    // need to do here : IsObstackle, lane, row index

    public void DeleteChildrenByType(BattlefieldObjectType type)
    {
        var childrenToRemove = new System.Collections.Generic.List<GameObject>();
        foreach (Transform child in transform)
        {
            MB_TypedBattlefieldObject typedObj = child.GetComponent<MB_TypedBattlefieldObject>();
            if (typedObj != null)
            {
                if (typedObj.battlefieldObjectType == type)
                {
                    childrenToRemove.Add(child.gameObject);
                }               
            }

        }
        foreach (GameObject child in childrenToRemove)
        {
            DestroyImmediate(child);// Use Destroy for runtime, DestroyImmediate for editor
        }
    } 



}
