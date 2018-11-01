using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeKappaDT : SpikesCollider {
    public float x_left = 0.0234f;
    public float x_right = -0.0235f;

    private PolygonCollider2D poly;
    protected override void Start()
    {
        base.Start();
        poly = this.GetComponent<PolygonCollider2D>();
    }

    protected override void Update()
    {
        //base.Update();
        if (spike_dmg_timer > 0)
            spike_dmg_timer -= Time.deltaTime;
    }

    public void RotatePolyTrigger()
    {
        Vector2 v = t.localPosition;
        Quaternion q = t.localRotation;
        if (!behaviour.direction)
        {
            v.x = x_left;
            q.y = 180;
        }
        else
        {
            v.x = x_right;
            q.y = 0;
        }
        t.localPosition = v;
        t.localRotation = q;
    }

    public void DisablePoly()
    {
        poly.enabled = false;
    }
}
