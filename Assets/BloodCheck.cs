using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCheck : MonoBehaviour {
    public float x_pos_left = 0; 

    private EnnemiAI ai;
    private Transform t;
    private bool on_ground = false;
    private int count_blood = 0;
    protected virtual void Start () {
        ai = this.GetComponentInParent<EnnemiAI>();
        t = this.GetComponent<Transform>();
    }
	
	protected virtual void Update () {
        if(x_pos_left != 0)
        {
            if (ai.direction)
            {
                Vector3 v = t.localPosition;
                v.x = 0;
                t.localPosition = v;
            }
            else
            {
                Vector3 v = t.localPosition;
                v.x = x_pos_left;
                t.localPosition = v;
            }
        }
		
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "blood")
        {
            ai.on_blood = true;
            count_blood++;
        }
        if(GlobalVar.OnGroundCondition(other.gameObject))
        {
            on_ground = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "blood")
        {
            if(count_blood > 0)
                count_blood--;
            if (count_blood == 0)
                ai.on_blood = false;
        }
        if (GlobalVar.OnGroundCondition(other.gameObject))
        {
            on_ground = false;
        }

    }

    public bool IsOnGround()
    {
        return on_ground;
    }
}
