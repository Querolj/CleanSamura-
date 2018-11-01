using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesCollider : MonoBehaviour {

    public float stun_power_spike = 0.2f;
    public int spike_str = 1;
    public bool mob_spike = true;
    public float spike_drawback = 5;
    public bool drawback_direction_by_mob = true;

    protected EnnemiAI behaviour;
    private SamuraiController sc;
    private bool damage_dealt = false;
    public float spike_dmg_cd = 1;
    protected float spike_dmg_timer = 0;
    protected Transform t;
    private bool on_time = false;

    protected virtual void Start () {
        //spike_dmg_timer = spike_dmg_cd;
        if (mob_spike)
            behaviour = this.GetComponentInParent<EnnemiAI>();
        else
            drawback_direction_by_mob = false;
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        t = this.GetComponent<Transform>();
    }

    protected virtual void Update () {
        if(mob_spike)
        {
            if (!behaviour.direction)
            {
                Quaternion v = t.localRotation;
                v.y = 180;
                t.localRotation = v;
            }
            else
            {
                Quaternion v = t.localRotation;
                v.y = 0;
                t.localRotation = v;
            }
        }
        if (spike_dmg_timer > 0)
            spike_dmg_timer -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!damage_dealt)
        {
            if (other.gameObject.tag == "Player" && spike_dmg_timer <= 0)
            {
                if(mob_spike && drawback_direction_by_mob)
                    sc.ReceiveDamage(spike_str, spike_drawback, !behaviour.direction, stun_power_spike);
                else
                {
                    float f = sc.trans.position.x - t.position.x;
                    if(f > 0)
                        sc.ReceiveDamage(spike_str, spike_drawback, false, stun_power_spike);
                    else
                        sc.ReceiveDamage(spike_str, spike_drawback, true, stun_power_spike);
                }
                sc.body.velocity = new Vector3(0, 0, 0);
                if (sc.dashing)
                    sc.EndDash();
                spike_dmg_timer = spike_dmg_cd;
            }
        }
    }
}
