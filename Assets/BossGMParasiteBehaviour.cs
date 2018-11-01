using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGMParasiteBehaviour : EnnemiAI {
    public Vector2 charging_right_off;
    public Vector2 charging_right_size;
    public Vector2 charging_left_off;
    public Vector2 charging_left_size;
    public float charge_cd;
    public float gerbe_cd;

    public bool charging = false;
    public bool gerbing = false;

    private float charge_time = 0;
    private float gerbe_time = 0;
    private const float distance_charge = 1f;
    private bool direction_charge;
    private BossGMParasiteAnimation anime;
    private Vector2 or_offset;
    private Vector2 or_size;
    protected override void CustomStart()
    {
        anime = this.GetComponent<BossGMParasiteAnimation>();
        charge_time = charge_cd;
        or_offset = e_collider.offset;
        or_size = e_collider.size;
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------



    protected override bool SpecialAttack()
    {
        AllCD();

        float dist = Mathf.Abs(sc.trans.position.x - e_trans.position.x);
        if(!attacking && attack_delay_timer <= 0 && !charging && dist > distance_charge && in_sight && charge_time <= 0 && !gerbing)
        {
            speed = initial_speed * 2.2f;
            charging = true;
            mods = 1;
            //time_before_hit = 0;
            float distance = sight_start.position.x - s_trans.position.x;
            if (distance > 0)
            {
                direction_charge = false;
            }
            else
            {
                direction_charge = true;
            }
        }
        if(charging)
        {
            if (!direction_charge)
            {
                Move(false);
            }
            else
            {
                Move(true);
            }
            return true;
        }
        else
        {
            if (in_range_for_attack && !attacking && attack_delay_timer <= 0 && !charging && in_sight && !gerbing)
            {
                if (gerbe_time <= 0)
                {
                    int choosen_attack = Random.Range(1, 11);
                    if (choosen_attack <= 5 && !gerbing)
                    {
                        return false;
                    }
                    else//Attaque gerbe
                    {
                        gerbing = true;
                        return true;
                    }
                }
                else
                    return false;
                
            }
                
        }
        return false;
    }

    private void AllCD()
    {
        float f = Time.deltaTime;
        if (charge_time > 0)
            charge_time -= f;
        if (gerbe_time > 0)
            gerbe_time -= f;
    }


    public void StopCharge()
    {
        if(charging)
        {
            speed = initial_speed;
            charge_time = charge_cd;
            charging = false;
            anime.StopChargeAnime();
            e_collider.offset = or_offset;
            e_collider.size = or_size;
        }
    }
    public void StopGerbe()
    {
        gerbe_time = gerbe_cd;
        gerbing = false;
    }

    protected override bool CustomBasicAttack()
    {
        if (charging)
            return true;
        return false;
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    protected override bool CustomBoxCollider()
    {
        if(charging)
        {
            e_collider.direction = CapsuleDirection2D.Horizontal;
            if(!direction)
            {
                //sight_start = new Vector2(-0.076f, 0.328f);
                Vector2 v_size = e_collider.size;
                Vector2 v_offset = e_collider.offset;
                v_size = charging_left_size;
                v_offset = charging_left_off;
                e_collider.size = v_size;
                e_collider.offset = v_offset;
            }
            else
            {
                Vector2 v_size = e_collider.size;
                Vector2 v_offset = e_collider.offset;
                v_size = charging_right_size;
                v_offset = charging_right_off;
                e_collider.size = v_size;
                e_collider.offset = v_offset;

            }
            return true;
        }
        return false;
    }
    
    protected override bool CustomSight(ref Vector3 position_range)
    {
        
        return false;
    }
}
