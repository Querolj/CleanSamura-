using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCamLimit : MonoBehaviour {
    public float limit_right = 999;
    public float limit_left = 999;
    public float limit_down = 999;
    public float limit_up = 999;
    private FollowPlayer fp;
    void Start () {
        fp = GameObject.Find("Main Camera").GetComponentInParent<FollowPlayer>();
        if (fp == null)
            print("fp null in AddCamLimit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            fp.limit_up = this.limit_up;
            fp.limit_down = this.limit_down;
            fp.limite_right = this.limit_right;
            fp.limite_left = this.limit_left;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            fp.limit_up = 999;
            fp.limit_down = -999;
            fp.limite_right = 999;
            fp.limite_left = -999;
        }
    }
}
