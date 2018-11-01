using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour {

    MayoController mayo_controller;
    bool wall_detected = false;
	// Use this for initialization
	void Start () {
        mayo_controller = GameObject.Find("Mayo").GetComponent<MayoController>();

    }
	
    void OnTriggerStay2D(Collider2D other)
    {
        if (!wall_detected)
        {   
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("Wall"))
            {
                Staying_stuff(other, false);
            }
            else if(LayerMask.LayerToName(other.gameObject.layer).Equals("GrabWall"))
            {
                Staying_stuff(other, true);
            }
            
        }
    }

    private void Staying_stuff(Collider2D other, bool grab_wall)
    {
        bool direction_wall = false;
        if (mayo_controller.myTrans.position.x < other.gameObject.transform.position.x)
            direction_wall = true;
        mayo_controller.SetWall(other.gameObject, other.gameObject.GetComponent<SpriteRenderer>(), other.gameObject.GetComponent<Transform>(), direction_wall, grab_wall);
        wall_detected = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (wall_detected && (LayerMask.LayerToName(other.gameObject.layer).Equals("Wall") || LayerMask.LayerToName(other.gameObject.layer).Equals("GrabWall")))
        {
            mayo_controller.SetWall(null, null, null, false, false);
            wall_detected = false;
        }
    }
}
