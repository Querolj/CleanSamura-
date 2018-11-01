using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancierSound : EnnemiSound {

    public AudioClip thrust;

    public void PlayThrust()
    {
        source.PlayOneShot(thrust);
    }
}
