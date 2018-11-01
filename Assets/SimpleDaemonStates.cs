using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDaemonStates : EnnemiStates {
    private SimpleDaemonBehaviour behaviour;
    protected override void Start()
    {
        base.Start();
        behaviour = this.GetComponent<SimpleDaemonBehaviour>();
    }

    public override void DamageReceived(float strenght, int damage, bool direction, float stun_mult = 1)
    {
        if(behaviour.IsInAir())
        {
            e_body.velocity = new Vector2(0, e_body.velocity.y);
            base.DamageReceived(strenght*100, damage, direction, stun_mult);
        }
        else
            base.DamageReceived(strenght, damage, direction, stun_mult);

    }
}
