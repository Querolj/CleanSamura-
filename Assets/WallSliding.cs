using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSliding : MonoBehaviour {
    private SamuraiController sc;
    private SamuraiAnimator sa;
    private Transform t;
	void Start () {
        sc = this.GetComponentInParent<SamuraiController>();
        sa = this.GetComponentInParent<SamuraiAnimator>();
        t = this.GetComponent<Transform>();
    }
	
	void Update () {

        if(sc.on_ground)
            sc.SetWallSlide(false);
        if (sc.IsWallSliding())
            WallSlide();
        if((sc.on_unusual_ground || sc.on_ground ) && sc.IsWallSliding())
            sc.SetWallSlide(false);
    }

    private void WallSlide()
    {
        //sc.body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Surface"  && !sc.on_ground && !sc.dashing)
        {
            sc.SetWallSlide(true);
            sc.WallSlideDirection = sc.GetDirection();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.tag == "Surface" || other.tag == "UnusualSurface")&& !sc.on_ground)
        {
            sc.SetWallSlide(false);
            
        }
    }
}
