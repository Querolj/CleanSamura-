using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoredByDash : MonoBehaviour {
    public Collider2D samurai_coll;
    public float time_ignored_cd = 0.2f;

    private SamuraiController sc;
    private Collider2D coll;
    private bool ignored = false;
    private float time_ignored = 0;
    void Start () {
        
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        coll = this.GetComponent<Collider2D>();
       
    }
	
	void Update () {
		if (sc.dashing && !sc.jump_back && !ignored)
        {
            ignored = true;
            coll.enabled = false;
            time_ignored = time_ignored_cd;
            print("ignored");
            //Physics2D.IgnoreCollision(coll, samurai_coll);
        }

        if(time_ignored > 0)
        {
            time_ignored -= Time.deltaTime;
            if(time_ignored <= 0)
            {
                ignored = false;
                coll.enabled = true;
            }
        }
        /*else if(!sc.dashing && ignored)
        {
            ignored = false;
            Physics2D.IgnoreCollision(coll, samurai_coll, false);
        }*/
    }
}
