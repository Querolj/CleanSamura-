using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteZone : MonoBehaviour {
    private EnnemiAI behaviour;
    private Transform t;
    private bool in_bite_zone;
    void Start () {
        behaviour = this.GetComponentInParent<EnnemiAI>();
        t = this.GetComponent<Transform>();
    }
	
	void Update () {
        Quaternion q = t.localRotation;
        if (behaviour.direction)
        {
            q.y = 180;
        }
        else
        {
            q.y = 0;
        }
        t.localRotation = q;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            in_bite_zone = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            in_bite_zone = false;
        }
    }

    public bool IsInBiteZone()
    {
        return in_bite_zone;
    }
    public void SetInBiteZone(bool b)
    {
        if (b == true)
            print("in_bite_zone is true ?");
        in_bite_zone = b;
    }
}
