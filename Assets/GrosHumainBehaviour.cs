using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrosHumainBehaviour : EnnemiAI{

    public float time_before_strong_hit;
    public float time_before_punch;
    public float strong_hit_strenght;


    public Vector2 offset_hit_right;
    public Vector2 size_hit_right;
    public Vector2 offset_punch_right;
    public Vector2 size_punch_right;

    public Vector2 offset_hit_left;
    public Vector2 size_hit_left;
    public Vector2 offset_punch_left;
    public Vector2 size_punch_left;


    [HideInInspector]
    public bool special_hit_launched = false;
    [HideInInspector]
    public bool punch_launched = false;
    [HideInInspector]
    public bool punch_range;
    private HittingZoneGrosHumain hz;
    private float initial_time_before_hit;
    private float initial_str;
    private GrosHumainAnimation gros_anime;
    protected override void CustomStart()
    {
        hz = this.GetComponentInChildren<HittingZoneGrosHumain>();
        initial_time_before_hit = time_before_hit;
        initial_str = states.strenght;
        gros_anime = this.GetComponent<GrosHumainAnimation>();
    }
    protected override bool CustomBoxCollider()
    {
        if(punch_launched)
        {
            if(direction)
            {
                e_collider.offset = offset_punch_right;
                e_collider.size = size_punch_right;
            }
            else
            {
                e_collider.offset = offset_punch_left;
                e_collider.size = size_punch_left;
            }
            return true;
        }
        else if(special_hit_launched)
        {
            if (direction)
            {
                e_collider.offset = offset_hit_right;
                e_collider.size = size_hit_right;
            }
            else
            {
                e_collider.offset = offset_hit_left;
                e_collider.size = size_hit_left;
            }
            return true;
        }
        return false;
    }
    protected override bool SpecialAttack()
    {
        if (in_range_for_attack && !attacking && attack_delay_timer <= 0)
        {
            if (punch_range) { 

                time_before_hit = time_before_punch;
                punch_launched = true;

                BasicAttack();
                return true;
            }
            else  
                return false;
            
        }

        return false;
    }

    protected override void RebootingSpecAtk()
    {
        RebootPunch();
        RebootStrongAttack();
    }

    public void RebootPunch()
    {
        punch_launched = false;
        time_before_hit = initial_time_before_hit;
        hz.RebootHitZone();
    }
    public void RebootStrongAttack()
    {
        time_before_hit = initial_time_before_hit;
        special_hit_launched = false;
        states.strenght = initial_str;
        e_anim.sprite_ready = 0;
        gros_anime.ToBasicHitAnime();
        hz.RebootHitZone();
    }

    
}
