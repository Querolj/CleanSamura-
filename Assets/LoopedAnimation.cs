using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopedAnimation : MonoBehaviour {

    public Sprite[] sprites;
    public float fps;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int index = (int)(Time.timeSinceLevelLoad * fps);
        index = index % sprites.Length;
        spriteRenderer.sprite = sprites[index];
    }
}
