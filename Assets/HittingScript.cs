using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingScript : MonoBehaviour {
    [HideInInspector]
    public bool damage_dealt = false;
    [HideInInspector]
    public bool damage_new_target = false;
    public float stop_time_duration = 0.09f;

    public Vector2 offset_estoc;
    public Vector2 size_estoc;
    //public Vector2 offset_estoc_l;
    //public Vector2 size_estoc_l;

    private SamuraiController sc;
    private SamuraiAnimator sa;
    private BoxCollider2D hitting_zone; 
    private Levier levier;
    
    [HideInInspector]
    public bool stop_time = false;
    [HideInInspector]
    public List<EnnemiStates> targets;
    //Breaking stuff
    private BreakableJoint bj;
    void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        sa = this.GetComponentInParent<SamuraiAnimator>();
        hitting_zone = this.GetComponent<BoxCollider2D>();
        targets = new List<EnnemiStates>();
        levier = null;
    }
	
	// Update is called once per frame
	void Update () {
        if(!sa.IsHitting() && !sa.IsDashing())
            ChangeHitZone();
        //if (damage_dealt)
        //    DealDamage();
    }

    private void ChangeHitZone()
    {
        bool d = sc.GetDirection();
        if (d)
        {
            Quaternion q = hitting_zone.transform.rotation;
            q.y = 180;
            if (sa.IsEstoc())
            {
                hitting_zone.offset = offset_estoc;
                hitting_zone.size = size_estoc;
                
            }
            else
            {
                hitting_zone.offset = new Vector2(-0.2595734f, 0.2798519f);
                hitting_zone.size = new Vector2(0.4194357f, 0.5560379f);
            }
            hitting_zone.transform.rotation = q;
        }
        else
        {
            Quaternion q = hitting_zone.transform.rotation;
            q.y = 0;
            if (sa.IsEstoc())
            {
                hitting_zone.offset = offset_estoc;
                hitting_zone.size = size_estoc;
            }
            else
            {
                hitting_zone.offset = new Vector2(-0.2595734f, 0.2798519f);
                hitting_zone.size = new Vector2(0.4194357f, 0.5560379f);
            }
            hitting_zone.transform.rotation = q;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------
    public void ActiveHittingZone()
    {
        hitting_zone.enabled = true;
    }
    public void DesactiveHittingZone()
    {
        hitting_zone.enabled = false;

    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    void DealDamage()
    {
        lock(targets)
        {
            foreach (EnnemiStates target in targets.ToArray())
            {
                DamagingEnnemi(target);
            }
            damage_new_target = true;
        }
        if (bj != null && !bj.IsBroken())
            bj.BreakJoint();
        damage_dealt = false;
        if (levier != null && !levier.GetActivate())
            Levier();
        
    }
    private void DamagingEnnemi(EnnemiStates target)
    {
        if (!stop_time)
        {
            stop_time = true;
            Invoke("StopTime", stop_time_duration);
        }
        if (sc.GetDirection())
            target.DamageReceived(sc.strenght, sc.damage, true);
        else
            target.DamageReceived(sc.strenght, sc.damage, false);
    }
    private void Levier()
    {
        levier.OnOff();
        
    }
    private void StopTime()
    {
        stop_time = false;
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    public void RemoveFromTarget(EnnemiStates es)
    {
        lock (targets)
        {
            targets.Remove(es);
        }
    }
    
    //--------------------------------------------------------------------------------------------------------------------------------------------
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ennemi")
        {
            lock (targets)
            {
                //targets.Add(other.gameObject.GetComponent<EnnemiStates>());
                EnnemiStates target = other.gameObject.GetComponent<EnnemiStates>(); 
                if(target.substitute)//Pour attaquer un collider enfant à Ennemi States
                {
                    target = target.real_states;
                }
                if (damage_dealt && !targets.Contains(target))//Gérer le fait qu'un ennemi rentre dans la zone quand on est censé faire des dommages
                {
                    if (target == null)
                        print("target null");
                    else
                    {
                        targets.Add(target);
                        DamagingEnnemi(target);
                    }
                    
                }
                /*else if(!targets.Contains(target))
                {
                    targets.Add(target);  
                }*/
                
            }
        }

        if(other.tag == "BreakableJoint")
        {
            bj =  other.gameObject.GetComponent<BreakableJoint>();
            
        }
        if(other.tag == "Levier")
        {
            levier = other.gameObject.GetComponent<Levier>();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Ennemi")
        {
            //RemoveFromTarget(other.gameObject.GetComponent<EnnemiStates>());
        }
        if (other.tag == "BreakableJoint")
            bj = null;

        if (other.tag == "Levier")
        {
            levier = null;
        }
    }
    public void FlushArray()
    {
        lock (targets)
        {
            targets.Clear();
        }
    }
    

}
