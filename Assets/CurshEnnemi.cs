using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurshEnnemi : MonoBehaviour {

    private Prison prison;
    private AudioSource source;
    private bool can_crush = true;
    void Start () {
        prison = this.GetComponentInParent<Prison>();
        source = this.GetComponent<AudioSource>();

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ennemi" && prison.chain_broke && can_crush)
        {
            EnnemiStates states = other.gameObject.GetComponent<EnnemiStates>();
            foreach(BoxCollider2D coll in prison.GetColliders() )
            {
                Physics2D.IgnoreCollision(coll, states.GetCollider());
            }
            
            states.SetDeath(true);
        }
        if(other.gameObject.tag == "Surface")
        {
            can_crush = false;
            source.Play();
        }
    }
}
