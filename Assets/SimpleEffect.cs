using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEffect : MonoBehaviour {

    public Sprite[] sprites;
    public float fps;

    private SpriteRenderer spriteRenderer;
    private int count_sprite = 0;
    private int old_index = -1;
    private bool play = true;
    private bool launch = false;
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int index = (int)(Time.timeSinceLevelLoad * fps);
        if (play)
        {
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
                Erase();
            }
        }
    }

    private void Erase()
    {
        GameObject.Destroy(this.gameObject);
    }

    public bool Play()
    {
        if (!play)
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
