using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {
    public int life_bonus;
    private SamuraiController sc;
    private LifeGestion life_gestion;
    private AudioSource source;
    private bool destroy = false;
    private SpriteRenderer renderer;
    void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        source = this.GetComponent<AudioSource>();
        life_gestion = GameObject.Find("GeneralSettings/LifeDisplay").GetComponent<LifeGestion>();
        renderer = this.GetComponent<SpriteRenderer>();

    }
    void Update()
    {
        if (destroy && !source.isPlaying)
            GameObject.Destroy(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && sc.life < 5)
        {
            destroy = true;
            sc.AddLife(life_bonus);
            life_gestion.UpdateLifeDisplayHeal(sc.life, life_bonus);
            source.Play();
            renderer.enabled = false;
        }
    }
}
