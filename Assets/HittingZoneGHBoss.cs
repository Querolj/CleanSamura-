using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingZoneGHBoss : HittingZoneE {

    private BossGMParasiteBehaviour behaviour;
    private BossGMParasiteAnimation bgmp_anime;
    protected override void CustomStart()
    {
        behaviour = this.GetComponentInParent<BossGMParasiteBehaviour>();
        bgmp_anime = this.GetComponent<BossGMParasiteAnimation>();
    }

    protected override bool CustomChangeHitZone()
    {
        return false;
    }
}
