using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public bool checkpoint_reached = false;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            checkpoint_reached = true;
        }
    }
}
