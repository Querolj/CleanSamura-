using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDaemonBehaviour : EnnemiAI {
    public float jump_str = 0.1f;
    public float jump_cd;

    private float jump_time = 0;
    private bool in_air = false;
    private bool play_atter = false;
    private float init_speed;
    private bool attack_started = false;
    private bool attack_direction;
    private bool jump_back_direction;
    private bool anime_ended = true;
    private BloodCheck blood_checker;
    private bool old_attacking_anime = false;
    private PolygonCollider2D attack_collider;
    private CapsuleCollider2D attack_collider2;

    //JumpBack
    public float jump_back_str = 0.1f;
    private float jump_back_time = 0;
    private const float jb_recovery = 1;
    private float jb_recovery_time;
    private bool jb_in_air = false;
    //Simple Hit stuff
    private bool simple_hit_on = false;
    private SimpleDaemonAnimation anime;
    //Collider stuff
    public Vector2 pursuit_size_r;
    public Vector2 pursuit_offset_r;
    public Vector2 pursuit_size_l;
    public Vector2 pursuit_offset_l;

    
    protected override void CustomStart()
    {
        jb_recovery_time = jb_recovery;
        init_speed = speed;
        blood_checker = this.GetComponentInChildren<BloodCheck>();
        attack_collider = GameObject.Find(this.name + "/ColliderWhenAtk").GetComponent<PolygonCollider2D>();
        attack_collider2 = GameObject.Find(this.name + "/ColliderWhenAtk2").GetComponent<CapsuleCollider2D>();
        anime = this.GetComponent<SimpleDaemonAnimation>();
        if (attack_collider == null)
            print("attack_collider null in SimpleDaemonBehaviour");
    }


    protected override bool CustomBoxCollider()
    {
        if (mods == 1)//Change Collider
        {
            e_collider.direction = CapsuleDirection2D.Horizontal;
            if (direction)
            {
                e_collider.size = pursuit_size_r;
                e_collider.offset = pursuit_offset_r;
            }
            else
            {
                e_collider.size = pursuit_size_l;
                e_collider.offset = pursuit_offset_l;
            }
            return true;
        }
        else if(!e_collider.isActiveAndEnabled)
        {
            
        }
            
        return false;
    }
    protected override void SpecialUpdate()
    {
        if (jb_recovery_time > 0)
            jb_recovery_time -= Time.deltaTime;
        if (jump_time > 0)
        {
            SwitchCollider(false);
            Jump();
            jump_time -= Time.deltaTime;
        }
        else if(jump_back_time > 0)
        {
            JumpBack();
            jump_back_time -= Time.deltaTime;
        }

        if (in_air && blood_checker.IsOnGround() && jump_time <= 0)
        {
            SwitchCollider(true);
            in_air = false;
            play_atter = true;
            if (attack_started)
            {
                attack_started = false;
            }
        }
        if(jb_in_air && blood_checker.IsOnGround() && jump_back_time <= 0)
        {
            e_body.velocity = new Vector2(0, 0);
            jb_in_air = false;
            block_move = false;
        }
        old_attacking_anime = e_anim.attacking;
    }

    protected override bool CustomBasicAttack()
    {
        if (anime.jump_backing || jb_in_air)
            return true;
        if(!simple_hit_on)
        {
            if (in_air)
                return true;//Refuse l'attaque simple si le démon est en train de finir sa charge
            anime.SwitchHittingAnime(false);
            //Changer d'autres données...
            SwitchAttackData(false);
            simple_hit_on = true;
        }
        return false;
    }
    protected override bool SpecialAttack()
    {
        float dist = Mathf.Abs(s_trans.position.x - e_trans.position.x);

        
        if(mods == 1)
        {
            if (dist > 0.5f && dist < 1.2f &&
            !attacking && attack_delay_timer <= 0 && !in_air && anime_ended && !anime.jump_backing && !jb_in_air)
            {
                if (simple_hit_on)
                {
                    anime.SwitchHittingAnime(true);
                    //Changer d'autres données...
                    SwitchAttackData(true);
                    simple_hit_on = false;
                }
                block_move = true;
                attacking = true;
                attack_started = true;
                mods = 1; //Poursuit le joueur
                e_anim.ready_attack = true;
                Invoke("AttackEvent", time_before_hit);
                attack_delay_timer = attack_delay;
                anime_ended = false;
                return true;
            }
            else if (dist < dist_from_player && jump_back_time <= 0 && !in_range_for_attack && !in_air && !anime.jump_backing && jb_recovery_time <= 0) //Jump Back
            {
                jb_recovery_time = jb_recovery;

                block_move = true;
                jb_in_air = true;
                jump_back_direction = direction;
                jump_back_time = jump_cd;
                anime.jump_backing = true;
                return true;
            }
        }
        

        if (!anime_ended || in_air || attack_started || jump_back_time > 0)
            return true;
        return false;

    }

    protected override void AttackEvent()
    {
        base.AttackEvent();
        if(!simple_hit_on)
        {
            jump_time = jump_cd;
            in_air = true;
            attack_direction = direction;
        }
    }

    protected override bool InsteadOfBlockMove()
    {
        SpecialUpdate();
        return true;
    }

    private void Jump()
    {
        const int mult = 1;
        if(attack_direction)
        {
            e_body.AddForce(new Vector2(mult * jump_str, jump_str));
        }
        else
        {
            e_body.AddForce(new Vector2(-mult * jump_str, jump_str));
        }
    }

    private void JumpBack()
    {
        const float mult = 1;
        const float mult2 = 1;
        if (!jump_back_direction)
        {
            e_body.AddForce(new Vector2(mult * jump_back_str, mult2 * jump_back_str));
        }
        else
        {
            e_body.AddForce(new Vector2(-mult * jump_back_str, mult2 * jump_back_str));
        }
    }

    private void SwitchAttackData(bool b)//True = données de la charge
    {
        if(b)
        {
            time_before_hit *= 2;
        }
        else
        {
            time_before_hit /= 2;
        }
    }
    private void SwitchCollider(bool b)//True = collider normal
    {
        if (b)
        {
            e_collider.enabled = true;
            attack_collider.enabled = false;
            attack_collider2.enabled = false;
        }
        else
        {
            e_collider.enabled = false;
            attack_collider.enabled = true;
            attack_collider2.enabled = true;
        }
    }
    public override bool Cac()
    {
        if(simple_hit_on)
        {
            if (!(in_range_for_attack && !states.IsDead())) //Juste simple_hit_on ajouté 
            {
                return false;
            }
        }
        else
        {
            if (!anime.IsInBiteZone())
                return false;
        }
        
        bool d;
        if ((e_trans.position.x - s_trans.position.x) > 0)
            d = true;
        else
            d = false;

        if (sc.on_blood)
            sc.ReceiveDamage(1, states.strenght / 5, d, stun_power);
        else if (sc.on_ground)
            sc.ReceiveDamage(1, states.strenght, d, stun_power);
        else
        {
            sc.ReceiveDamage(1, states.strenght, d, stun_power);
        }
        return true;
    }
    //-----------------------------------------------------------------------------------------------------------------------------


    public void EndJump()//Jump charge
    {
        SetPlayAtter(false);
        block_move = false;
        TurnToPlayer();
        SetAnimeEnded(true);
    }

    public bool CanPlayAtter()
    {
        return play_atter;
    }
    public void SetPlayAtter(bool b)
    {
        play_atter = b;
    }
    public void StopBody()
    {
        e_body.velocity = new Vector2(0, 0);
    }
    public void SetAnimeEnded(bool b)
    {
        anime_ended = b;
    }
    public bool IsInAir()
    {
        return in_air;
    }


}
