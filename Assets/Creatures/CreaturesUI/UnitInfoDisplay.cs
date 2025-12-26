using UnityEngine;
using TMPro;
using System;


public class UnitInfoDisplay : MonoBehaviour
{
    public static UnitInfoDisplay Instance { get; private set; }
    public UnitMB unit;
    public GameObject entryPrefab;

    private Canvas _canvas;
    private RectTransform _rect;
    private Vector2 _dragLastLocal;

    private int mouseButtonDragNumber = 0;

    void Awake()
    {
        InitSingleton();
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

    }
    void Update()
    {
        UpdateDrag();
    }

    void UpdateDrag()
    {

        // Begin drag with RMB
        if (Input.GetMouseButtonDown(mouseButtonDragNumber))
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_rect.parent,
                Input.mousePosition,
                GetCanvasCamera(),
                out var localPoint))
            {
                _dragLastLocal = localPoint;
            }
        }

        // Continue drag while RMB held
        if (Input.GetMouseButton(mouseButtonDragNumber))
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_rect.parent,
                Input.mousePosition,
                GetCanvasCamera(),
                out var localNow))
            {
                Vector2 delta = localNow - _dragLastLocal;
                _rect.anchoredPosition += delta;
                _dragLastLocal = localNow;
            }
        }
    }
    private Camera GetCanvasCamera()
    {
        // For Screen Space - Overlay, camera is null; otherwise use the canvas camera
        return _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
    }

    void InitSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception("UnitInfoDisplay: should be a sincleton, but more that 1 instance created");
        }  
    }


    void Start()
    {

    }

    public void SetUnitUpdateInfo(GameObject unit)
    {
        this.unit = unit.GetComponent<UnitMB>();
        UpdateInfo();
    }

   public void SetNoneUpdateInfo()
    {
        this.unit = null;
        UpdateInfo();
    }

    private void AddDisplayStat(Stat stat)
    {

        string text = $" - {GetStatName(stat.type)}: {stat.value}";
        AddDisplayText(text);
    }
    
    private void AddDisplayStatusEffect(SOAbstractStatusEffect effect)
    {
        string text;
        if (effect == null)
        {
            text = " - null";
        }
        else
        {
            text = $" - {effect}";
        }
        
        AddDisplayText(text);
    }

    private void AddDisplayText(string text)
    {
        GameObject entryGO = Instantiate(entryPrefab, transform);
        TextMeshProUGUI titleText = entryGO.GetComponent<TextMeshProUGUI>();
        titleText.text = text;
    }

    private void DestroyAllChildren()
    {

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateInfo()
    {   
        DestroyAllChildren();
        if (unit != null)
        {
            AddDisplayText("Name:" + unit.initialData.name);
            AddDisplayText("Type:" + unit.data.type.ToString());
            AddDisplayText("Role:" + unit.data.role.ToString());
            AddDisplayText("Disposition:" + unit.disposition.ToString());
            AddDisplayText("Can Act:" + unit.CanAct.ToString());
            AddDisplayText("Stats:");
            foreach (Stat stat in unit.data.stats)
            {
                AddDisplayStat(stat);
            }
            AddDisplayText("Status Effects:");
            foreach (SOAbstractStatusEffect effect in unit.statusEffects)
            {
                AddDisplayStatusEffect(effect);
            }
            AddDisplayText("*** END ***");
        }



    }

    private string GetStatName(StatType type)
    {
        switch (type)
        {
            case StatType.HP: return "HP";
            case StatType.Attack: return "Attack";
            case StatType.Mana: return "Mana";
            case StatType.PhysicalDefence: return "Physical Defense";
            case StatType.FireDefense: return "Fire Defense";
            case StatType.WaterDefense: return "Water Defense";
            case StatType.GrassDefense: return "Grass Defense";
            default: return $"Uknown Stat {type}";
        }
    }
}
