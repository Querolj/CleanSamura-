using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAnimation : MonoBehaviour {
    public Sprite[] spritesNormalStates;
    private Spawner spawner;

    private SpriteRenderer spriteRenderer;
    public float fps_normal;
    public float fps_spawn;

    // Use this for initialization
    void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spawner = this.GetComponent<Spawner>();


    }
	
	// Update is called once per frame
	void Update () {
        int index_normal = (int)(Time.timeSinceLevelLoad * fps_normal);

        if(!spawner.spawn)
        {
            index_normal = index_normal % spritesNormalStates.Length;
            spriteRenderer.sprite = spritesNormalStates[index_normal];
        }

    }
}
