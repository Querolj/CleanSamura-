using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Fonctionnement : 
 * Ajouter une Light source appelé "Light" en enfant, puis ajusté la lumière (position et scale)
 *
 */
public class FireAnimation : MonoBehaviour {
    public Sprite[] fire;
    private SpriteRenderer spriteRenderer;
    public float fps;

    private Transform light_pos;
    private Vector3 light_scale_little;
    private Vector3 light_scale_big;
    [Range(2, 50)]
    public int light_interval;
    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        light_pos = GameObject.Find(this.name + "/Light").GetComponent<Transform>();
        light_scale_little = light_pos.localScale;
        light_scale_little.x *= 0.9f;
        light_scale_little.y *= 0.9f;
        light_scale_big = light_pos.localScale;
        light_scale_big.x *= 1f;
        light_scale_big.x *= 1f;
        light_interval = 2;
    }
	
	// Update is called once per frame
	void Update () {
        int index = (int)(Time.timeSinceLevelLoad * fps);
        index = index % fire.Length;
        spriteRenderer.sprite = fire[index];
        if((int)(Time.timeSinceLevelLoad * fps * 10) % light_interval == 0)
        {
            light_pos.localScale = light_scale_little;
        }
        else
        {
            light_pos.localScale = light_scale_big;
        }
        
    }
}
