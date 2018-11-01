using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiStates : MonoBehaviour {
    protected Rigidbody2D e_body;
    protected CapsuleCollider2D e_collider;
    protected Transform e_trans;
    protected EnnemiAI AI;
    protected SamuraiController sc;
    protected HittingScript hs;
    //Ennemi stats
    public int life = 1;
    public float strenght;
    public float throwing_strenght;
    
    public int blood_quantity;
    public bool is_daemon; //Les démons ne perdent pas contrôle sur le sang
    public float drawback = 1f;
    public float stun_time = 0.3f; //Le temps que l'ennemi lui même est stun, pas sa puissance de stun (stun_power est dans behaviour/AI)
    private float stun_timer = 0;
    public int stun_threshold = 1;
    //effects
    protected ParticleSystem blood_splash;
    protected ParticleSystem.EmissionModule blood_emission;
    protected SpriteRenderer renderer;
    public float stun_redness = 0.5f;
    [HideInInspector]
    public bool stunned = false;
    [HideInInspector]
    public bool time_end = true;
    private bool just_once = true;
    protected int initial_stun_threshold;
    //Sound related
    private bool taking_damage = false;
    //Death
    private bool is_dead = false;
    private bool on_ground = false;
    //Substitute (to hit with other collider)
    public bool substitute = false;
    public EnnemiStates real_states;

    // !!!!  temporary for proto
    public bool last_one = false;

    protected virtual void Start () {
        if(!substitute)
        {
            e_body = this.GetComponent<Rigidbody2D>();
            e_collider = this.GetComponent<CapsuleCollider2D>();
            e_trans = this.GetComponent<Transform>();
            GameObject s = GameObject.Find("Samuraï");
            sc = s.GetComponent<SamuraiController>();
            hs = s.GetComponentInChildren<HittingScript>();
            AI = this.GetComponent<EnnemiAI>();
            if (AI == null)
                print("AI null");
            renderer = this.GetComponent<SpriteRenderer>();
            //blood_splash = GameObject.Find(this.name + "/Blood").GetComponent<ParticleSystem>();
            //Copy our empty object with the Particle system attached and extract the main class from its ParticleSystem
            //var main = Instantiate(this, gameObject.transform.position, Quaternion.identity).GetComponent<ParticleSystem>().main;

            blood_splash = this.GetComponentInChildren<ParticleSystem>();
            initial_stun_threshold = stun_threshold;
            blood_emission = blood_splash.emission;
        }
        
    }
	
	protected virtual void Update () {
        if(!substitute)
        {
            if (!time_end & just_once)
            {
                AI.SetBlockMove(true);
                just_once = false;
            }
            else if (just_once)
            {
                AI.SetBlockMove(false);
                just_once = false;
            }
            if (stun_timer > 0)
            {
                stun_timer -= Time.deltaTime;
                renderer.color = new Color(1, stun_redness, stun_redness, 1);
            }
            else if (stunned)
            {
                StopStun();
            }
        }
	}
    //--------------------------------------------------------------------------------------------------------------------------------------------
    public bool GetDirection()
    {
        return AI.GetDirection();
    }
    public int GetMod()
    {
        return AI.GetMod();
    }
    public void SetBlockMove(bool block)
    {
        AI.SetBlockMove(block);
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    public virtual void DamageReceived(float strenght, int damage, bool direction, float stun_mult = 1)
    {
        taking_damage = true;
        life -= damage;
        if(life <= 0)
        {
            //gérer la mort
            if(!is_dead)
                Death();
            if(on_ground)
            {
                e_body.simulated = false;
                e_collider.enabled = false;
            }
            if (last_one)
                sc.SetRespawn(true);
        }
        if(direction)
        {
            e_body.AddForce(Vector2.right * strenght);
        }
        else
        {
            e_body.AddForce(Vector2.left * strenght);
        }

        if (stun_threshold == 0)
        {
            stun_threshold = initial_stun_threshold;
            time_end = false;
            stunned = true;
            Drawnback();

            stun_timer = stun_time * stun_mult;
            just_once = true;
            AI.ResetAttack();
        }
        else
        {
            stun_threshold--;
            stunned = true;
            //stun_timer = 0.2f;
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    public void Drawnback()
    {
        if (sc.GetDirection())
        {
            e_body.AddForce(Vector2.right * drawback);
        }
        else
        {
            e_body.AddForce(Vector2.left * drawback);
        }
    }
    public void Death()
    {
        BloodSplit();
        if (AI == null)
        {
            print("AI null in Death()");
            AI = this.GetComponent<EnnemiAI>();
        }
            
        AI.SetBlockMove(true);
        if(hs != null)
            hs.RemoveFromTarget(this);
        is_dead = true;
        
    }
    private void BloodSplit()
    {
        //blood_splash.emission.enabled = true;
        blood_splash = this.GetComponentInChildren<ParticleSystem>();
        blood_splash.Play();
        //blood_splash.Emit(blood_quantity);
    }
    private void StopStun()
    {
        stunned = false;
        just_once = true;
        time_end = true;
        renderer.color = new Color(1, 1, 1, 1);
        AI.mods = 1;
        if (AI == null)
            print("null");
        AI.ResetAttack();
    }
    private void OnlyRedness()
    {
        stunned = false;
        renderer.color = new Color(1, 1, 1, 1);
        AI.mods = 1;
    }
    public bool IsDead()
    {
        return is_dead;
    }
    public void SetDeath(bool d, int life = 1)
    {
        if (d)
            this.life = 0;
        else
            this.life = life; 
    }
    public CapsuleCollider2D GetCollider()
    {
        return e_collider;
    }
    public bool TakingDamage
    {
        get { return taking_damage; }
        set { taking_damage = value; }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!substitute && other.collider.tag == "Surface")
        {
            on_ground = true;
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (!substitute && other.collider.tag == "Surface")
        {
            on_ground =false;
        }

    }
}
