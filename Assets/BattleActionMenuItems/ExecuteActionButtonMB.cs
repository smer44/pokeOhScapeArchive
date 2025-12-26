using UnityEngine;
using UnityEngine.UI;

public class ExecuteActionButtonMB : MonoBehaviour
{
    //public BattleFieldMB battleField;
    void Start()
    {
        // set onclick action to execute current action selected in battlefield
        GetComponent<Button>().onClick.AddListener(() =>
        {
            BattleFieldMB field = BattleFieldMB.Instance;
            BattleFieldState state = field.mems.CurrentFieldState();
            if (state == BattleFieldState.AwaitSelectionFinish && field.mems.CurrentAction() != null)
            {
                field.PerformSelectedAction();
            }
            else
            {
                Debug.Log("ExecuteActionButton: pressed while no action is selected");
                //TODO: also all required targets should be selected.
            }

        });

        BattleFieldMB field = BattleFieldMB.Instance;
        field.onBattleFieldStateChanged += (state) =>
        {
            BattleFieldMB field = BattleFieldMB.Instance;
            gameObject.SetActive(state == BattleFieldState.AwaitSelectionFinish && field.mems.CurrentAction() != null);
        };
        
        gameObject.SetActive(false);
    }

    void Update()
    {

    }
}
