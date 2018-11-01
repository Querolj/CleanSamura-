using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleKappaStates : EnnemiStates {
    public const float time_idle_cd = 3;
    private float time_idle;

    private LittleKappaAnimation anime;

    protected override void Start()
    {
        base.Start();
        time_idle = time_idle_cd;
        anime = this.GetComponent<LittleKappaAnimation>();

    }
    protected override void Update()
    {
        base.Update();
        if(anime.idling)
        {
            time_idle -= Time.deltaTime;
            if (time_idle <= 0)
                anime.idling = false;
        }

    }

    public override void DamageReceived(float strenght, int damage, bool direction, float stun_mult = 1)
    {
        float f = sc.trans.position.x - e_trans.position.x;
        print(f < 0);
        print("dir " + direction);
        if((f >= 0 && GetDirection()) || (f < 0 && !GetDirection()))
            base.DamageReceived(strenght, damage, direction, stun_mult);
        else
        {
            //Inclure sound effect + sprite
            SpingEffect();
        }
    }

    private void SpingEffect()
    {
        print("No damage to Kappa");
        anime.idling = true;
        time_idle = time_idle_cd;
    }
}
