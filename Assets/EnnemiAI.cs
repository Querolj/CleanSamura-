using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiAI : MonoBehaviour {
    // Ennemi propreties
    protected EnnemiAnimation e_anim;
    protected ExpressionAnimation expr;
    protected EnnemiStates states;
    protected Rigidbody2D e_body;
    protected CapsuleCollider2D e_collider;
    protected Transform e_trans;
    protected Transform s_trans;
    protected Transform sight_start;
    protected SamuraiController sc;
    protected Cleaning cleaning;
    //Ranged stuff
    public bool is_ranged = false;
    public Transform throw_point;
    public GameObject projectile;
    private GameObject proj_to_trash;
    //Behaviour
    public bool coward = false;
    public bool follow_player = true;
    private bool turn = false;
    private bool out_int = false;
    //View & mods
    public float view_range;
    public float pursuit_range;
    public float pursuit_range_y = 0f;
    public bool is_static = false;
    [Space]
    public float left_sight_x = -0.169f;
    public float right_sight_x = 0.136f;
    protected bool in_sight = false;
    public int mods = 0; // 0= waiting, 1= poursuite, 2= patrouille, 3=special, dépend du type
    public float dist_from_player = 0.2f;
    private int old_mod;
    [HideInInspector]
    public bool block_move = false;
    //box collider direction
    public float x_offset_right=0f;
    public float x_size_right = 0f;
    public float x_offset_left = 0f;
    public float x_size_left = 0f;
    //Waypoints
    public Transform[] waypoints;
    private int current_waypoint = 0;

    //Movements
    public float speed = 5f;
    public float speed_on_blood = 0f;
    protected float initial_speed;
    public bool direction = false;
    public bool stick_to_direction = false;
    protected bool old_direction = false;
    //Limits, relative to ennemi world pos
    public float limit_l;
    public float limit_r;
    public bool active_limits = false;
    public bool dont_move = false;
    private bool initial_direction;
    private bool look_at_wall;
    [Space]
    //Attacks
    public float time_before_hit;
    public float stun_power = 0.2f;
    public float attack_delay; // time between each attacks
    public float range_for_player = -0.24f;
    [HideInInspector]
    public bool in_range_for_attack = false;
    [HideInInspector]
    public bool attacking = false;
    //protected bool attack_delayed = false;
    protected float attack_delay_timer;
    //Blood
    [HideInInspector]
    public bool on_blood = false;
    protected bool old_on_blood = false;
    //Type d'ennemi avec attaque spé
    public string type="";//lancier, ...
    //Modularité pour différents types d'ennemis
    protected bool special_attack_behaviour = false;

    protected virtual void Start () {
        if (dont_move)
            initial_direction = direction;
        if (is_static && mods != 0)
            mods = 0;
        attack_delay_timer = attack_delay;
        initial_speed = speed;
        GameObject s = GameObject.Find("Samuraï");

        s_trans = s.GetComponent<Transform>();
        e_trans = this.GetComponent<Transform>();
        e_body = this.GetComponent<Rigidbody2D>();
        e_collider = this.GetComponent<CapsuleCollider2D>();
        e_anim = this.GetComponent<EnnemiAnimation>();
        if (e_anim == null)
            print("start null");
        sight_start = GameObject.Find(this.name+"/SightStart").transform;
        expr = GameObject.Find(this.name + "/Expression").GetComponent<ExpressionAnimation>();
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        states = this.GetComponent<EnnemiStates>();
        Vector3 position_range = sight_start.position;
        SetPointZoneDirection(ref position_range);
        CustomStart();
    }
	
	void Update () {
        if (states.IsDead())
        {
            OnDeath();
        }
        else
        {
            SpecialUpdate();
            if (attack_delay_timer > 0)
                attack_delay_timer -= Time.deltaTime;
            //if (time_before_hit_timer > 0)
            //    time_before_hit_timer -= Time.deltaTime;
        }

        Vector3 position_range = sight_start.position;
        RaycastHit2D[] ray;
        if (!block_move && !states.IsDead())
        {
            SetPointZoneDirection(ref position_range);
            
            var mask =  1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Surface");
            ray = Physics2D.LinecastAll(sight_start.position, position_range, mask);

            if (ray.Length > 0  && (ray[0].transform.gameObject.layer == LayerMask.NameToLayer("Player")))
            {
                if(follow_player)
                    mods = 1;
                in_sight = true;
            }
            else
                in_sight = false;
            ModsAndAttack();
        }
        else if(block_move)
        {
            if(!InsteadOfBlockMove())
                e_body.velocity = new Vector2(0, 0);
        }
        
    }


    private void SetPointZoneDirection(ref Vector3 position_range)
    {
        if (!direction) 
        {
            //Sight
            if(!CustomSight(ref position_range))
            {
                Vector2 v = sight_start.localPosition;
                v.x = left_sight_x;

                sight_start.localPosition = v;
                position_range.x += -1 * view_range;
            }
            
            Debug.DrawLine(sight_start.position, position_range, Color.red);

            //Throw point
            if(is_ranged)
            {
                Vector2 w = throw_point.localPosition;
                w.x = -0.135f;
                throw_point.localPosition = w;
            }
            //box collider
            if(!CustomBoxCollider() && (x_offset_left != 0 || x_size_left != 0))
            {
                if (!e_collider.isActiveAndEnabled)
                {
                    e_collider.enabled = true;
                }
                e_collider.direction = CapsuleDirection2D.Vertical;
                Vector2 v_size = e_collider.size;
                Vector2 v_offset = e_collider.offset;
                v_size.x = x_size_left;
                v_offset.x = x_offset_left;
                e_collider.size = v_size;
                e_collider.offset = v_offset;
            }
        }
        else
        {
            //Sight
            if (!CustomSight(ref position_range))
            {
                Vector2 v = sight_start.localPosition;
                v.x = right_sight_x;

                sight_start.localPosition = v;
                position_range.x += view_range;
            }
                
            Debug.DrawLine(sight_start.position, position_range, Color.red);

            //Throw point
            if (is_ranged)
            {
                Vector2 w = throw_point.localPosition;
                w.x = 0.135f;
                throw_point.localPosition = w;
            }
            //box collider
            if (!CustomBoxCollider() && (x_offset_right != 0 || x_size_right != 0))
            {
                if (!e_collider.isActiveAndEnabled)
                {
                    e_collider.enabled = true;
                }
                e_collider.direction = CapsuleDirection2D.Vertical;
                Vector2 v_size = e_collider.size;
                Vector2 v_offset = e_collider.offset;
                v_size.x = x_size_right;
                v_offset.x = x_offset_right;
                e_collider.size = v_size;
                e_collider.offset = v_offset;
            }
        }
        old_direction = direction;
    }

    protected virtual void BasicAttack()
    {
        attacking = true;
        mods = 1; //Poursuit le joueur
        e_anim.ready_attack = true;
        block_move = true;
        Invoke("AttackEvent", time_before_hit);
        //time_before_hit_timer = time_before_hit;
        attack_delay_timer = attack_delay;
        //Invoke("AttackReadyEvent", attack_delay);// + time_before_hit?
    }

    private bool CheckRangeY()
    {
        if (pursuit_range_y == 0)
            return true;
        float distance = Mathf.Abs(e_trans.position.y - s_trans.position.y);
        if(distance < pursuit_range_y)
        {
            return true;
        }
        return false;
    }
    private void ModsAndAttack()
    {
        if(!states.is_daemon)
        {
            if (on_blood && !old_on_blood)
                speed = speed_on_blood;
            if (old_on_blood && !on_blood)
                speed = initial_speed;
        }
        
        
        if (special_attack_behaviour)//Empêche attaque classique
        {
            InsteadOfAttack();
        }

        if (SpecialAttack())
        {

        }
        else if (!follow_player && waypoints.Length < 1)
        {
            Patrol();
        }
        else if (in_range_for_attack && !attacking && attack_delay_timer <= 0 && !CustomBasicAttack())
        {
            BasicAttack();
        }
        else if (mods == 1 && follow_player)
        {
            if (old_mod != mods)
                expr.play_spotted = true;

            float distance = e_trans.position.x - s_trans.position.x;
            if (Mathf.Abs(distance) <= pursuit_range && CheckRangeY())
            {
                if (distance > dist_from_player)
                {
                    out_int = false;
                    if (!coward)
                        Move(false);
                    else if (attack_delay_timer > 0)
                    {
                        Move(true);
                    }
                    else
                    {
                        Move(false);
                    }
                }
                else if (distance < -dist_from_player)
                {
                    out_int = false;
                    if (!coward)
                        Move(true);
                    else if (attack_delay_timer > 0)
                    {
                        Move(false);
                    }
                    else
                    {
                        Move(true);
                    }
                }
                else
                {
                    if (!out_int)
                    {
                        out_int = true;
                        if (distance >= 0)
                            turn = false;
                        else
                            turn = true;
                    }
                    Move(turn);
                }
            }
            else if (!is_static || waypoints.Length == 0)
                mods = 2;
            else
                mods = 0;
        }
        else if (mods == 2 && !is_static)
        {
            Patrol();
        }
        old_mod = mods;
        old_on_blood = on_blood;
    }

    void SpecialBehaviour()
    {
        switch(type)
        {
            case "lancier":

                break;
            default:
                print("type not found");
                break;

        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    protected virtual void AttackEvent()
    {
        
        if (e_anim.ready_attack)
        {
            e_anim.ready_attack = false;
            e_anim.attacking = true;
            
        }
        attacking = false;
    }

    public void AttackType(bool d)
    {
        if(!is_ranged)
            Cac();
        else //throw projectile
            Ranged();
    }

    public virtual bool Cac() //vrai si le coup a infligé des dommages
    {
        if (!(in_range_for_attack && !states.IsDead()))
        {
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
    public void Ranged()
    {
        if (proj_to_trash != null)
            GameObject.Destroy(proj_to_trash);
        proj_to_trash = Instantiate(projectile, throw_point.position, Quaternion.identity);
        Rigidbody2D rb = proj_to_trash.GetComponent<Rigidbody2D>();
        Vector2 v;

        bool d;
        if(!dont_move)
        {
            if ((e_trans.position.x - s_trans.position.x) > 0)
                d = true;
            else
                d = false;
        }
        else
        {
            d = !initial_direction;
        }

        if (d)//droite
        {
            v = new Vector2(Vector2.left.x, Vector2.up.y);
            rb.AddForce(v * states.throwing_strenght);
        }
        else
        {
            v = new Vector2(Vector2.right.x, Vector2.up.y);
            rb.AddForce(v * states.throwing_strenght);
        }
    }

    
    //--------------------------------------------------------------------------------------------------------------------------------------------
    private void Patrol()
    {
        float diff;
        if (current_waypoint >= waypoints.Length)
            current_waypoint = 0;
        diff = waypoints[current_waypoint].position.x - e_trans.position.x;

        if (diff < 0.1 && diff > -0.1)
        {
            if (waypoints.Length > current_waypoint)
            {
                current_waypoint++;
            }
            else
            {
                current_waypoint = 0;
            }
        }
        else if (diff > 0)
            Move(true);
        else
            Move(false);
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------
    protected void Move(bool right)
    {
        if(!dont_move)
        {
            Vector2 moveVel = e_body.velocity;
            if (right && !Limits(true))
            {
                direction = true;
                moveVel.x = speed;
            }
            else if (!Limits(false))
            {
                direction = false;
                moveVel.x = -1 * speed;
            }
            e_body.velocity = moveVel;
        }
    }
    private bool Limits(bool d)
    {
        if (active_limits)
        {
            if(d && e_trans.position.x >= limit_r)
            {
                return true;
            }
            else if(!d && e_trans.position.x <= limit_l)
            {
                return true;
            }
        }
        return false;
    }
    
    //--------------------------------------------------------------------------------------------------------------------------------------------
    public void ResetAttack()
    {
        if(!ResetAttackCustom())
        {
            attacking = false;
            if (e_anim == null)
                e_anim = this.GetComponent<EnnemiAnimation>();
            if (e_anim.ready_attack)
                e_anim.ready_attack = false;
            RebootingSpecAtk();
        }
    }
    public bool PlayerInSight()
    {
        return in_sight;
    }
    public bool GetDirection()
    {
        return direction;
    }
    public CapsuleCollider2D GetCollider()
    {
        return e_collider;
    }
    public int GetMod()
    {
        return mods;
    }
    public void SetBlockMove(bool block)
    {
        block_move = block;
    }
    public bool GetBlockMove()
    {
        return block_move;
    }
    public void SetMod(int m)//1 = poursuite
    {
        mods = m;
    }
    public SamuraiController GetSc()
    {
        return sc;
    }
    public Rigidbody2D GetBody()
    {
        return e_body;
    }
    public float GetStrenght()
    {
        return states.strenght;
    }
    public void SetLookAtWall(bool b)
    {
        look_at_wall = b;
    }
    public bool IsLookingAtWall()
    {
        return look_at_wall;
    }
    public void TurnToPlayer()
    {
        if(e_trans.position.x - s_trans.position.x > 0)
        {
            direction = false;
        }
        else
        {
            direction = true;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    protected virtual void CustomStart()
    {

    }

    protected virtual bool ResetAttackCustom()
    {
        return false;
    }
    protected virtual bool CustomSight(ref Vector3 position_range)
    {
        return false;
    }

    protected virtual bool SpecialAttack()//True si l'attaque spécial a lieu
    {
        return false;
    }
    protected virtual bool CustomBasicAttack()
    {
        return false;
    }
    protected virtual void SpecialUpdate() //Se déclenche si !blockmove
    {

    }

    protected virtual void InsteadOfAttack()
    {

    }

    protected virtual bool InsteadOfBlockMove()
    {
        return false;
    }

    protected virtual bool CustomBoxCollider() //Change la hitbox si true
    {
        return false;
    }

    protected virtual void RebootingSpecAtk()
    {
        
    }

    protected virtual void MoreAttackEvent()
    {
        
    }

    protected virtual void OnDeath()
    {
        expr.dead = true;
    }
    public Rigidbody2D EBody
    {
        get { return e_body; }
        set { e_body = value; }
    }
}
