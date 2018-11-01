using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerbeColliderBossGMP : MonoBehaviour {

    public int blood_right_x;
    public int blood_quantity;
    public float stun_power_gerbe = 0.2f;

    private BossGMParasiteAnimation anime_info;
    private BossGMParasiteBehaviour behaviour;
    private SamuraiController sc;
    
    private bool damage_dealt = false;
    private Transform t;
    private bool on_time = false;
	void Start () {
        anime_info = this.GetComponentInParent<BossGMParasiteAnimation>();
        behaviour = this.GetComponentInParent<BossGMParasiteBehaviour>();
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        t = this.GetComponent<Transform>();
        

    }
	
	void Update () {
        if (anime_info.IsGerbing() && !on_time)
        {
            damage_dealt = false;
            on_time = true;
            if (!behaviour.direction)
            {
                Quaternion v = t.localRotation;
                v.y = 0;
                t.localRotation = v;
            }
            else
            {
                Quaternion v = t.localRotation;
                v.y = 180;
                t.localRotation = v;
            }
        }
        else if (!anime_info.IsGerbing())
            on_time = false;
	}

    private void BloodSplash()
    {

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (!damage_dealt && anime_info.CanDealDamageGerbe())
        {
            
            //blood_splash.Emit(blood_quantity);
            if (other.gameObject.tag == "Player")
            {
                damage_dealt = true;
                sc.ReceiveDamage(1, behaviour.GetStrenght(), !behaviour.direction, stun_power_gerbe);
                //anime_info.StopGerbe();
                
            }
        }
    }
}
