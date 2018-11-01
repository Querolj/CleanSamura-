using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCheckSimpleDaemon : BloodCheck {
    public float offset_x = 0.2336674f;
    public float size_x = 0.4758797f;

    private float init_offset_x;
    private float init_size_x;
    private SimpleDaemonBehaviour behaviour;
    private BoxCollider2D coll;
    protected override void Start()
    {
        base.Start();
        coll = this.GetComponent<BoxCollider2D>();
        init_offset_x = coll.offset.x;
        init_size_x = coll.size.x;
        behaviour = this.GetComponentInParent<SimpleDaemonBehaviour>();
    }
    protected override void Update()
    {
        base.Update();
        Vector2 off = coll.offset;
        Vector2 size = coll.size;
        if (behaviour.IsInAir())
        {
            off.x = offset_x;
            size.x = size_x;
        }
        else
        {
            off.x = init_offset_x;
            size.x = init_size_x;
        }
        coll.offset = off;
        coll.size = size;
    }
}
