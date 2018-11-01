using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiSlide : MonoBehaviour {
    private SamuraiController sc;
    private float base_friction;
    private Rigidbody2D sc_body;
    private CapsuleCollider2D sc_caps;
    void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        sc_body = GameObject.Find("Samuraï").GetComponent<Rigidbody2D>();
        sc_caps = GameObject.Find("Samuraï").GetComponent<CapsuleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print(sc_body.sharedMaterial.friction);
        if ( other.tag == "Surface" && !sc.on_ground)
        {
            print("in");
            //other.sharedMaterial.friction = 0;
            sc_caps.sharedMaterial.friction = 0;
            sc_body.sharedMaterial.friction = 0;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Surface" )
        {
            print("out");
            //other.sharedMaterial.friction = 0.4f;
            sc_caps.sharedMaterial.friction = 5;
            sc_body.sharedMaterial.friction = 5;
        }

    }
}
