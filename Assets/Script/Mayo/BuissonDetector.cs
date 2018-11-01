using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuissonDetector : MonoBehaviour {
    //Envoie un message à Mayo si ils se touchent avec les coordonnées nécéssaire pour changer de perspective
    MayoController mc;

    bool mayo_in = false;
    Transform trans_buisson;
    Rigidbody2D rb_buisson;

    void Start () {
		mc = GameObject.Find("Mayo").GetComponent<MayoController>();
        trans_buisson = this.GetComponent<Transform>();
        rb_buisson = this.GetComponent<Rigidbody2D>();
    }
	
    void OnTriggerStay2D(Collider2D other)
    {
        if (!mayo_in && LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {              
            mayo_in = true;
            //Envoie des données
            mc.SetBuisson(trans_buisson, rb_buisson, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Player") && mayo_in)
        {
            mayo_in = false;
            //Données set à null dans MayoController
            mc.SetBuisson(null, null, false);
        }
    }
}
