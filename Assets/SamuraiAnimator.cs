using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiAnimator : MonoBehaviour {
    public Sprite[] spritesStanding;
    public Sprite[] spritesWalking;
    public Sprite[] spritesHitting;
    public Sprite[] spritesEstoc;
    public Sprite[] spritesHittingJump;
    public Sprite[] spritesCleaning;
    public Sprite[] spritesCleaningBlood;
    public Sprite[] spritesJumping;
    public Sprite[] spritesJumpBack;
    public Sprite spritesHurt;
    public Sprite[] spritesDying;
    public Sprite[] spritesSliding;
    public Sprite[] spritesDashing;
    public Sprite[] spritesDashSlip;
    public Sprite[] spriteWallSlide;
    public Sprite[] spriteWallRepulse;
    public Sprite[] spritesWallRunning;

    private SpriteRenderer spriteRenderer;
    private SamuraiController sc;
    public float fps_walking;
    public float fps_standing;
    public float fps_hitting;
    public float fps_dying;
    public float fps_cleaning;
    public float fps_sliding;
    public float fps_dashing;
    public float fps_dash_slip;
    public float fps_jump_back;
    public float fps_wall_slide;
    public float fps_wall_running;

    //Bool & count d'animation
    private bool hitting = false;
    private bool estoc = false;
    private bool rewind_estoc = true;
    private bool play_dash = false;
    private bool play_dash_slip = false;
    private int count_hit = 0;
    private int count_estoc = 0;
    private int count_dash = 0;
    private int count_dash_slip = 0;
    private int count_jump_back = 0;
    private int count_anime_dead = 0;
    private int old_index_hit = -1;
    private int old_index_estoc = -1;
    private int old_index_dash = -1;
    private int old_index_dash_slip = -1;
    private int old_index_jb = -1;
    private int old_index_death = -1;
    //Coups spéciaux
    private const float hit_cd = 0.01f;
    private float hit_time;
    private const float estoc_cd = 0.1f;
    private float estoc_time;
    //Dégats synchro
    private HittingScript hs;

    // Use this for initialization
    void Start () {
        hit_time = 0;
        estoc_time = 0;
        spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        hs = GameObject.Find("Samuraï/HittingZone").GetComponent<HittingScript>();

    }
	
	// Update is called once per frame
	void Update () {
        int index_dying = (int)(Time.timeSinceLevelLoad * fps_dying);
        

        if(sc.dead)
        {
            if(count_anime_dead < spritesDying.Length)
            {
                index_dying = index_dying % spritesDying.Length;
                if(old_index_death != index_dying)
                {
                    spriteRenderer.sprite = spritesDying[count_anime_dead];
                    count_anime_dead++;
                }

                old_index_death = index_dying;
            }
        }
        if(!sc.dead)
        {
            NormalMove();
        }
        
    }

    private void NormalMove()
    {
        int index_walking = (int)(Time.timeSinceLevelLoad * fps_walking);
        int index_standing = (int)(Time.timeSinceLevelLoad * fps_standing);
        int index_hitting = (int)(Time.timeSinceLevelLoad * fps_hitting);
        int index_cleaning = (int)(Time.timeSinceLevelLoad * fps_cleaning);
        int index_sliding = (int)(Time.timeSinceLevelLoad * fps_sliding);
        int index_dashing = (int)(Time.timeSinceLevelLoad * fps_dashing);
        int index_dash_slip = (int)(Time.timeSinceLevelLoad * fps_dash_slip);

        if (hit_time > 0)
            hit_time -= Time.deltaTime;
        if (estoc_time > 0)
            estoc_time -= Time.deltaTime;

        //if (!sc.block_move && !sc.dashing)
        //    FlipRenderer();
        if(sc.GetRedness())
        {
            spriteRenderer.sprite = spritesHurt;
            if(hs.damage_dealt)
            {
                hitting = false;
                
                CancelAttack();
            }
        }
        else if(sc.dash_slipped || play_dash_slip)
        {
            index_dash_slip= index_dash_slip % spritesDashSlip.Length;
            if (count_dash_slip < spritesDashSlip.Length)
            {
                play_dash_slip = true;
                if (old_index_dash_slip != index_dash_slip)
                {
                    spriteRenderer.sprite = spritesDashSlip[count_dash_slip];
                    count_dash_slip++;
                }
            }
            else
            {
                play_dash_slip = false;
                count_dash_slip = 0;
                sc.dash_slipped = false;
            }
            old_index_dash_slip = index_dash_slip;
        }
        else if(Input.GetKey(KeyCode.A) && (sc.on_ground || sc.on_unusual_ground))
        {
            if(sc.on_blood)
            {
                index_cleaning = index_cleaning % spritesCleaningBlood.Length;
                spriteRenderer.sprite = spritesCleaningBlood[index_cleaning];
            }
            else
            {
                index_cleaning = index_cleaning % spritesCleaning.Length;
                spriteRenderer.sprite = spritesCleaning[index_cleaning];
            }
            
        }
        else if(sc.GetWallJumpTime() > 0)
        {
            
            int index_wallsliding = (int)(Time.timeSinceLevelLoad * fps_wall_slide);
            index_wallsliding = index_wallsliding % spriteWallRepulse.Length;
            spriteRenderer.sprite = spriteWallRepulse[index_wallsliding];
        }
        else if(sc.IsWallRunning())
        {
            int index_wallrunning = (int)(Time.timeSinceLevelLoad * fps_wall_running);
            index_wallrunning = index_wallrunning % spritesWallRunning.Length;
            spriteRenderer.sprite = spritesWallRunning[index_wallrunning];
        }
        else if(sc.IsWallSliding())
        {
            //float rot = Mathf.Abs(sc.trans.localRotation.y);
            int index_wallsliding = (int)(Time.timeSinceLevelLoad * fps_wall_slide);
            index_wallsliding = index_wallsliding % spriteWallSlide.Length;
            spriteRenderer.sprite = spriteWallSlide[index_wallsliding];
         
        }
        else if(sc.on_blood)
        {
            if (hs.damage_dealt )
            {
                CancelAttack();
            }
            index_sliding = index_sliding % spritesSliding.Length;
            spriteRenderer.sprite = spritesSliding[index_sliding];
            if(sc.dashing)
            {
                play_dash = false;
                count_dash = 0;
            }
            if(hitting)
            {
                hitting = false;
                old_index_hit = -1;
                count_hit = 0;
            }
        }
        else if(sc.dashing || play_dash)
        {
            if (hs.damage_dealt)
            {
                CancelAttack();
            }
            if (Input.GetKeyDown("left ctrl"))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    estoc = true;
                else
                    hitting = true;
            }
            index_dashing = index_dashing % spritesDashing.Length;
            if (count_dash < spritesDashing.Length)
            {
                play_dash = true;
                if (old_index_dash != index_dashing)
                {
                    spriteRenderer.sprite = spritesDashing[count_dash];
                    count_dash++;
                }
            }
            else
            {
                play_dash = false;
                count_dash = 0;
                //estoc_time = estoc_cd;
            }
            old_index_dash = index_dashing;
        }
        else if(estoc)
        {
            Estoc(index_hitting);
        }
        else if(sc.jump_back_anime)
        {
            if (hs.damage_dealt)
            {
                CancelAttack();
            }
            int index_jump_back = (int)(Time.timeSinceLevelLoad * fps_jump_back);
            if(count_jump_back < spritesJumpBack.Length)
            {
                if(old_index_jb != index_jump_back)
                {
                    spriteRenderer.sprite = spritesJumpBack[count_jump_back];
                    count_jump_back++;
                }
            }
            else
            {
                sc.jump_back_anime = false;
                count_jump_back = 0;
            }
            old_index_jb = index_jump_back;

        }
        else if (Input.GetKeyDown("left ctrl") || hitting) //Hitting
        {
            if (!estoc)
            {
                if (Input.GetKey(KeyCode.UpArrow) && !hitting && estoc_time <= 0 )
                    Estoc(index_hitting);
                else if(!Input.GetKey(KeyCode.UpArrow) && hit_time <= 0)
                    SimpleHit(index_hitting);
            }
        }
        else if ((Input.GetKey("right") || Input.GetKey("left") )  && (sc.on_ground||(sc.on_unusual_ground && sc.IsWallSliding())))
        {
            index_walking = index_walking % spritesWalking.Length;
            spriteRenderer.sprite = spritesWalking[index_walking];
        }
        else
        {
            index_standing = index_standing % spritesStanding.Length;
            spriteRenderer.sprite = spritesStanding[index_standing];
        }

        if (!sc.on_ground && !sc.on_unusual_ground && !sc.dashing && !hitting && !sc.GetRedness() && !sc.IsWallSliding() && sc.GetWallJumpTime() <= 0)
        {
            spriteRenderer.sprite = spritesJumping[0];
        }
    }
    private void FlipRenderer()
    {
        if(!hitting && !play_dash_slip && !play_dash)
        {
            if (sc.GetDirection())
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false; //-0.03163809
            }
        }
        
    }

    private void SimpleHit(int index_hitting)
    {
        if (hs.stop_time)
        {

        }
        else if (count_hit < spritesHitting.Length)
        {
            hitting = true;
            index_hitting = index_hitting % spritesHitting.Length;
            if (old_index_hit != index_hitting)
            {
                if (sc.on_ground || sc.on_unusual_ground)
                    spriteRenderer.sprite = spritesHitting[count_hit];
                else
                    spriteRenderer.sprite = spritesHittingJump[count_hit];
                if (count_hit == 1)
                {
                    hs.damage_dealt = true;
                }
                else if (count_hit == spritesHitting.Length - 1)
                {
                    CancelAttack();
                }

                count_hit++;
            }
            old_index_hit = index_hitting;
        }
        else
        {
            hit_time = hit_cd;
            hitting = false;
            count_hit = 0;
            sc.NullVelocity();
        }
    }

    private void Estoc(int index_hitting)
    {
        if (hs.stop_time)
        {
            
        }
        else if (count_estoc < spritesEstoc.Length)
        {
            if(!rewind_estoc)
            {
                sc.RewindEstocTime();
                rewind_estoc = true;
            }
            estoc = true;
            index_hitting = index_hitting % spritesEstoc.Length;
            if (old_index_hit != index_hitting)
            {
                if (sc.on_ground || sc.on_unusual_ground)
                    spriteRenderer.sprite = spritesEstoc[count_estoc];
                else
                    spriteRenderer.sprite = spritesEstoc[count_estoc];
                if (count_estoc == 1)
                {
                    hs.damage_dealt = true;
                }
                else if (count_estoc == spritesHitting.Length - 1)
                {
                    CancelAttack();
                }

                count_estoc++;
            }
            old_index_hit = index_hitting;
        }
        else
        {
            estoc_time = estoc_cd;
            sc.NullVelocity();
            rewind_estoc = false;
            count_estoc = 0;
            estoc = false;
        }
    }

    private void CancelAttack()
    {
        hs.damage_dealt = false;
        hs.FlushArray();
        count_hit = 0;
        hitting = false;
    }
    //------------------------------------------------------------------------------------------------------------------------------
    public bool IsDashing()
    {
        return play_dash && play_dash_slip;
    }
    public bool IsHitting()
    {
        return hitting;
    }
    public bool IsEstoc()
    {
        return estoc || hit_time > 0;
    }
    public bool IsPlayingDash()
    {
        return play_dash;
    }
}
