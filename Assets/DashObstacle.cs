using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashObstacle : MonoBehaviour {

    private SamuraiController sc;
    void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Spike")
            sc.can_dash = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Surface" || other.gameObject.tag == "UnusualSurface" || other.gameObject.tag == "Spike") && sc.dashing)
        {
            sc.EndDash();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike")
            sc.can_dash = true;
    }
}
