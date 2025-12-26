// Simple script to show outline on hover
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineOnHover : MonoBehaviour
{
    private Material originalMaterial;
    public Material outlineMaterial;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    void OnMouseEnter()
    {
        if (outlineMaterial != null)
        {
            objectRenderer.material = outlineMaterial;
        }
    }

    void OnMouseExit()
    {
        objectRenderer.material = originalMaterial;
    }
}
