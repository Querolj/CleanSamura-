using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrosHumainAnimation : EnnemiAnimation {
    public Sprite[] spritesHittingFist;
    public Sprite[] spritesHittingStrong;

    private GrosHumainBehaviour ghb;
    private Sprite[] tmp_hit;
    private Sprite[] baseHitSprite;
    protected override void CustomStart()
    {
        baseHitSprite = spritesHitting;
        ghb = this.GetComponent<GrosHumainBehaviour>();
        tmp_hit = spritesHitting;
    }

    protected override void ChangeAnimation()
    {
        if(ghb.punch_launched)
        {
            spritesHitting = spritesHittingFist;
        }
        else if(ghb.special_hit_launched)
        {
            spritesHitting = spritesHittingStrong;
        }
    }

    protected override void AfterAttack()
    {
        if (ghb.special_hit_launched)
            ghb.RebootStrongAttack();
        if (ghb.punch_launched)
            ghb.RebootPunch();
        
        spritesHitting = tmp_hit;
        
    }
    public void ToBasicHitAnime()
    {
        spritesHitting = baseHitSprite;
    }
}
