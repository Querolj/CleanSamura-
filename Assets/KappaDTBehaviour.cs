using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KappaDTBehaviour : EnnemiAI {

    private bool old_dir;
    private KappaDTAnimation anime;
    private SpikeKappaDT spike;
    protected override void Start()
    {
        base.Start();
        anime = this.GetComponent<KappaDTAnimation>();
        spike = this.GetComponentInChildren<SpikeKappaDT>();
        if (spike == null)
            print("spike null KappaTree");
        old_dir = direction;
    }

    protected override void SpecialUpdate()
    {
        if(old_dir != direction)//Animation turn
        {
            block_move = true;
            if(!direction)
            {
                anime.TurningRToL = true;
            }
            else
            {
                anime.TurningLToR = true;
            }
        }
        old_dir = direction;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        spike.DisablePoly();
    }

}
