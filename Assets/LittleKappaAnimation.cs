using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleKappaAnimation : EnnemiAnimation{
    [Space]
    public Sprite[] spritesIdle;
    public int fps_idle = 10;
    [HideInInspector]
    public bool idling = false;

    private int count_idle = 0;
    private int old_index_idle = -1;
    private LittleKappaBehaviour behaviour;
    protected override void CustomStart()
    {
        behaviour = this.GetComponent<LittleKappaBehaviour>();
    }
    protected override bool ReadySpecialAttack()
    {
        if(idling && !states.IsDead())
        {
            int index_idle = (int)(Time.timeSinceLevelLoad * fps_idle);
            index_idle = index_idle % spritesIdle.Length;

            if (old_index_idle != index_idle)
            {
                spriteRenderer.sprite = spritesIdle[count_idle];
                if(count_idle < spritesIdle.Length - 1)
                    count_idle++;
            }
            if (count_idle >= spritesIdle.Length)
            {
                
            }
            else
                old_index_idle = index_idle;
            return true;
        }
        

        return false;
    }
        
    

}
