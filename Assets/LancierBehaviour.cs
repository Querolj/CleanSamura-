using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancierBehaviour : EnnemiAI {
    public float charging_time;
    public float charge_cd;
    public Vector2 size_left_charge;
    public Vector2 offset_left_charge;
    public Vector2 size_right_charge;
    public Vector2 offset_right_charge;

    [HideInInspector]
    public bool charging = false;
    private float charge_time = 0;
    private float initial_timebt;
    private bool direction_charge;
    protected override void CustomStart()
    {
        initial_timebt = time_before_hit;

    }

    protected override bool CustomBoxCollider()
    {
        if(charging)
        {
            if(!direction)
            {
                e_collider.size = size_left_charge;
                e_collider.offset = offset_left_charge;
            }
            else
            {
                e_collider.size = size_right_charge;
                e_collider.offset = offset_right_charge;
            }
            return true;
        }
        return false;
    }

    protected override bool SpecialAttack()
    {
        if(charge_time > 0)
            charge_time -= Time.deltaTime;
        float dist = Mathf.Abs(sc.trans.position.x - e_trans.position.x);
        //print((speed <= initial_speed && in_sight));
        if (!attacking && attack_delay_timer <=0 && charge_time <= 0 && !charging && dist > view_range / 1.5 && dist <= view_range && speed <= initial_speed && in_sight && !on_blood && mods == 1)
        {
            speed = initial_speed * 1.3f;
            charging = true;
            mods = 1;
            time_before_hit = 0;
            float distance = sight_start.position.x - s_trans.position.x;
            if (distance > 0)
            {
                direction_charge = false;
            }
            else
            {
                direction_charge = true;
            }
            
            Invoke("StopCharge", charging_time);
            return true;
        }
        else if(charging)
        {
            
            if (on_blood)
            {
                speed = speed_on_blood;
                charging = false;
                time_before_hit = initial_timebt;
                charge_time = charge_cd;
                return false;
            }
            else if(states.stunned)
            {
                speed = initial_speed;
                charging = false;
                time_before_hit = initial_timebt;
                charge_time = charge_cd;
                return false;
            }
            float distance = e_trans.position.x - s_trans.position.x;
            if (in_range_for_attack || (distance < -0.15f && !direction_charge) || (distance >= 0.15f && direction_charge))
            {
                speed = initial_speed;
                BasicAttack();
                charging = false;
                time_before_hit = initial_timebt;
                charge_time = charge_cd;

            }
            //Continue tout droit
            Move(direction_charge);

            return true;
        }
        return false;
    }

    private void StopCharge()
    {
        if(charging)
        {
            charge_time = charge_cd;
            speed = initial_speed;
            BasicAttack();
            charging = false;
            time_before_hit = initial_timebt;
        }
    }
}
