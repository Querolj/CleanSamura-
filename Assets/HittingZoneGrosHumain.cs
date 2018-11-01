using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingZoneGrosHumain : HittingZoneE {
    public Vector2 off_punch_left;
    public Vector2 size_punch_left;
    public Vector2 off_punch_right;
    public Vector2 size_punch_right;
    public Vector2 off_spe_right;
    public Vector2 size_spe_right;
    public Vector2 off_spe_left;
    public Vector2 size_spe_left;

    private GrosHumainBehaviour ghb;
    protected override void CustomStart()
    {
        ghb = this.GetComponentInParent<GrosHumainBehaviour>();
    }

    protected override bool CustomChangeHitZone()
    {
        if ((ghb.punch_launched || ghb.special_hit_launched) )
        {
            Vector3 v = h_trans.localPosition;
            if (ghb.direction)//right
            {
                v.x = pos_x_right;
                if(ghb.punch_launched)
                {
                    h_collider.offset = off_punch_right;
                    h_collider.size = size_punch_right;
                }
                else if(ghb.special_hit_launched)
                {
                    h_collider.offset = off_spe_right;
                    h_collider.size = size_spe_right;
                }
                else
                {
                    h_collider.offset = offset_right;
                    h_collider.size = size_right;
                }
            }
            else
            {
                v.x = pos_x_left;
                if (ghb.punch_launched)
                {
                    h_collider.offset = off_punch_left;
                    h_collider.size = size_punch_left;
                }
                else if (ghb.special_hit_launched)
                {
                    h_collider.offset = off_spe_left;
                    h_collider.size = size_spe_left;
                }
                else
                {
                    h_collider.offset = offset_left;
                    h_collider.size = size_left;
                }
            }
            h_trans.localPosition = v;
            return true;
        }


        return false;
    }

    public void RebootHitZone()
    {
        if(ghb.direction)
        {
            h_collider.offset = offset_right;
            h_collider.size = size_right;
        }
        else
        {
            h_collider.offset = offset_left;
            h_collider.size = size_left;
        }
    }
}
