using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancierAnimation : EnnemiAnimation {
    public Sprite[] spritesCharging;
    public Sprite[] spritesChargeHit;

    private LancierBehaviour lb;
    private Sprite[] tmp_walking;
    private Sprite[] tmp_hit;
    private bool anime_changed = false;
    protected override void CustomStart()
    {
        lb = this.GetComponent<LancierBehaviour>();
        tmp_walking = spritesWalking;
        tmp_hit = spritesHitting;
    }

    protected override void ChangeAnimation()
    {
        if(lb.charging && !anime_changed)
        {
            spritesWalking = spritesCharging;
            spritesHitting = spritesChargeHit;
            anime_changed = true;
        }
        else if(!lb.charging && anime_changed)
        {
            spritesWalking = tmp_walking;
            //spritesHitting = tmp_hit;
            anime_changed = false;
        }
    }

    protected override void AfterAttack()
    {
        spritesHitting = tmp_hit;
    }

}
