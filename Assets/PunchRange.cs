using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchRange : MonoBehaviour {
    public float off_x_right;
    public float size_x_right;
    public float off_x_left;
    public float size_x_left;

    private GrosHumainBehaviour ghb;
    private bool old_direction = false;
    private BoxCollider2D trigger;
    // Use this for initialization
    void Start () {
        ghb = this.GetComponentInParent<GrosHumainBehaviour>();
        trigger = this.GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
		if(old_direction != ghb.direction && ghb.direction)
        {
            Vector2 off = trigger.offset;
            Vector2 size = trigger.size;
            off.x = off_x_right;
            size.x = size_x_right;

            trigger.offset = off;
            trigger.size = size;
        }
        else if(old_direction != ghb.direction)
        {
            Vector2 off = trigger.offset;
            Vector2 size = trigger.size;
            off.x = off_x_left;
            size.x = size_x_left;

            trigger.offset = off;
            trigger.size = size;
        }
        old_direction = ghb.direction;

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.gameObject.tag == "Player")
        {
            ghb.punch_range = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ghb.punch_range = false;
        }
    }
}
