using UnityEngine;

public enum UnitColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Orange,
    Cyan,
    Magenta,
    White,
    Black,
 
}


public class MaterialColorChange : MonoBehaviour
{
    public GameObject model;
    //public Material[] childMaterials;
    public int[] materialIndexes = { 0, 1, 2, 4 };
    Renderer childRenderer;

    [Header("Color Settings")] 
    public UnitColor unitColor = UnitColor.Red;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        childRenderer = model.GetComponent<Renderer>();
        //childMaterials = childRenderer.materials;
        ApplyHueToAllChildMaterials();
        //Debug.Log($"Found {childMaterials} materials on model '{model}'.");
    }

    // Update is called once per frame
    void Update()
    {

    }


    static float HueFromUnitColor(UnitColor color)
    {
        switch (color)
        {
            case UnitColor.Red: return 0f;                  // 0°
            case UnitColor.Orange: return 30f / 360f;       // 30°
            case UnitColor.Yellow: return 60f / 360f;       // 60°
            case UnitColor.Green: return 120f / 360f;       // 120°
            case UnitColor.Cyan: return 180f / 360f;        // 180°
            case UnitColor.Blue: return 240f / 360f;        // 240°
            case UnitColor.Purple: return 275f / 360f;      // ~275°
            case UnitColor.Magenta: return 300f / 360f;     // 300°
            case UnitColor.White:
            case UnitColor.Black:
                // Keep hue unchanged; choose 0 as placeholder (we'll preserve S/V below)
                return 0f;
            default: return 0f;
        }
    }
    static Color ColorWithHue(Color source, float newHue01, float saturationOverride = -1f, float valueOverride = -1f)
    {
        Color.RGBToHSV(source, out float h, out float s, out float v);

        // For white/black, keep S=0 or V=0 unless explicitly overridden
        if (saturationOverride >= 0f)
            s = Mathf.Clamp01(saturationOverride);
        if (valueOverride >= 0f)
            v = Mathf.Clamp01(valueOverride);

        // If original color is grayscale (s≈0), give it a reasonable default saturation so hue is visible
        //if (s <= 0.0001f && saturationOverride < 0f)
        s = 1f;

        return Color.HSVToRGB(newHue01, s, v);
    }

    public static bool ApplyHueToMaterial(Material m, float hue, float satOverride = -1f, float valOverride = -1f)
    {
        if (m.HasProperty("_Color"))
        {
            m.color = ColorWithHue(m.color, hue, satOverride, valOverride);
            return true;
        }
        else if (m.HasProperty("_BaseColor"))
        {
            var baseCol = m.GetColor("_BaseColor");
            m.SetColor("_BaseColor", ColorWithHue(baseCol, hue, satOverride, valOverride));
            return true;
        }
        return false;
    }

    public void ApplyHueToAllChildMaterials()
    {   
        Material[] materials = childRenderer.materials;
        foreach (int n in materialIndexes)
        {
            Material m = materials[n];
            Debug.Log($"ApplyHueToAllChildMaterials: Applying hue to material '{m}'.");
            float targetHue = HueFromUnitColor(unitColor);
            ApplyHueToMaterial(m, targetHue);
        }
    }




}
