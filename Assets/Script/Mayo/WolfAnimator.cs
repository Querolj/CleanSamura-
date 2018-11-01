using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimator : MonoBehaviour {
    public float fps;

    public Sprite[] spritesLeft;
    public Sprite[] spritesRight;

    private WolfController wc;
    private SpriteRenderer spriteRenderer;
    public int wolf_number = 0;
    void Start () {
		wc = GameObject.Find("Wolf"+wolf_number).GetComponent<WolfController>();
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
    }
	
	void Update () {
        int index = (int)(Time.timeSinceLevelLoad * fps);

        if(wc.directed)
        {
            index = index % spritesRight.Length;
            spriteRenderer.sprite = spritesRight[index];
        }
        else
        {
            index = index % spritesLeft.Length;
            spriteRenderer.sprite = spritesLeft[index];
        }
    }
}
