using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : MonoBehaviour {

    public EnnemiAI prisonner;
    public Collider2D prisonner_coll;
    public int prisonner_mod = 0;
    
    public bool chain_broke = false;
    private BoxCollider2D[] colliders;
    private Rigidbody2D rb_prison;
    private bool once = false;
    void Start () {
        colliders = this.GetComponentsInChildren<BoxCollider2D>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile") , LayerMask.NameToLayer("Default"));
        rb_prison = this.GetComponent<Rigidbody2D>();
        if (prisonner != null)
        {
            prisonner.SetMod(prisonner_mod);
            Collider2D col_barreaux = GameObject.Find(this.transform.parent.name+"/"+this.name + "/Barreaux").GetComponent<BoxCollider2D>();
            if (prisonner_coll == null)
                print("prisonner_coll null");
            else
                Physics2D.IgnoreCollision(prisonner_coll, col_barreaux);

        }
    }
	
	void Update () {
		if(chain_broke && !once)
        {
            rb_prison.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            once = true;
        }
	}

    public BoxCollider2D[] GetColliders()
    {
        return colliders;
    }
}
