using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDaemonAnimation : EnnemiAnimation {
    public Sprite[] spritesRunning;
    public Sprite[] spritesAtter;
    public Sprite[] spritesJumpBack;
    public Sprite[] spritesRealSimpleHit;
    
    public Sprite spritesJumpBackAtter;
    public float atter_fps;
    

    private SimpleDaemonBehaviour behaviour;
    private Sprite[] spritesWalkingInit;
    private Sprite[] spritesHittingInit;
    private int count_atter = 0;
    private int old_index_atter = -1;
    private BiteZone bite_zone;
    //Attack stuff
    private bool player_damaged = false;
    //Jump Back
    public float jump_back_fps = 8;
    [HideInInspector]
    public bool jump_backing = false;
    private int count_jump_back = 0;
    private int old_index_jump_back = -1;

    protected override void CustomStart()
    {
        spritesWalkingInit = spritesWalking;
        spritesHittingInit = spritesHitting;
        behaviour = this.GetComponent<SimpleDaemonBehaviour>();
        bite_zone = behaviour.gameObject.GetComponentInChildren<BiteZone>();
    }
    protected override void ChangeAnimation()
    {
        if(behaviour.mods == 1)
        {
            spritesWalking = spritesRunning;
        }
        else
        {
            spritesWalking = spritesWalkingInit;
        }
    }

    protected override bool ReadySpecialAttack()
    {
        int index_atter = (int)(Time.timeSinceLevelLoad * atter_fps);
        int index_jump_back = (int)(Time.timeSinceLevelLoad * jump_back_fps);
        bool can_bite = false;
        if(behaviour.CanPlayAtter())
        {
            behaviour.StopBody();
            if (old_index_atter != index_atter)
            {
                spriteRenderer.sprite = spritesAtter[spritesAtter.Length - 1];
                count_atter++;
            }
            if (count_atter == spritesAtter.Length)
            {
                count_atter = 0;
                behaviour.EndJump();
                old_index_atter = index_atter;
                player_damaged = false;
                return true;
            }
            old_index_atter = index_atter;
            can_bite = true;
        }
        else if(behaviour.IsInAir() && !attacking)
        {
            spriteRenderer.sprite = spritesHitting[spritesHitting.Length - 1];
            can_bite = true;
        }
        else if(jump_backing)
        {
            if (old_index_jump_back != index_jump_back)
            {
                spriteRenderer.sprite = spritesJumpBack[count_jump_back];
                count_jump_back++;
            }
            if (count_jump_back == spritesJumpBack.Length)
            {
                count_jump_back = 0;
                old_index_jump_back = index_jump_back;
                jump_backing = false;
                //behaviour.block_move = false;
                return true;
            }
            old_index_jump_back = index_jump_back;
            return true;
        }


        if (can_bite && IsInBiteZone() && !player_damaged && !behaviour.CanPlayAtter())
        {
            player_damaged = true;
            behaviour.Cac();
            bite_zone.SetInBiteZone(false);
        }

        return can_bite;
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------
    public void SwitchHittingAnime(bool b)//True si charge, sinon hit simple
    {
        if(b)
        {
            spritesHitting = spritesHittingInit;
        }else
        {
            spritesHitting = spritesRealSimpleHit;
        }
    }
    public bool IsInBiteZone()
    {
        return bite_zone.IsInBiteZone();
    }
}
