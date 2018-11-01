using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levier : MonoBehaviour {

    public Sprite[] spritesLeftToRight;
    public Sprite[] spritesRightToLeft;
    public float fps;

    private SpriteRenderer spriteRenderer;
    private bool activated;
    private bool on_off = false;
    private int count_lr = 0;
    private int count_rl = 0;
    private int old_index_lr = -1;
    private int old_index_rl = -1;


    void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

    }
	
	void Update () {
        int index = (int)(Time.timeSinceLevelLoad * fps);

        if(activated)
        {
            if(on_off)
            {
                if(count_lr < spritesLeftToRight.Length)
                {
                    index = index % spritesLeftToRight.Length;
                    if (old_index_lr != index)
                    {
                        spriteRenderer.sprite = spritesLeftToRight[count_lr];
                        count_lr++;
                    }

                    old_index_lr = index;
                }
                else
                {
                    count_lr = 0;
                    activated = false;
                    old_index_lr = -1;
                }

            }
            else
            {
                if (count_rl < spritesRightToLeft.Length)
                {
                    index = index % spritesRightToLeft.Length;
                    if (old_index_rl != index)
                    {
                        spriteRenderer.sprite = spritesRightToLeft[count_rl];
                        count_rl++;
                    }

                    old_index_rl = index;
                }
                else
                {
                    count_rl = 0;
                    activated = false;
                    old_index_rl = -1;

                }
            }
            

        }

    }


    public bool GetActivate()
    {
        return activated;
    }
    public bool GetOnOff()
    {
        return on_off;
    }
    public void OnOff()
    {
        activated = true;
        if (on_off)
            on_off = false;
        else
            on_off = true;
        //print("onoff " + GetOnOff());
    }


    public void SetActivate(bool b)
    {
        activated = b;
    }
}
