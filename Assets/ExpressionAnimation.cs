using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionAnimation : MonoBehaviour {
    public Sprite[] spritesSpotted;

    private SpriteRenderer spriteRenderer;
    public float fps_spotted;
    public float time_spotted = 1f;
    [HideInInspector]
    public bool play_spotted = false;
    [HideInInspector]
    public bool dead = false;
    private EnnemiAI behaviour;
    private int old_mod = 0;
    // Use this for initialization
    void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        behaviour = this.GetComponentInParent<EnnemiAI>();
    }
	
	// Update is called once per frame
	void Update () {
        int index_spotted= (int)(Time.timeSinceLevelLoad * fps_spotted);

        if (play_spotted && !dead && behaviour.mods == 1 && old_mod != behaviour.mods)
        {
            Invoke("PlaySpotted", time_spotted);
            index_spotted = index_spotted % spritesSpotted.Length;
            spriteRenderer.sprite = spritesSpotted[index_spotted];

        }
        else
        {
            spriteRenderer.sprite = null;
        }
        old_mod = behaviour.mods;


    }

    private void PlaySpotted()
    {
        play_spotted = false;
    }
}
