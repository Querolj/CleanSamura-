using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KappaBehaviour : EnnemiAI {

    protected override bool CustomBasicAttack()
    {
        if (!attacking && in_sight && mods == 1)
        {
            //Stop
        }
        return true;
    }
}
