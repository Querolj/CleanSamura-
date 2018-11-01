using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCarrying
{
    MayoController mayo_controller;
    bool is_carrying = false;
    Transform carried_object_trans;
    Rigidbody2D carried_object_rb;
    bool direction;
    private int force = 500;

    public ObjectCarrying(MayoController mayo_controller)
    {
        this.mayo_controller = mayo_controller;
    }

    public void update(bool take, bool direction, bool launch)
    {
        this.direction = direction;
        if (is_carrying)
        {
            
            if (take)
                release_object();
            else
                carrying();
        }
        else if (take && !is_carrying&& mayo_controller.object_locked != null)
        {
            //mayo_controller.object_locked.layer = LayerMask.NameToLayer("Player");
            //carried_object = mayo_controller.object_locked;
            carried_object_trans = mayo_controller.object_locked_trans;
            carried_object_rb = mayo_controller.object_locked_rb;
            is_carrying = true;     
        }
        if (is_carrying && launch)
            throw_object();

    }

    public void carrying()
    {
        float distance_from_mayo = 0.30f;
        Vector3 vec;
        if(!direction)
            vec = mayo_controller.myTrans.position + Vector3.left * distance_from_mayo;
        else
            vec = mayo_controller.myTrans.position + Vector3.right * distance_from_mayo;
        vec.y -= 0.1f;
        carried_object_trans.position = vec;
        carried_object_rb.simulated = false;
    }

    public void release_object()
    {
        is_carrying = false;
        carried_object_rb.simulated = true;
        //carried_object.layer = LayerMask.NameToLayer("PortableObjectS");
    }
    public void throw_object()
    {
        release_object();
        if(mayo_controller.directed)
        {
            carried_object_rb.AddForce(force * Vector2.right);
        }
        else
        {
            carried_object_rb.AddForce(force * Vector2.left);
        }
        
        mayo_controller.launch = false;
    }
}
