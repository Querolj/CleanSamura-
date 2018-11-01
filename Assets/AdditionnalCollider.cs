using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionnalCollider : MonoBehaviour {
    public bool coll_enabled = false;

    private Transform t;
    private EnnemiAI behaviour;
    void Start () {
        t = this.GetComponent<Transform>();
        behaviour = this.GetComponentInParent<EnnemiAI>();
        if (behaviour == null)
            print("behaviour null in AdditionnalCollider");
        this.GetComponent<Collider2D>().enabled = coll_enabled;
    }
	
	void Update () {
        Quaternion q = t.localRotation;
		if(behaviour.direction)
        {
            q.y = 180;
        }
        else
        {
            q.y = 0;
        }
        t.localRotation = q;
	}
}
