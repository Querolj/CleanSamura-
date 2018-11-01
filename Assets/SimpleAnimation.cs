using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour {
    public Sprite[] sprites;
    public Sprite initialSprite;
    public float fps;
    [Space]
    public bool interval_on = false;
    public float interval_cd;
    
    private SpriteRenderer spriteRenderer;
    private float interval_time = 0;
    private int count_sprite = 0;
    private int old_index = -1;
    public bool play = false;
    public bool loop = false;
    private bool launch = false;
    void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        
        if (play && !interval_on)
        {
            Animation();
        }
        else if(interval_on)
        {
            if (interval_time <= 0)
                Animation();
            else
                interval_time -= Time.deltaTime;
        }
    }

    private void Animation()
    {
        int index = (int)(Time.timeSinceLevelLoad * fps);
        index = index % sprites.Length;
        if (count_sprite < sprites.Length)
        {
            if (old_index != index)
            {
                spriteRenderer.sprite = sprites[count_sprite];
                if (count_sprite == 3)
                    launch = true;

                count_sprite++;
            }
            old_index = index;
        }
        else
        {
            Reboot();
        }
    }
    private void Reboot()
    {
        if(!loop)
            play = false;
        //spriteRenderer.sprite = initialSprite;
        count_sprite = 0;
        if (interval_on)
            interval_time = interval_cd;
    }
    
    public bool Play()
    {
        if(!play)
        {
            play = true;
            return true;
        }
        return false;
    }
    public bool IsPlaying()
    {
        return play;
    }
    public bool MustLaunch()
    {
        return launch;
    }
    public void SetLaunch(bool b)
    {
        launch = b;
    }
}
