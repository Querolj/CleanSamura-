using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SamuraiController : MonoBehaviour {
    //Object propreties
    private CapsuleCollider2D s_collider;
    private SamuraiAnimator sa;
    [HideInInspector]
    public Rigidbody2D body;
    [HideInInspector]
    public Transform trans;
    private SpriteRenderer renderer;
    private GameObject hitting_zone;
    private Transform hitting_zone_trans;
    private bool directed; //true = right


    //Player stats
    public float strenght;
    public int damage;
    public int life;
    private int life_max;
    private LifeGestion life_gestion;
    public float attack_slow;
    public float jump_slow = 1f;
    public float max_speed = 5f;
    [Range(0.1f, 3f)]
    public float speed_on_blood;
    //Player afflictions
    [HideInInspector]
    public bool dead = false;
    //Knockback
    private float knock_count;
    private bool knock_dir;
    private float knock_str;
    //Estoc boost
    public float estoc_cd;
    private float estoc_time = 0;
    //Dashydash
    public float dash_str;
    public float jump_back_str;
    [HideInInspector]
    public bool dashing = false;
    [HideInInspector]
    public bool can_dash = true;
    public float dash_cd;
    private float dash_count = 0.05f;
    [HideInInspector]
    public float dash_time;
    [HideInInspector]
    public bool dash_slipped = false;
    //jump back
    [HideInInspector]
    public bool jump_back = false;
    public float jump_back_dist = 0.25f;//en réalité le temps pendant lequel on jump back
    private float jump_back_count;
    [HideInInspector]
    public bool jump_back_anime = false;
    //Blood & cleaning stuff
    [Range(0.005f, 0.5f)]
    public float cleaning_speed;
    private bool clean_button = false;
    private bool clean_button_up = false;
    [HideInInspector]
    public bool on_blood = false;
    [HideInInspector]
    public bool block_move = false;
    private bool slide_once = false;
    //New Blood Check
    private BloodCheckSamurai blood_checker;
    private Transform blood_checker_trans;
    //change speed
    private float change_speed_timer;
    private bool null_velo = false;
    //Animation
    private bool redness = false;
    private Color to_red = new Color(1,0.5f,0.5f,1);
    //Movements & block wall
    private float direction;
    public bool end_hit = true;
    public bool damage_dealt = false;
    //Wall Slide
    public float jump_wall_cd = 0.15f;
    private bool wall_sliding = false;
    private float jump_wall_timer = 0;
    private bool jump_from_wall = false;
    private bool wall_slide_direction = false;
    //wall running
    public float wall_running_speed = 2;
    private bool wall_running = false;
    //Wall edge
    private bool old_wall_sliding;

    private float old_max_speed;
    public float current_speed = 0f;
    public float acceleration = 0.1f;
    public float jumpVelocity = 10;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public bool on_ground = false;
    public bool on_unusual_ground = false;
    [HideInInspector]
    public bool can_jump = false;
    //AntiStuck
    private bool anti_stuck_on = false;

    //                       <Sound related> 
    private SamuraiSound sound;
    //Effects
    private bool is_jumping = false;
    
    //Waypoint
    private Vector3 active_waypoint;
    //SkillGestion
    private SkillGestion skill_gestion;

    //Temporary timer to respawn
    private const float respawn_cd = 3f;
    private float respawn_time = 3f;
    private bool respawn_on = false;
    private Vector2 initial_position;
    class TupleB
    {
        public List<SpriteRenderer>  lists_renderer;
        public float x;
        public TupleB(List<SpriteRenderer> lists_renderer, float x)
        {
            this.lists_renderer = lists_renderer;
            this.x = x;
        }
    }

    void Start () {

        life_max = life;
        trans = this.GetComponent<Transform>();
        active_waypoint = trans.position;
        life_gestion = GameObject.Find("GeneralSettings/LifeDisplay").GetComponent<LifeGestion>();
        sa = this.GetComponent<SamuraiAnimator>();
        jump_back_count = jump_back_dist;
        
        s_collider = this.GetComponent<CapsuleCollider2D>();
        if (s_collider == null)
            print("null");
        body = this.GetComponent<Rigidbody2D>();
        renderer = this.GetComponent<SpriteRenderer>();
        hitting_zone = GameObject.Find(this.name + "/HittingZone");
        GameObject floor_effect = GameObject.Find(this.name + "/FloorEffect");
        hitting_zone_trans = hitting_zone.GetComponent<Transform>();
        //blood_checker = this.GetComponentInChildren<BloodCheckSamurai>();
        GameObject bc= GameObject.Find(this.name + "Checker/BloodChecker").gameObject;
        blood_checker = bc.GetComponent<BloodCheckSamurai>();
        blood_checker_trans = bc.transform;
        old_max_speed = max_speed;
        skill_gestion = this.GetComponentInChildren<SkillGestion>();
        sound = this.GetComponent<SamuraiSound>();
        initial_position = trans.position;

        //Skill
        if(!skill_gestion.WallRunning)
        {
            jump_wall_cd = 0.4f;
        }
    }

    void Update () {
        if(!dead && !respawn_on)
        {
            if (dash_time >= 0)
                dash_time -= Time.deltaTime;

            if (change_speed_timer > 0)
                change_speed_timer -= Time.deltaTime;
            CheckKeyPressedUpdate();
            ChangeDirection();
            DeadCheck();
            WallSlideCheck();

            if(!wall_sliding)
                BetterJump();

            if (dashing && on_blood)
                DashSlip();
            
            if (on_blood)
                Sliding();
            else
                StopSliding();
        }
        else
        {
            if (respawn_time > 0)
                respawn_time -= Time.deltaTime;
            else
            {
                respawn_time = respawn_cd;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //trans.position = active_waypoint;
            }
        }
        if (redness)
            renderer.color = to_red;
    }


    
    private void FixedUpdate()
    {
        if (!dead)
        {
            direction = Input.GetAxisRaw("Horizontal");
            
            //Reset Wall Jump
            
            if (jump_wall_timer != 0)
                jump_wall_timer -= Time.deltaTime;
            //if (anti_stuck_on)
            //    AntiStuck();
            if (can_jump)
            {
                Jump();
                if (wall_sliding)
                    wall_sliding = false;
                can_jump = false;
            }
            else if (!KnockBack() && !block_move && !dash_slipped && jump_wall_timer <= 0 && !Input.GetButtonDown("Jump")
                && !sa.IsHitting())//!KnockBack() doit être en premier,, && !jump_from_wall
            {
                if (!Dash() && !JumpBack() && !null_velo)
                {
                    Move(direction);
                }
                else
                    print("move canceled 1");
                if (null_velo)
                    null_velo = false;
            }
            else
                print("move canceled 2");


            //Blood related
            if (clean_button && !clean_button_up)
            {
                CleanFloor();
                clean_button = false;
            }
            if (clean_button_up)
            {
                clean_button_up = false;
            }
        }
        else
        {
            //Si le joueur est mort ?
        }
    }
    private void CheckKeyPressedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.R)) //Stop Cleaning
        {
            trans.position = initial_position;
        }

        if (!can_jump && (on_ground||wall_sliding || on_unusual_ground))
        {
            can_jump = Input.GetButtonDown("Jump");
        }
        if (Input.GetKeyUp(KeyCode.A)) //Stop Cleaning
        {
            clean_button_up = true;
            block_move = false;
        }
        else if (Input.GetKey(KeyCode.A)) //Cleaning
        {
            clean_button = true;
            block_move = true;
        }
        else if (!on_blood && Input.GetKeyDown(KeyCode.LeftShift) && !dashing && !jump_back)
        {
            if(((Input.GetKey("left") || Input.GetKey("right")) || wall_sliding) && dash_time <= 0 && can_dash)
            {
                if(wall_sliding)
                {
                    wall_sliding = false;
                    body.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                sound.PlayDash();
                dashing = true;
                dash_time = dash_cd;
            }
            else if((on_ground || on_unusual_ground) && !(Input.GetKey("left") || Input.GetKey("right")))//jump back
            {
                jump_back_anime = true;
                jump_back = true;
                jump_back_count = jump_back_dist;
            }
        }
        else if (Input.GetKeyDown("left ctrl") && !dashing) //Hitting
        {
            
            ChangeSpeed(attack_slow);
            if (dashing)
                EndDash();
        }
        if(!dashing)
            WallSlideStuff();
    }
    //------------------------------------------------------------------------------------------------------------------------
    private void Move(float hInput)
    {
        //Wall Slide Block
        print("moving");
        if (!body.IsAwake())
            body.WakeUp();
        if (hInput == 0)
        {
            current_speed = 0;
            return;
        }
        Vector2 moveVel = body.velocity;
        if (current_speed + acceleration < Mathf.Abs(hInput * max_speed))
            current_speed += acceleration;
        else
            current_speed = max_speed;
        if (hInput > 0)
        {
            //Keep wall jump velocity
            if (jump_from_wall)
            {
                if (directed)
                    return;
                else
                {
                    jump_from_wall = false;
                    wall_sliding = false;
                }
            }
            directed = true;
            if (on_ground || on_unusual_ground)
                moveVel.x = current_speed;
            else
                moveVel.x = current_speed/ jump_slow;
        }
        else if (hInput < 0)
        {
            if (jump_from_wall)
            {
                if (!directed)
                    return;
                else
                {
                    jump_from_wall = false;
                    wall_sliding = false;
                }
            }
            directed = false;
            if (on_ground || on_unusual_ground)
                moveVel.x = -current_speed;
            else
                moveVel.x = -(current_speed/ jump_slow);
        }
        body.velocity = moveVel;
    }

    //------------------------------------------------------------------------------------------------------------------------
    private void WallSlideStuff()
    {
        if(sa.IsPlayingDash())
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            wall_running = false;
            wall_sliding = false;
            return;
        }

        //Cas ou on arrive au bout d'un mur
        if(!GetRedness() && old_wall_sliding && !wall_sliding && jump_wall_timer <= 0)//On vient juste de sortir d'un wall sliding
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.AddForce(new Vector2(body.velocity.x, jumpVelocity/3));
        }

        float rot = Mathf.Abs(trans.localRotation.y);
        if (wall_sliding )
        {
            if (body.velocity.y < 0.1)
                body.gravityScale = 0.5f;
            else
                body.gravityScale = 1;

            if (Input.GetKey("right") && rot == 0 ||
                Input.GetKey("left") && rot == 1)
            {
                body.constraints = RigidbodyConstraints2D.FreezeRotation;
                wall_running = false;
                wall_sliding = false;
            }
            else if (skill_gestion.WallRunning && (Input.GetKey("right") && rot == 1 || Input.GetKey("left") && rot == 0)) //(rot < 181 && rot > 179)
            {
                body.constraints = RigidbodyConstraints2D.FreezeRotation;
                wall_running = true;
                Vector2 v = new Vector2(body.velocity.x, wall_running_speed);
                body.velocity = v;
            }
            else if (skill_gestion.WallRunning && Input.GetKey("down"))
            {
                body.constraints = RigidbodyConstraints2D.FreezeRotation;
                wall_running = false;
                Vector2 v = new Vector2(body.velocity.x, -wall_running_speed);
                body.velocity = v;
            }
            else if (wall_sliding && !GetRedness())
            {
                wall_running = false;
                body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            }
            else
                body.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
        else
        {
            body.gravityScale = 1;
            wall_running = false;
        }
        old_wall_sliding = wall_sliding;
    }

    //------------------------------------------------------------------------------------------------------------------------
    private void AntiStuck()
    {
        print("wallmach");
        if ((directed && Input.GetKey("right")) | (!directed && Input.GetKey("left")))
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
        else
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void WallSlideCheck()
    {
        if (!wall_sliding && body.constraints == (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX))
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (jump_from_wall && (on_ground || on_unusual_ground))
            jump_from_wall = false;
    }

    private void Estoc()
    {
        if (estoc_time <= 0)
        {
            body.velocity = new Vector2(0, 0);
        }
        else
        {
            estoc_time = 0;
            if (directed)
            {
                body.velocity = Vector2.right * dash_str / 3;
            }
            else
            {
                body.velocity = Vector2.left * dash_str / 3;
            }
        }
    }
    private void DashSlip()
    {
        dash_count = 0;
        dashing = false;
        body.velocity = new Vector2(0, 0);
        dash_slipped = true;
    }

    private void BetterJump()
    {
        if (body.velocity.y < 0)
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (body.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    private void Sliding()
    {
        if (slide_once && on_ground)
        {
            slide_once = false;
            max_speed = speed_on_blood;
        }
    }

    private void StopSliding()
    {
        if (!slide_once)
        {
            slide_once = true;
            max_speed = old_max_speed;
        }
    }

    private bool Dash()
    {
        if(dash_count <= 0)//Fin du dash
        {
            EndDash();
        }
        else if (dashing)
        {
            if(dash_count == 0.05f)
            {
                body.velocity = new Vector2(0, 0);
            }
            if (directed)
            {
                body.velocity = Vector2.right * dash_str;
            }
            else
            {
                body.velocity = Vector2.left * dash_str;
            }
            dash_count -= Time.deltaTime;
            return true;
        }
        return false;
    }
    private bool JumpBack()
    {
        if (jump_back_count <= 0)
            EndJumpBack();
        else if(jump_back)
        {
            if (directed)
            {
                body.velocity = Vector2.left * jump_back_str;
            }
            else
            {
                body.velocity = Vector2.right * jump_back_str;
            }
            jump_back_count -= Time.deltaTime;
            return true;
        }

        return false;
    }

    private void Jump()
    {
        
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        if(!wall_sliding && (on_ground || on_unusual_ground))
        {
            //print("jump normal");
            is_jumping = true;
            body.velocity = new Vector2(0f, 0f);
            body.AddForce(new Vector2(0f, jumpVelocity));
        }else if(wall_sliding)
        {
            //print("jump wall");
            jump_wall_timer = jump_wall_cd;
            jump_from_wall = true;
            body.velocity = new Vector2(0f, 0f);
            if (wall_slide_direction)
            {
                directed = false;
                body.AddForce(new Vector2(-jumpVelocity/2, jumpVelocity));
            }
            else
            {
                directed = true;
                body.AddForce(new Vector2(jumpVelocity/2, jumpVelocity));
            }
            
        }
        
    }

    private void ChangeDirection()
    {
        //Vector3 v = hitting_zone_trans.localPosition;
        Vector3 w = blood_checker_trans.localPosition;
        if (direction > 0)
        {
            w.x = 0.085f;
        }
        else if(direction < 0)
        {
            w.x = 0;
        }
        //hitting_zone_trans.localPosition = v;
        blood_checker_trans.localPosition = w;
    }
    private void DeadCheck()
    {
        if(life <= 0)
        {
            dead = true;
            
        }
    }

    private void CleanFloor()
    {
        if (blood_checker.blood_in_range.Count != 0)
            EraseBlood();
    }
    private void EraseBlood()
    {
        foreach(SpriteRenderer sp in blood_checker.blood_in_range.ToArray())
        {
            if (sp == null)
                break;
            Color c = sp.color;
            if (c.a > 0.2f)
            {
                if (body.velocity.x > 0)
                    c.a -= cleaning_speed * 3f; //Bonus pour les slides
                else
                    c.a -= cleaning_speed;
                sp.color = c;
            }
            else
            {
                blood_checker.blood_in_range.Remove(sp);
                GameObject.Destroy(sp.gameObject);
            }

        }

    }

    private bool KnockBack()
    {
        if (knock_count > 0)
        {
            if (knock_dir)
                if (on_ground || on_unusual_ground)
                    body.velocity = Vector2.left * knock_str;
                else
                    body.velocity = Vector2.left * knock_str / 3;
            else
                if (on_ground || on_unusual_ground)
                body.velocity = Vector2.right * knock_str;
            else
                body.velocity = Vector2.right * knock_str / 3;
            knock_count -= Time.deltaTime;
            return true;
        }
        return false;
    }
    private void StopRedness()
    {
        renderer.color = new Color(1, 1, 1, 1);
        redness = false;
        block_move = false;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------
    public bool GetDirection()
    {
        return directed;
    }
    public float GetDirectionFloat()
    {
        return direction;
    }
    public CapsuleCollider2D GetCollider()
    {
        return s_collider;
    }
    
    public void EndDash()
    {
        dash_count = 0.05f;
        dashing = false;
        body.velocity = new Vector2(0, 0);
    }
    public void EndJumpBack()
    {
        jump_back_count = jump_back_dist;
        jump_back = false;
        body.velocity = new Vector2(0, 0);
    }
    public Vector2 GetOffsetCollider()//Celui qui n'est pas un trigger
    {
        return s_collider.offset;
    }
    public Vector2 GetSizeCollider()//Celui qui n'est pas un trigger
    {
        return s_collider.size;
    }
    public void SetVelocity(float v)
    {
        Vector2 vec = new Vector2(v, 0);
        body.velocity = vec;
    }
    public void ChangeSpeed(float v, float t = 0.1f)
    {
        max_speed = v;
        Invoke("ChangeSpeedTimer", t);

    }
    private void ChangeSpeedTimer()
    {
        if(!on_blood)
            max_speed = old_max_speed;

    }
    public void ReceiveDamage(int damage, float strenght, bool d, float stun_power = 0.2f) // strenght = force de répulsion
    {
        if(knock_count <= 0)
        {
            sound.PlayHurt();
            life -= damage;
            life_gestion.UpdateLifeDisplayDmg(life, damage);
            redness = true;
            block_move = true;
            Invoke("StopRedness", stun_power);
            knock_count = stun_power;
            knock_dir = d;
            knock_str = strenght;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (dashing)//stop le dash
            {
                EndDash();
            }
            if(wall_sliding)
            {
                wall_sliding = false;
                
            }
        }
        
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    
    public bool GetRedness()
    {
        return redness;
    }

    public void SetCollidersOffset(float o)
    {
        Vector2 v = s_collider.offset;
        v.x = o;
        s_collider.offset = v;     
    }
    public void AddLife(int life)
    {
        if (this.life + life > this.life_max)
        {
            this.life = life_max;
        }
        else
            this.life += life;
    }

    public void SetRespawn(bool b)
    {
        respawn_on = b;
    }
    public void RewindEstocTime()
    {
        estoc_time = estoc_cd;
    }
    public void NullVelocity()
    {
        body.velocity = new Vector2(0, 0);
        null_velo = true;
    }
    public void SetWaypoint(Vector3 way)
    {
        active_waypoint = way;
    }
    public void SetWallSlide(bool b)
    {
        wall_sliding = b;
        wall_slide_direction = directed;
    }
    public bool IsWallSliding()
    {
        return wall_sliding;
    }
    public bool IsWallRunning()
    {
        return wall_running;
    }
    public void SetFriction(float n_friction)
    {
        s_collider.sharedMaterial.friction = n_friction;
    }

    public float GetWallJumpTime()
    {
        return jump_wall_timer;
    }
    public bool IsJumping()
    {
        return is_jumping;
    }
    public void SetIsJumping(bool b)
    {
        is_jumping = b;
    }
    public bool AntiStuckOn
    {
        get { return anti_stuck_on; }
        set { anti_stuck_on = value; }
    }
    public bool WallSlideDirection
    {
        get { return wall_slide_direction; }
        set { wall_slide_direction = value; }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------
    /*void OnCollisionEnter2D(Collision2D other)
    {
        //print(body.sharedMaterial.friction);
        if (other.gameObject.tag == "Surface" && dashing)
        {
            print("in");
            EndDash();
        }
        
    }*/

}
