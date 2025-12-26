using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.SceneManagement;


public class BattleFieldMB : MonoBehaviour
{

    public static BattleFieldMB Instance { get; private set; }
    public string name;
    [SerializeField]
    public GameObject[] grid;
    public GameObject cellPrefab;
    //public GameObject wallPrefab;
    public GameObject trapPrefab;

    public GameObject unitPrefab;

    //public GameObject towerPrefab;
    public string exitSceneName;
    //public BattleActionMenuUI battleActionMenu;

    public BattleFieldConfig loadConfig;

    public GameObject wonMessage;
    public GameObject loseMessage;
    
    public Dictionary<string, List<GameObject>> teams;

    public Dictionary<string,bool> aiTeams;
    public bool generateField = true;

    public string currentTeam;
    public string[] teamNames;
    public string [] aiTeamNames;
    public int teamNamePos = 0;
    //other teams are concidered to be enemy teams.
    //public string enemyTeam;
    //public BattleFieldState fieldState;

    //public GameObject selectedFirstPerformer;
    //public HashSet<GameObject> selectedTargets;
    //public AbstractBattleActionSO selectedAction;

    public BattleFieldMemoryStack mems;

    public int width;
    public int height;
    private int[] neighboursOffsetX = new int[] { -1, 1, 0, 0 };
    private int[] neighboursOffsetY = new int[] { 0, 0, -1, 1 };


    private int[] neighboursOffsetWithDiagonalX = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
    private int[] neighboursOffsetWithDiagonalY = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
    public Action<BattleFieldState> onBattleFieldStateChanged;

    //private GameObject battleActionMenuGO;

    private DummyAI ai = new DummyAI();
    private void UpdateCurrentStateText()
    {
        BattleActionMenuUI battleActionMenu = BattleActionMenuUI.Instance;
        battleActionMenu.SetCurrentUnitName(mems.CurrentFieldState().ToString());
    }

    GameObject GetCell(int x, int y)
    {

        return grid[ToOneDimIndex(x, y)];
    }

    GameObject GetCell(Vector2Int xy)
    {

        return grid[ToOneDimIndex(xy.x, xy.y)];
    }

    int ToOneDimIndex(int x, int y)
    {
        return y * width + x;
    }

    public Vector2Int ToTwoDimIndex(int index)
    {
        int y = index / width;
        int x = index % width;
        return new Vector2Int(x, y);
    }


    private void InitializeGridCellsIfEmpty()
    {
        for (int n = 0; n < grid.Length; n++)
        {

            Vector2Int xy = ToTwoDimIndex(n);
            GameObject cell = GetCell(xy);

            Vector3 pos = new Vector3(xy.x, 0, xy.y);

            GameObject currentCellPrefab = grid[n] == null ? cellPrefab : grid[n];
            GameObject instance = Instantiate(currentCellPrefab, pos, Quaternion.identity);
            instance.transform.SetParent(this.transform);
            grid[n] = instance;

        }
    }

