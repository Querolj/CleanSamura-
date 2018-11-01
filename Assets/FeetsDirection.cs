using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetsDirection : MonoBehaviour {

    private SamuraiController sc;
    private Transform t;
    private bool ignored = false;
    void Start () {
        sc = this.GetComponentInParent<SamuraiController>();
        t = this.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if(!ignored)
        {
            ignored = true;
            Physics2D.IgnoreCollision(this.GetComponent<CapsuleCollider2D>(), this.GetComponentInParent<CapsuleCollider2D>());
        }

        Quaternion q = sc.trans.localRotation;//t.localRotation;
        if (sc.GetDirection())
        {
            q.y= 180;
        }
        else
        {
            q.y = 0;
        }
        sc.trans.localRotation = q;
	}
}
