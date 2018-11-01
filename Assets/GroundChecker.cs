using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour {

    private SamuraiController sc;
    private Transform t;
    void Start () {
        sc = this.GetComponentInParent<SamuraiController>();
        t = this.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        ChangeHitZone();
    }

    private void ChangeHitZone()
    {
        bool d = sc.GetDirection();
        Quaternion q = t.localRotation;
        if (d)
        {
            q.y = 180;
        }
        else
        {
            q.y = 0;
        }
        t.localRotation = q;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Surface")
            sc.on_ground = true;
        if (other.tag == "UnusualSurface")
            sc.on_unusual_ground = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Surface")
            sc.on_ground = false;
        if (other.tag == "UnusualSurface")
            sc.on_unusual_ground = false;
    }
}
