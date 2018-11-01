using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrigger : MonoBehaviour {

    private SamuraiController sc;
    private BoxCollider2D body_trigger;
    void Start () {
        sc = this.GetComponentInParent<SamuraiController>();
        body_trigger = this.GetComponent<BoxCollider2D>();
        body_trigger.offset = sc.GetOffsetCollider();
        body_trigger.size = sc.GetSizeCollider();
	}
	
	void Update () {

        if (body_trigger.offset != sc.GetOffsetCollider())
        {
            body_trigger.offset = sc.GetOffsetCollider();
            body_trigger.size = sc.GetSizeCollider();
        }
    }
}
