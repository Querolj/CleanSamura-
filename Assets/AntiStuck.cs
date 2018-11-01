using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiStuck : MonoBehaviour {
    private SamuraiController sc;
    private Transform t;
    private bool colliding = false;
	// Use this for initialization
	void Start () {
        sc = this.GetComponentInParent<SamuraiController>();
        t = this.GetComponent<Transform>();    
    }
	
	// Update is called once per frame
	void Update () {
        ChangeDirection();

    }

    private void ChangeDirection()
    {
        Vector3 v = t.localPosition;
        if (sc.GetDirectionFloat() > 0)
        {
            v.x = 0.19f;//. 0.235f;
        }
        else if (sc.GetDirectionFloat() < 0)
        {
            v.x = 0;
        }
        t.localPosition = v;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (sc.dashing)
        {
            print("dash canceled");
            //sc.EndDash();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        //colliding = true;
        if (sc.dashing)
        {
            /*print("dash canceled");
            sc.EndDash();*/
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        colliding = false;
    }

    public bool isAntiStuckColliding()
    {
        return colliding;
    }
}