    void Awake()
    {
        UpdateGridHeight();
        InitSingleton();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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


    void Start()
    {


        if (generateField)
        {
            InitializeGridCellsIfEmpty();
        }

        //InitHashSets();
        mems = new BattleFieldMemoryStack();
        mems.onBattleFieldStateChanged += onBattleFieldStateChanged;
        BattleActionMenuUI battleActionMenu = BattleActionMenuUI.Instance;
        //battleActionMenuGO = battleActionMenu.gameObject;

        //Debug.Log($"battleActionMenuGO: {battleActionMenuGO}");
        //battleActionMenuGO.SetActive(true);
        // If teams are not known, we update teams from field content.

        //PrintTeams(teams);

        //InitAllUnitCanActs();
        //PlacingFunctions.UpdateAllUnitsPlacing(this);
        //ClearField();
        //FlyOffAllDebug(teams);
        //ResetTeamsAndUnits();
        ResetScene();
    }

    public void ResetTeamsAndUnits()
    {
        teams = UpdateTeams();
        aiTeams = aiTeamNames.ToDictionary(s => s, _ => true);
        //Debug.Log($"BattleFieldMB.ResetTeamsAndUnits: aiTeams: {string.Join(", ", aiTeams.Keys)}");
        teamNames = teams.Keys.ToArray();
        InitAllUnitCanActs();
        PlacingFunctions.UpdateAllUnitsPlacing(this);
    }

    public void UpdateGridHeight()
    {
        height = grid.Length / width + (grid.Length % width > 0 ? 1 : 0);
    }



    // Update is called once per frame
    void Update()
    {
        UpdateResetButton();
        UpdateLeftMouseButton();
        UpdateRightMouseButton();
        UpdateUndo();
        UpdateCurrentStateText();

    }


    public List<GameObject> GetNeighboursCells(int x, int y, bool includeDiagonal)
    {
        int[] dx = includeDiagonal ? neighboursOffsetWithDiagonalX : neighboursOffsetX;
        int[] dy = includeDiagonal ? neighboursOffsetWithDiagonalY : neighboursOffsetY;
        List<GameObject> neighbours = new List<GameObject>();

        for (int j = 0; j < dy.Length; j++)
        {
            int ny = y + dy[j];

            if (0 <= ny && ny < height)
            {
                for (int i = 0; i < dx.Length; i++)
                {
                    int nx = x + dx[i];
                    if (0 <= nx && nx < width)
                    {
                        neighbours.Add(GetCell(nx, ny));
                    }

                }

            }

        }
        return neighbours;
    }


    /// <summary>
    /// Checks each update, if left mouse is clicked, 
    /// and then uses raycasting from camera 
    /// to determine GameObject clickedObject it is clicked.
    /// Then we perform action on unit click
    /// for the reference object what clickedObject references to.
    /// It is done because not always the GameObject logicks will correspond currently clicked visuals.
    /// </summary>
    private void UpdateLeftMouseButton()
    {
        if (Input.GetMouseButtonDown(0))
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // We do not concider unit clicked to be direct GameObject what is clicked,
                // but the GameObject what is referenced 
                // or in alternative design, the reference could point not to a game object, but to a script smth like "clickable"
                GameObject clickedObject = hit.collider.gameObject;
                // get object referenced by clicked object:

                ReferenceOnClick refe = clickedObject.GetComponent<ReferenceOnClick>();
                Debug.Assert(refe != null, $"UpdateLeftMouseButton: ReferenceOnClick is null for clicked game object {clickedObject} ");
                GameObject refObject = refe.referencedObject;
                OnUnitClick(refObject);

                //}

                //CameraManager.Instance?.CameraMove(clickedObject.transform.position);



            }
        }

    }

    private void UpdateRightMouseButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                ReferenceOnClick refe = clickedObject.GetComponent<ReferenceOnClick>();
                //Debug.Assert(refe != null, $"UpdateRightMouseButton: ReferenceOnClick is null for clicked game object {clickedObject} ");
                if (refe != null)
                {
                    GameObject refObject = refe.referencedObject;
                    OnObjectRightClick(refObject);
                    return;                    
                }


            }

            OnNoneRightClick();


        }
    }

    private void OnObjectRightClick(GameObject obj)
    {
        UnitInfoDisplay.Instance.SetUnitUpdateInfo(obj);
    }

    private void OnNoneRightClick()
    {
        UnitInfoDisplay.Instance.SetNoneUpdateInfo();
    }

    private void OnUnitClick(GameObject clickedObject)
    {
        BattleFieldState fieldState = mems.CurrentFieldState();
        //Debug.Log($"OnUnitClick:  clicked on : {clickedObject.name}, fieldState : {fieldState}");
        // get object referenced by clicked object:
        switch (fieldState)
        {
            case BattleFieldState.SelectingFirstPerformer:
                if (OnFirstPerformerClick(clickedObject))
                {
                    //BattleFieldState newState = ToNextSelectionState();
                }
                break;

            //  case BattleFieldState.SelectingOtherPerformers:
            //      OnOthersPerformerClick(clickedObject);
            //      break;

            case BattleFieldState.SelectingTargets:
                if (OnTargetClick(clickedObject))
                {
                    //ToNextSelectionState();
                }
                break;

            case BattleFieldState.SelectingAction:
                //do nothing if currently i select an action
                break;


        }
        //ToNextSelectionState();


    }

    private bool OnFirstPerformerClick(GameObject clickedObject)
    {

        if (IsValidPerformer(clickedObject))
        {
            SelectFirstPerformer(clickedObject);
            return true;

        }
        return false;

    }

    public void SelectFirstPerformer(GameObject performer)
    {
    
        mems.MemorizeFirstPerformerWithFieldState(performer, BattleFieldState.SelectingAction);
        BattleActionMenuUI battleActionMenu = BattleActionMenuUI.Instance;
        battleActionMenu.SetCurrentPerformer(performer);       

    }


    public void SelectAction(AbstractBattleActionSO action)
    {

        BattleFieldState state = this.mems.CurrentFieldState();
        BattleFieldState nextState = this.ToNextSelectionState(state, action);
        this.mems.MemorizeActionWithFieldState(action, nextState);

    }



    private void OnOthersPerformerClick(GameObject clickedObject)
    {
        // check if can auto-select other performers.
        if (IsValidPerformer(clickedObject))
        {
            // selectedPerformers.Add(clickedObject);
        }


    }

    /// <summary>
    /// Validating a performer:
    /// performer have to be in current team.
    /// this method does not make check by amount of performers.
    /// 
    /// </summary>
    /// <param name="clickedObject"></param>
    /// <returns></returns>
    private bool IsValidPerformer(GameObject obj)
    {
        if (CheckFunctions.IsUnit(this, obj))
        {


            return CheckFunctions.IsUnitCanAct(this, obj) && CheckFunctions.IsUnitOfCurrentTeam(this, obj);
        }


        return false;
    }



    private bool OnTargetClick(GameObject clickedObject)
    {
        var selectedAction = mems.CurrentAction();

        if (selectedAction.IsValidTarget(this, clickedObject))
        {
            SelectTarget(clickedObject);
            return true;
        }
        else
        {
            Debug.Log($"OnTargetClick: Object {clickedObject}, is invalid for current action");
        }
        return false;


    }

    public void SelectTarget(GameObject target)
    {
        var selectedAction = mems.CurrentAction();
        mems.MemorizeTarget(target);
        BattleFieldState oldState = mems.CurrentFieldState();
        BattleFieldState newState = ToNextSelectionState(oldState, selectedAction);
        if (newState != oldState)
        {
            mems.SetCurrentState(newState);
        }
    }



    public void MoveChildObject(int fromX, int fromY, int toX, int toY, GameObject childObject)
    {
        GameObject fromCell = GetCell(fromX, fromY);
        GameObject toCell = GetCell(toX, toY);

        childObject.transform.SetParent(toCell.transform);

    }


    public Dictionary<string, List<GameObject>> UpdateTeams()
    {
        Dictionary<string, List<GameObject>> unitsByTeam = new Dictionary<string, List<GameObject>>();

        foreach (GameObject cellObject in grid)
        {
            CellMB cell = cellObject.GetComponent<CellMB>();
            UnitMB unit = cell.GetUnitChildIfAny();
            if (unit != null)
            {
                string team = unit.team;
                if (!unitsByTeam.ContainsKey(team))
                {
                    unitsByTeam[team] = new List<GameObject>();
                }
                unitsByTeam[team].Add(unit.gameObject);


            }
        }
        return unitsByTeam;
    }


    public void PrintTeams(Dictionary<string, List<GameObject>> teams)
    {
        foreach (var teamEntry in teams)
        {
            Debug.Log($"Team: {teamEntry.Key}, size: {teamEntry.Value.Count}");
            foreach (GameObject unit in teamEntry.Value)
            {
                Debug.Log($" - Unit: {unit}");
            }
        }
    }

    public void FlyOffAllDebug(Dictionary<string, List<GameObject>> teams)
    {
        foreach (var teamEntry in teams)
        {
            foreach (GameObject unit in teamEntry.Value)
            {
                //ActionFunctions.FlyOff(unit);
            }
        }
    }

    public void ToNextTeamTurn()
    {
        teamNamePos = (teamNamePos + 1) % teamNames.Length;
        currentTeam = teamNames[teamNamePos];
        UpdateIsCurrentTeamAIAtBeginOfTurn();
    }


    public bool HasCurrentTeamFinishedTurn()
    {
        foreach (var unit in teams[currentTeam])
        {
            if (CheckFunctions.IsUnitCanAct(this, unit))
            {
                return false;
            }
        }
        return true;
    }


    public bool CheckTeamDead(string teamName)
    {
        Debug.Log($"CheckTeamDead: called for {teamName}");
        foreach (var unit in teams[teamName])
        {
            UnitMB unitMB = unit.GetComponent<UnitMB>();
            if (unitMB.IsAlive())
            {
                Debug.Log($"CheckTeamDead: team is alive:{teamName} ");
                return false;
            }
        }
        Debug.Log($"CheckTeamDead: team is dead:{teamName} ");
        return true;
    }

    public bool CheckWinOrLoss()
    {
        string playerteam = teamNames[0];
        if (CheckTeamDead(playerteam))
        {
            loseMessage.SetActive(true);
            mems.MemorizeFieldState(BattleFieldState.BattleOver);
            //fieldState = BattleFieldState.BattleOver;
            return true;
        }

        foreach (string otherTeam in teamNames[1..])
        {
            if (!CheckTeamDead(otherTeam))
            {
                return false;
            }
        }
        wonMessage.SetActive(true);
        mems.MemorizeFieldState(BattleFieldState.BattleOver);
        //fieldState = BattleFieldState.BattleOver;
        return true;
    }



    public void RestoreCurrentTeamMoves()
    {
        foreach (var unitGO in teams[currentTeam])
        {
            ActionFunctions.SetUnitCanAct(unitGO, true);
        }
    }

    

    public void DisableAllUnitCanActs()
    {
        foreach (KeyValuePair<string, List<GameObject>> team in teams)
        {
            foreach (GameObject unit in team.Value)
            {
                ActionFunctions.SetUnitCanAct(unit, false);
            }
        }
    }

    public void InitAllUnitCanActs()
    {
        if (teamNames.Length > 0)
        {
            InitTeamNamePos();
            DisableAllUnitCanActs();
            RestoreCurrentTeamMoves();

        }

    }

    public void InitTeamNamePos()
    {
        while (teamNames[teamNamePos] != currentTeam)
        {
            teamNamePos++;
        }
    }

    /// <summary>
    /// If current team is done with moves,
    /// select another team and mark all units 
    /// in this team to be able to act.
    /// </summary>
    public void CheckTeamFinishAfterAction()
    {
        Debug.Log("CheckTeamFinishAfterAction:");
        if (CheckWinOrLoss())
        {
            StartCoroutine(ToExitSceneAnimate());
            return;
        }

        if (HasCurrentTeamFinishedTurn())
        {

            ToNextTeamTurn();
            RestoreCurrentTeamMoves();
            if (mems.CurrentFieldState() == BattleFieldState.AITurn)
            {
                ai.DoRandomAITurn();
            }
        }
    }

    public BattleFieldState ToNextSelectionState(BattleFieldState fieldState, AbstractBattleActionSO selectedAction)
    {
        //BattleFieldState fieldState = mems.CurrentFieldState();
        //var selectedAction = mems.CurrentAction();
        switch (fieldState)
        {

            case BattleFieldState.SelectingFirstPerformer:
                //fieldState = BattleFieldState.SelectingAction;
                return BattleFieldState.SelectingAction;

            case BattleFieldState.SelectingAction:
            case BattleFieldState.SelectingTargets:
                // case BattleFieldState.SelectingOtherPerformers:

                if (selectedAction.CanFinishSelection(this))
                {
                    //fieldState = BattleFieldState.AwaitSelectionFinish;
                    return BattleFieldState.AwaitSelectionFinish;
                }
                else
                {
                    //fieldState = BattleFieldState.SelectingTargets;
                    return BattleFieldState.SelectingTargets;
                }
            //case BattleFieldState.BattleOver:
            //case BattleFieldState.AwaitSelectionFinish:
                //throw new ArgumentException($"ToNextSelectionState should not be called in state {fieldState}");
                //return null


        }
        throw new ArgumentException($"ToNextSelectionState should not be called in state {fieldState}");

    }

    public void PerformSelectedAction()
    {
        mems.SetCurrentState(BattleFieldState.WaitingAnimationEnd);
        AbstractBattleActionSO selectedAction = mems.CurrentAction();
        //BattleFieldState fieldState = mems.CurrentFieldState();
        GameObject selectedFirstPerformer = mems.CurrentFirstPerformer();
        //Debug.Assert(selectedAction != null, $"BattleFieldMB.PerformSelectedAction: selected action is null");

        selectedAction.Act(this);
    }

    public void OnUnitDidAction(GameObject unit)
    {
        //AbstractBattleActionSO selectedAction = mems.CurrentAction();
        //BattleFieldState fieldState = mems.CurrentFieldState();
        //GameObject selectedFirstPerformer = mems.CurrentFirstPerformer();
        Debug.Log($"OnUnitDidAction: unit: {unit}"); 
        ActionFunctions.ApplyStatusEffectsAfterTurn(unit);

        BattleActionMenuUI battleActionMenu = BattleActionMenuUI.Instance;
        battleActionMenu.ResetUI();
        GameObject battleActionMenuGO = battleActionMenu.gameObject;

        battleActionMenuGO.SetActive(false);
        ActionFunctions.ToggleUnitCanAct(unit);
        ResetSelection();
        CheckTeamFinishAfterAction();
    }

    public IEnumerator OnUnitDidActionAnimate(GameObject unit)
    {
        OnUnitDidAction(unit);
        yield return null;
    }

    public IEnumerator ToExitSceneAnimate()
    {
        yield return new WaitForSeconds(5);
        yield return SceneManager.LoadSceneAsync(exitSceneName, LoadSceneMode.Single);
    }



    //void InitHashSets()
    //{
    // selectedPerformers = new HashSet<GameObject>();
    //    selectedTargets = new HashSet<GameObject>();
    //}
    void ResetSelection()
    {
        //selectedFirstPerformer = null;
        //selectedAction = null;
        //selectedTargets.Clear();
        mems.Reset();

    }

    void ValidateHashSetsEmpty()
    {
        // Debug.Assert(selectedPerformers.Count == 0, "BattleFieldMB:Performers Hashtable is not empty!");
        var selectedTargets = mems.CurrentTargets();
        Debug.Assert(selectedTargets.Count == 0, "BattleFieldMB:Targets Hashtable is not empty!");
    }

    /// <summary>
    /// Validating first performer, it just need to belong to a team, whose turn is currently in.
    /// </summary>
    public bool ValidateFirstPerformer(GameObject obj)
    {
        UnitMB unit = obj.GetComponent<UnitMB>();

        return currentTeam.Equals(unit.team);

    }

    /// <summary>
    /// Checks, if the unit is of the current playing team were from current action is performed.
    /// </summary>
    /// <param name="unit"> unit</param>
    /// <returns></returns>
    public bool IsFriendlyUnit(GameObject unit)
    {
        UnitMB unitMB = unit.GetComponent<UnitMB>();
        return currentTeam.Equals(unitMB.team);
    }


    /* public void SetCurrentAction(AbstractBattleActionSO action)
     {
         mems.MemorizeAction(action);
         //selectedAction = actionInput;
     }
     */

    [ContextMenu("ResetScene")]

    public void ResetScene()
    {
        ClearField();
        loadConfig.Load(this);
        ResetTeamsAndUnits();
        UpdateIsCurrentTeamAIAtBeginOfTurn();
    }

    /// <summary>
    /// returns true if current team is AI controlled
    /// </summary>
    public void UpdateIsCurrentTeamAIAtBeginOfTurn()
    {
        bool isAi = aiTeams.TryGetValue(currentTeam, out var v) ? v : false;
        if (isAi)
        {
            mems.SetCurrentState(BattleFieldState.AITurn);
        }
        else
        {
            mems.SetCurrentState(BattleFieldState.SelectingFirstPerformer);
        }
    }

    public void UpdateResetButton()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }
    }

    [ContextMenu("ClearField")]
    public void ClearField()
    {
        foreach (var cell in grid)
        {
            foreach (Transform child in cell.transform)
            {
                GameObject childGO = child.gameObject;
                MB_TypedBattlefieldObject type = childGO.GetComponent<MB_TypedBattlefieldObject>();
                if (type != null)
                {
                    //the issue that this will be executed some time later, 
                    Destroy(childGO);
                    //so need to disconnect the game object rom the parent immetiately like that:
                    childGO.transform.SetParent(null);
                }

            }

        }
    }

    public void LoadSceneOld(BattleFieldConfig load)
    {
        foreach (BattleFieldPositioned4Unit unit in load.units)
        {
            //int n = unit.zone * width + unit.lane;
            int n = GridFunctions.ToIndex(unit, width);
            GameObject cell = grid[n];


            GameObject newUnit = ActionFunctions.SpawnUnit(unitPrefab, cell, unit.unit);
            SpriteFromSOMB spriteMB = newUnit.GetComponent<SpriteFromSOMB>();
            spriteMB.sprite = unit.sprite;
            spriteMB.UpdateSprite();

            UnitMB unitMB = newUnit.GetComponent<UnitMB>();
            unitMB.team = unit.team;

            Creature3dVisuals visuals = newUnit.GetComponent<Creature3dVisuals>();
            if (visuals != null)
            {
                Debug.Log($"Setting visuals for {newUnit}: {unit.mesh}, {unit.material}");
                visuals.mesh = unit.mesh;
                visuals.material = unit.material;
                visuals.UpdateVisuals();
                if (unit.rotate)
                {
                    visuals.gameObject.transform.Rotate(0.0f, 180.0f,  0.0f, Space.World);
                }
            }

        }
    }




 
    public void UpdateUndo()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {

            mems.UndoLastIfNotEmpty();
            //Debug.Log("Undo performed. Current memory: " + mems.CurrentInfo());
            //selection menu has to be updated or reseted : 
            GameObject firstPerformer = mems.CurrentFirstPerformer();
            BattleActionMenuUI battleActionMenu = BattleActionMenuUI.Instance;
            if (firstPerformer == null)
            {
                battleActionMenu.ResetUI();
            }
            else
            {
                battleActionMenu.SetCurrentPerformer(firstPerformer);
            }


        }
    }



}
