using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Zone"), LayerMask.NameToLayer("Zone"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ennemi"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Blood"), LayerMask.NameToLayer("Player"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Blood"), LayerMask.NameToLayer("Ennemi"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ennemi"), LayerMask.NameToLayer("Ennemi"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Default"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Spec"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Ennemi"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Projectile"));
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
