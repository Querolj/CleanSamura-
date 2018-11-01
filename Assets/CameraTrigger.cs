using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {
    public float add_limite_right;
    public float add_limite_left;
    public float add_limit_up;
    public float add_limit_down;
    public bool set_limit = true;


    private FollowPlayer fp;
    private SamuraiController sc;
    private Transform t;
    private bool entered = true;
    void Start () {
        fp = GameObject.Find("Main Camera").GetComponent<FollowPlayer>();
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        t = this.GetComponent<Transform>();
    }

    private void AddLimit()
    {
        fp.limite_right += add_limite_right;
        fp.limite_left += add_limite_left;
        fp.limit_down += add_limit_down;
        fp.limit_up += add_limit_up;
    }
    private void SetLimits()
    {
        fp.limite_right = add_limite_right;
        fp.limite_left = add_limite_left;
        fp.limit_down = add_limit_down;
        fp.limit_up = add_limit_up;
    }

    private void RemoveLimit()
    {
        fp.limite_right -= add_limite_right;
        fp.limite_left -= add_limite_left;
        fp.limit_down -= add_limit_down;
        fp.limit_up -= add_limit_up;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (entered && sc.trans.position.x <= t.position.x)
            {
                if (!set_limit)
                    AddLimit();
                else
                    SetLimits();
                entered = false;
            }
            /*else if(!entered &&  sc.trans.position.x > t.position.x)
            {
                RemoveLimit();
                entered = true;
            }*/
                
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!set_limit && other.tag == "Player")
        {
            RemoveLimit();
            entered = true;
        }
    }

}
