using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingZoneLancier : HittingZoneE {
    public Vector2 offset_right_charge;
    public Vector2 size_right_charge;
    public Vector2 offset_left_charge;
    public Vector2 size_left_charge;

    private LancierBehaviour lb;

    protected override void CustomStart()
    {
        lb = this.GetComponentInParent<LancierBehaviour>();
    }
    protected override bool CustomChangeHitZone()
    { 
        if(lb.charging)
        {
            if( lb.direction)
            {
                Vector3 v = h_trans.localPosition;
                v.x = 0.315f;
                h_trans.localPosition = v;
                h_collider.offset = offset_right_charge;
                h_collider.size = size_right_charge;
            }
            else
            {
                Vector3 v = h_trans.localPosition;
                v.x = 0;
                h_trans.localPosition = v;
                h_collider.offset = offset_left_charge;
                h_collider.size = size_left_charge;

            }
            return true;
        }
        return false;
    }
}
