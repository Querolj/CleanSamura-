using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleVolcanoAnimation : EnnemiAnimation {
    public Sprite[] spritesTurning;
    public float fps_turning;

    private bool turning_r_to_l = false;
    private bool turning_l_to_r = false;
    private int count_turning = 0;
    private int old_index_turning = -1;


    protected override bool ReadySpecialAttack()
    {

        if (turning_r_to_l || turning_l_to_r)
        {

            int index_turn = (int)(Time.timeSinceLevelLoad * fps_turning);
            index_turn = index_turn % spritesTurning.Length;

            if (old_index_turning != index_turn)
            {
                if (count_turning < spritesTurning.Length)
                    spriteRenderer.sprite = spritesTurning[count_turning];
                count_turning++;
            }

            if (count_turning == spritesTurning.Length + 1)
            {
                count_turning = 0;
                turning_r_to_l = false;
                turning_l_to_r = false;
                AI.block_move = false;

                base.FlipRenderer();
            }


            old_index_turning = index_turn;
            return true;
        }
        return false;
    }
    protected override void FlipRenderer()
    {
        //Do nothing
    }
    //------------------------------------------------------
    public bool TurningLToR
    {
        get { return turning_l_to_r; }
        set { turning_l_to_r = value; }
    }
    public bool TurningRToL
    {
        get { return turning_r_to_l; }
        set { turning_r_to_l = value; }
    }
}
