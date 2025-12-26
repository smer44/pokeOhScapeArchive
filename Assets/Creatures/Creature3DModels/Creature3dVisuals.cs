using UnityEngine;

public class Creature3dVisuals : MonoBehaviour
{
    [Header("GameObject with visuals where this will be set")]
    [SerializeField] public GameObject visuals;

    [Header("Will set this to visuals")]
    [SerializeField] public Mesh mesh;
    [SerializeField] public Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateVisuals()
    {
        if (visuals == null)
        {
            Debug.LogWarning("[Creature3dVisuals] 'visuals' is not assigned.", this);
            return;
        }
        if (mesh != null)
        {
            var meshFilter = visuals.GetComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
        }
        if (material != null)
        {
            var meshRenderer = visuals.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = material;
        }


    }
}
