using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static UnityEngine.UI.CanvasScaler;
public class MenuUnitInfoMB : MonoBehaviour
{
    public static MenuUnitInfoMB Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI creatureName;

   // [SerializeField] private  GameObject creatureHealth;
    [SerializeField] private GameObject healthBarGO; // assign the HealthBar GameObject
    [SerializeField] private GameObject manaBarGO;
    [SerializeField] private UnityEngine.UI.Image creatureSpriteImage;
    [SerializeField] private UnityEngine.UI.Image healthFill;
    [SerializeField] private UnityEngine.UI.Image manaFill;


    private void Update()
    {
        LeftMouseHandle();
    }
    public void LeftMouseHandle()
    {


        
        if (Input.GetMouseButtonDown(0))
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // Check if the clicked object itself has UnitMB
                UnitMB unit = clickedObject.GetComponent<UnitMB>();

                // If not, check if the clicked object has ReferenceOnClick pointing to a UnitMB
                if (unit == null)
                {
                    ReferenceOnClick refScript = clickedObject.GetComponent<ReferenceOnClick>();
                    if (refScript != null && refScript.referencedObject != null)
                    {
                        unit = refScript.referencedObject.GetComponent<UnitMB>();
                        
                    }
                }
       

                if (unit != null)
                {
                    setInfo(unit);  // Update your TextMeshProUGUI here
                }
                else
                {
                    Debug.Log("Clicked object has no UnitMB!");
                }
            }
        }

    }

    public void setInfo(UnitMB creatureData)
    {
    {     
        // Name    
        if (creatureData.initialData.name != null)
        {
                creatureName.text = creatureData.initialData.name; // display the name from the UnitDataSO
        }
        else
        {
                creatureName.text = " "; // fallback
        }


        //Health
        if (healthBarGO != null && healthFill != null)
        {
            healthBarGO.SetActive(true);
            float fillAmount = Mathf.Clamp01((float)creatureData.data.GetHP() / creatureData.initialData.GetHP());
            healthFill.fillAmount = fillAmount;
        }


            ////  Mana
            if (manaBarGO != null && manaFill != null)
            {
             manaBarGO.SetActive(true);
            //    float fillManAAmount = Mathf.Clamp01((float)creatureData.data.mana / creatureData.initialData.HP);
            //    healthFill.fillAmount = fillManAAmount;
            }


            //Sprite\\
            if (creatureSpriteImage != null)
            {
                // Find the child named "UnitSpriteVisual"
                SpriteRenderer sr = creatureData.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    //SpriteRenderer sr = spriteChild.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        creatureSpriteImage.sprite = sr.sprite;
                        creatureSpriteImage.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("a");
                }
            }
        }

    }
}   


