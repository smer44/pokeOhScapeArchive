using UnityEngine;

[System.Serializable]
public class BattleFieldPositioned4Unit
{
    public int zone;
    public int lane;

    public UnitSO unit;
    public string team;
    public Sprite sprite;
    public Mesh mesh;
    public Material material;
    public bool rotate = false;
}

[CreateAssetMenu(fileName = "BattleFieldConfig", menuName = "Scriptable Objects/BattleFieldConfig")]
public class BattleFieldConfig : ScriptableObject
{
    //public UnitSO[] units;
    //public int[] zones;
    //public int[] lanes;

    public BattleFieldPositioned4Unit[] units;


    public void Load(BattleFieldMB field)
    {
        foreach (BattleFieldPositioned4Unit unit in units)
        {
            int n = GridFunctions.ToIndex(unit, field.width);
            GameObject cell = field.grid[n];
            GameObject newUnit = ActionFunctions.SpawnUnit(field.unitPrefab, cell, unit.unit);
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
}
