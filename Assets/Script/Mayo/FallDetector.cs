using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetector : MonoBehaviour {
    MayoController mc;
	// Use this for initialization
	void Start () {
        mc = GameObject.Find("Mayo").GetComponent<MayoController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            mc.myTrans.position = mc.respawn_point;
        }
    }
}
