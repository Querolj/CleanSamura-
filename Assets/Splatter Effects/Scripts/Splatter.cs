using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
    public List<Sprite> sprites; //ref to the sprites which will be used by sprites renderer
    [HideInInspector]
    public Color32 splatColor; //color values which can be assigned by another script
    private SpriteRenderer spriteRenderer;//ref to sprite renderer component
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        this.tag = "blood";
        BoxCollider2D box = this.gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(1.35386f, 0.4854428f);
        box.offset = new Vector2(-0.05814904f, -0.3322785f);
        box.isTrigger = true;
        splatColor = new Color32(255,0,0,255);
        //at start we randomly select the sprites
        spriteRenderer.sprite = sprites[0];//[Random.Range(0, sprites.Count)];
        spriteRenderer.transform.localScale = new Vector3(0.2f, 0.1f, 1);

        //this.GetComponent<Transform>().lossyScale = new Vector3(0.1f, 0.1f, 1);
        //checks if randomColor is true and then randomly apply the colors
        ApplyStyle();
        
    }
    //this methode assign the color to the splatter
    public void ApplyStyle()
    {

        spriteRenderer.color = splatColor;
        
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 1));
    }
    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }
    public Sprite GetSprite()
    {
        return sprites[0];
    }
}
