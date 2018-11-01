using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortAnimation : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public bool on_shoot = false;
    public bool is_instantiated = false;

    public float fps;

    private int count_anime = 0;
    private int old_index = -1;
    private bool end = false;

    public AudioClip sound;
    private AudioSource source;
    void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = this.GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer == null)
            print("spriteRenderer null");

        if(source != null)
        {
            source.volume = 0.4f;
            source.clip = sound;
            source.playOnAwake = false;
            PlaySound();
        }
    }
	
	void Update () {
        if(!end)
        {
            int index = (int)(Time.timeSinceLevelLoad * fps);

            if (!on_shoot)
            {
                index = index % sprites.Length;
                spriteRenderer.sprite = sprites[index];
            }
            else
            {
                index = index % sprites.Length;
                if (old_index != index && count_anime < sprites.Length)
                {
                    spriteRenderer.sprite = sprites[count_anime];
                    count_anime++;
                }
                else if (count_anime >= sprites.Length)
                {
                    end = true;
                    spriteRenderer.sprite = null;
                }
                old_index = index;
            }
        }
    }

    private void PlaySound()
    {
        source.PlayOneShot(sound);
    }

    public bool IsEnded()
    {
        return end;
    }
}
