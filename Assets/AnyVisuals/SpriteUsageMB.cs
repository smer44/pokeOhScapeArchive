using UnityEngine;

public class SpriteFromSOMB : MonoBehaviour
{
    public Sprite sprite;
    public GameObject unitSprite;
    public Vector2 targetSize = new Vector2(1f, 1f);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSprite();

        //Debug.Log($"Sprite set: {spriteSO.sprite}");
    }

    public void UpdateSprite()
    {
        SetSprite(unitSprite, sprite , targetSize);
    }


    public static void SetSprite(GameObject go, Sprite sprite, Vector2 targetSize )
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        if (sprite == null)
            return;

        //resize sprite to given transform size:
        Vector2 spriteSize = sprite.bounds.size;
        Vector3 newScale = go.transform.localScale;

        newScale.x = targetSize.x / spriteSize.x;
        newScale.y = targetSize.y / spriteSize.y;
        go.transform.localScale = newScale;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
