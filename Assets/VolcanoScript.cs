
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoScript : MonoBehaviour {
    public float cooldown;
    public float projectile_strenght;
    public GameObject projectile;

    private SimpleAnimation anime;
    private float timer;
    private Transform throw_point;

    void Start () {
        anime = this.GetComponent<SimpleAnimation>();
        timer = cooldown;
        throw_point = GameObject.Find(this.transform.parent.name+"/"+this.name+ "/ThrowPoint").GetComponent<Transform>();//this.GetComponentInChildren<Transform>().position;
    }
	
	void Update () {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            InitiateProjectile();
        }
        if (anime.MustLaunch())
            LaunchProjectile();
    }

    private void InitiateProjectile()
    {
        anime.Play();
        timer = cooldown;
    }
    private void LaunchProjectile()
    {
        
        GameObject fireball = Instantiate(projectile, throw_point.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        int ecart = 50;
        float r = Random.Range(-ecart, ecart+1);

        Vector2 v = new Vector2(r/100, Vector2.up.y);
        rb.AddForce(v * projectile_strenght);
        anime.SetLaunch(false);
    }
}
