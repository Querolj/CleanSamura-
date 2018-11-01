using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleVolcanoBehaviour : EnnemiAI {
    public float x_right_volcano1;
    public float x_right_volcano2;
    public float x_left_volcano1;
    public float x_left_volcano2;

    private Transform volcano1_trans;
    private Transform volcano2_trans;
    private bool volcano_destroyed = false;
    //Turning
    private bool old_dir;
    private DoubleVolcanoAnimation anime;
    protected override void CustomStart()
    {
        volcano1_trans = GameObject.Find(this.name + "/Volcan01").GetComponent<Transform>();
        volcano2_trans = GameObject.Find(this.name + "/Volcan02").GetComponent<Transform>();
        anime = this.GetComponent<DoubleVolcanoAnimation>();
    }

    protected override void SpecialUpdate()
    {
        VolcanPosition();
        Turning();
    }

    private void VolcanPosition()
    {
        if (!volcano_destroyed)
        {
            Vector3 v1 = volcano1_trans.localPosition;
            Vector3 v2 = volcano2_trans.localPosition;
            Quaternion q = volcano1_trans.localRotation;
            if (direction)
            {
                v1.x = x_right_volcano1;
                v2.x = x_right_volcano2;
                q.y = 180;
            }
            else
            {
                v1.x = x_left_volcano1;
                v2.x = x_left_volcano2;
                q.y = 0;
            }
            volcano1_trans.localPosition = v1;
            volcano2_trans.localPosition = v2;
            volcano1_trans.localRotation = q;
            volcano2_trans.localRotation = q;
        }
    }
    private void Turning()
    {
        if (old_dir != direction)//Animation turn
        {
            block_move = true;
            if (!direction)
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
        if(!volcano_destroyed)
        {
            GameObject.Destroy(volcano1_trans.gameObject);
            GameObject.Destroy(volcano2_trans.gameObject);
            volcano_destroyed = true;
        }
        
    }
    protected override void BasicAttack()
    {
        if(!(anime.TurningRToL || anime.TurningLToR ))
            base.BasicAttack();
    }
}
