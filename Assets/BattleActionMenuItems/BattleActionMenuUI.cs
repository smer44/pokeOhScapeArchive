using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
using System;


public class BattleActionMenuUI : MonoBehaviour
{
    //public BattleActionMenuContainerSO rootMenu;
    public GameObject buttonPrefab; // A prefab of with a UnityEngine.UI.Button component

    public static BattleActionMenuUI Instance { get; private set; }

    //public BattleFieldMB battleField;
    public Transform buttonParent;

    public string backText = "< Back";
    public string firstPerformerName;

    public TextMeshProUGUI currentUnitName;
    //Scack where you are currently located in the menu
    //private Stack<SO_BattleMenuContainer> menuStack = new Stack<SO_BattleMenuContainer>();
    //PathTreeContainer
    private Stack<PathTreeContainer> menuStack = new Stack<PathTreeContainer>();
    private Stack<SO_BattleMenuContainer> menuStackOld = new Stack<SO_BattleMenuContainer>();

    void SetUnit(GameObject unit)
    {
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        UnitSO unitSO = unitMB.data;


    }

    void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //Debug.Log($"InitSingleton called for {this}");
        }
        else
        {
            throw new Exception("BattleFieldMB: should be a sincleton, but more that 1 instance created");
        }      


    }

    void Awake()
    {
        InitSingleton();
    }

    void Start()
    {

        //Will add buttons as child game objects of current object
        buttonParent = transform;
        //OpenMenu(rootMenu);
        
        BattleFieldMB field = BattleFieldMB.Instance;
        field.onBattleFieldStateChanged += (state) =>
        {
            BattleFieldMB field = BattleFieldMB.Instance;
            gameObject.SetActive(state < BattleFieldState.AwaitSelectionFinish && field.mems.CurrentAction() != null);
        };
    }

    // Update is called once per frame
    void Update()
    {

    }




    void ClearChildren(string tag)
    {
        foreach (Transform child in buttonParent)
        {
            if (child.CompareTag(tag))
            {
                Destroy(child.gameObject);
            }

        }
    }

    public void ResetUI()
    {
        ClearChildren("SpawnUIButton");        
    }

    public void OpenRootMenu(PathTreeContainer menu)
    {
        menuStack.Clear();
        OpenMenu(menu);
    }


    void OpenMenu(PathTreeContainer menu)
    {
        ResetUI();
        //print($"Making menu for PathTreeContainer :{menu.name} ");

        // If not at the root, add back button
        if (menuStack.Count > 0)
        {
            GameObject backButtonObj = Instantiate(buttonPrefab, buttonParent);
            SetButtonText(backButtonObj, backText);
            //print($"Created backwards button for :{backButtonObj} ");

            backButtonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                OpenMenu((PathTreeContainer)menuStack.Pop());
            });
        }

        string itemsText = string.Join(", ", menu.children.Keys);
        //print($"Start create items for :{itemsText} ");

        foreach (var kvp in menu.children)
        {
            string itemName = kvp.Key;
            PathTreeNode node = kvp.Value;

            if (node == null)
            {
                continue;
            }

            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            //print($"Created button for :{buttonObj} ");
            SetButtonText(buttonObj, node.name);

            if (node is PathTreeContainer submenu)
            {
                buttonObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    menuStack.Push(menu);
                    //print($"menuStack.Push :{menu} ");
                    OpenMenu(submenu);
                });
            }
            else if (node is PathTreeLeaf leaf)
            {
                buttonObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SelectionActionForBattleField(leaf.action);
                });
            }
        }
    } // end of OpenMenu


    public void OpenRootMenu(SO_BattleMenuContainer menu)
    {
        menuStackOld.Clear();
        OpenMenu(menu);
    }


    void OpenMenu(SO_BattleMenuContainer menu)
    {

        ResetUI();
        //Debug.Log($"Making menu for SO_BattleMenuContainer :{menu.name} ");
        // If not at the root, add back button
        if (menuStackOld.Count > 0)
        {
            GameObject backButtonObj = Instantiate(buttonPrefab, buttonParent);
            SetButtonText(backButtonObj, backText);
            //Debug.Log($"Created backwards button for :{backButtonObj} ");
            backButtonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                OpenMenu(menuStackOld.Pop());
            });

        }
        string itemsText = string.Join<SO_AbstractBattleMenuItem>(", ", menu.items);
        //Debug.Log($"Start create items for :{itemsText} ");
        foreach (var item in menu.items)
        {
            if (item == null)
            {
                continue;
            }
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            //print($"Created button for :{buttonObj} ");
            SetButtonText(buttonObj, item.name);

            if (item is SO_BattleMenuContainer submenu)
            {
                buttonObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    menuStackOld.Push(menu);
                    //print($"menuStackOld.Push :{menu} ");
                    OpenMenu(submenu);
                });


            }
            else if (item is SO_BattleMenuItem singleItem)
            {
                buttonObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SelectionActionForBattleField(singleItem.action);
                });
            }


        }
    }// end of OpenMenu

    void SelectionActionForBattleField(AbstractBattleActionSO action)
    {
        BattleFieldMB battleField = BattleFieldMB.Instance;
        // Placeholder: Integrate with battle system
        //Debug.Log($"Selecting action: {action.name}");
        battleField.SelectAction(action);
    }


    void SetButtonText(GameObject buttonObj, string text)
    {
        Transform textTransform = buttonObj.transform.Find("Text (TMP)");
        TextMeshProUGUI tmpText = textTransform.GetComponent<TextMeshProUGUI>();
        tmpText.text = text;

    }

    public void SetCurrentPerformer(GameObject performer)
    {
        gameObject.SetActive(true);
        buttonParent = transform;
        UnitMB unitMB = performer.GetComponent<UnitMB>();
        UnitSO data = unitMB.data;
        //Debug.Log($"SetCurrentPerformer: selecting :{performer} ");
   
            //Debug.Log($"SetCurrentPerformer:  using list for :{performer} ");
        data.CreatePathTree();
        OpenRootMenu(data.pathTree);


        //SO_BattleMenuContainer actionsMenu = data.actions;



    }

    public void SetCurrentUnitName(string text)
    {
        Transform unitNameTraunform = transform.Find("CurrentUnitName");
        //Debug.Log($"SetCurrentUnitName : unitNameTraunform: {unitNameTraunform}");
        TextMeshProUGUI tmpText = unitNameTraunform.GetComponent<TextMeshProUGUI>();
        //Debug.Log($"SetCurrentUnitName : tmpText: {tmpText.name}");
        //tmpText.text = text;
    }

}
