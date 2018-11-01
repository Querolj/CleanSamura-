using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {
    public Splatter splatter;
    private SamuraiController sc;
    public int projectile_strenght = 1;
    public float throw_drawnback = 3;

    // Use this for initialization
    void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
       
    }



    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag== "Surface" && LayerMask.LayerToName(coll.gameObject.layer) == "Surface")
        {
            Vector3 v = this.transform.position;
            v.y += 0f;
            GameObject parent_anti_scale = new GameObject();
            parent_anti_scale.name = "anti_scaling";
            parent_anti_scale.transform.parent = coll.gameObject.transform;
            Splatter splatterObj = (Splatter)Instantiate(splatter, v, Quaternion.identity, parent_anti_scale.transform);
            GameObject.Destroy(this.gameObject);
        }
        else if (coll.gameObject.tag == "Player")
        {
            sc.ReceiveDamage(projectile_strenght, throw_drawnback, sc.GetDirection());
            GameObject.Destroy(this.gameObject);
        }
        
    }
}
