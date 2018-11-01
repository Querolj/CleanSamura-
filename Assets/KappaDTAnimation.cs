using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KappaDTAnimation : EnnemiAnimation {
    [Space]
    public Sprite[] spritesTurningRightToLeft; //Aussi LeftToRight
    public float fps_turning;

    private bool turning_r_to_l = false;
    private bool turning_l_to_r = false;
    private int count_turning = 0;
    private int old_index_turning = -1;
    private SpriteRenderer tree_renderer;
    private PolygonCollider2D coll_init;
    private PolygonCollider2D coll1;
    private Vector2[] points_init;
    private SpikeKappaDT spike;
    protected override void Start()
    {
        base.Start();
        GameObject spike_go = GameObject.Find(this.name + "/HitboxSpikes");
        spike = spike_go.GetComponent<SpikeKappaDT>();
        tree_renderer = spike_go.GetComponent<SpriteRenderer>();
        coll_init = spike_go.GetComponent<PolygonCollider2D>();
        points_init = coll_init.points;
        coll1 = GameObject.Find(this.name + "/HitboxSpikes/PolyTree1").GetComponent<PolygonCollider2D>();
        if (tree_renderer == null)
            print("tree_renderer null");
    }

    protected override bool ReadySpecialAttack()
    {
        if(turning_r_to_l || turning_l_to_r)
        {
            
            int index_turn = (int)(Time.timeSinceLevelLoad * fps_turning);
            index_turn = index_turn % spritesTurningRightToLeft.Length;

            if (old_index_turning != index_turn)
            {
                if(count_turning == 0)
                    coll_init.points = coll1.points;
                else if(count_turning == 2)
                {
                    spike.RotatePolyTrigger();
                }
                if (tree_renderer.enabled)
                    tree_renderer.enabled = false;
                if (count_turning < spritesTurningRightToLeft.Length)
                    spriteRenderer.sprite = spritesTurningRightToLeft[count_turning];
                count_turning++;
            }

            if (count_turning == spritesTurningRightToLeft.Length + 1)
            {
                count_turning = 0;
                turning_r_to_l = false;
                turning_l_to_r = false;
                AI.block_move = false;
                tree_renderer.enabled = true;
                coll_init.points = points_init;
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
