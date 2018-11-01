using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour {
    public DoorLevier door;

    private bool activated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !activated && door.opened)
        {
            activated = true;
            door.CloseDoor();
        }
    }
}
