using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDirectionControl : MonoBehaviour {

    private SamuraiController sc;
    private Transform t;
    void Start()
    {
        sc = this.GetComponentInParent<SamuraiController>();
        t = this.GetComponent<Transform>();
    }

    void Update()
    {
        Quaternion q = sc.trans.localRotation;
        if (sc.GetDirection())
        {
            q.y = 180;
        }
        else
        {
            q.y = 0;
        }
        sc.trans.localRotation = q;
    }
}
