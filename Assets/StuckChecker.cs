using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckChecker : MonoBehaviour {

    private SamuraiController sc;
    
    void Start()
    {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Surface" || other.gameObject.tag == "UnusualSurface")
        {
            //sc.block_move = true;
            /*
            Vector3 v = sc.body.velocity;
            v.x = 0;
            sc.body.velocity = v;
            */
            print("opla");
            sc.AntiStuckOn = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Surface" || other.gameObject.tag == "UnusualSurface")
        {
            sc.block_move = false;
            sc.AntiStuckOn = false;
        }
    }
}
