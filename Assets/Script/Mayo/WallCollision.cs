using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision
{
    MayoController mayo_controller;
    Rigidbody2D mayo_body;
    private bool is_jumping;
    public bool has_jumped = false;
    public WallCollision(MayoController mayo_controller)
    {
        this.mayo_controller = mayo_controller;
    }

    public void update(bool is_jumping)
    {
        //Debug.Log("update wall collision");
        mayo_controller.myBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (mayo_controller.direction_wall && mayo_controller.direction > 0)
            restrainMove();
        else if (!mayo_controller.direction_wall && mayo_controller.direction < 0)
            restrainMove();
        this.is_jumping = is_jumping;
        if(mayo_controller.grabable_wall)
            grab_update();

    }
    //------------------------------------------------------------------------------------------------------------------------------------
    public void grab_update()
    {
        //Debug.Log("in grap_update");
        //float yGrabLocation, xGrabLocation;
        //yGrabLocation = (mayo_controller.wall_renderer.sprite.bounds.max.y) + mayo_controller.wall_transform.position.y;
        string wall_name = mayo_controller.wall.name;
        Vector3 tloc = GameObject.Find(wall_name+"T").transform.position;//mayo_controller.wall.GetComponentInChildren<Transform>();

        /*yGrabLocation = tloc.position.y;
        xGrabLocation = tloc.position.x;*/
        //Debug.Log("ygrab : " + yGrabLocation);
        grabing(tloc);
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    public void grabing(Vector3 GrabLocation)
    {
        float range = 0.1f;

        if((mayo_controller.myTrans.position.y < GrabLocation.y + range) && (mayo_controller.myTrans.position.y > GrabLocation.y - range) && !has_jumped)
            {
            if (is_jumping)
            {
                mayo_controller.Jump();
                mayo_controller.canJump = false;
                is_jumping = false;
                has_jumped = true;
                mayo_controller.isGrabbing = false;
            }
            else if (mayo_controller.directed != mayo_controller.direction_wall)
            {
                mayo_controller.myBody.WakeUp();
                mayo_controller.isGrabbing = false;
            }
            else if(mayo_controller.myBody.IsAwake())
            {
                Vector3 mayo_pos = mayo_controller.myTrans.position;
                mayo_pos.y = GrabLocation.y;
                mayo_controller.myTrans.position = mayo_pos;
                mayo_controller.myBody.Sleep();
                mayo_controller.isGrabbing = true;
            }
            
                
        }
    }
    public void restrainMove()
    {
        
        if (!mayo_controller.isGround)
        {
            mayo_controller.myBody.velocity = new Vector3(0, mayo_controller.myBody.velocity.y, 0);
        }      
    }
}
