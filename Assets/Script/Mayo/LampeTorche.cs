using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampeTorche : MonoBehaviour {
    private Transform lampe_pos;
    private MayoController mc;
	// Use this for initialization
	void Start () {
        lampe_pos = this.GetComponent<Transform>();
        mc = GameObject.Find("Mayo").GetComponent<MayoController>();
        lampe_pos.gameObject.GetComponent<MeshRenderer>().enabled = false;

    }
	
	// Update is called once per frame
	void Update () {
        if(mc.normal_mode)
        {
            Vector3 dir = lampe_pos.localPosition;
            Vector3 rot = lampe_pos.localEulerAngles;
            if (mc.directed)
            {
                dir.x = 0.194f;
                rot.z = -90;
            }
            else
            {
                dir.x = -0.194f;
                rot.z = 90;
            }

            lampe_pos.localPosition = dir;
            lampe_pos.localEulerAngles = rot;

            if (Input.GetKeyUp(KeyCode.F))
            {
                if (lampe_pos.gameObject.GetComponent<MeshRenderer>().enabled)
                    lampe_pos.gameObject.GetComponent<MeshRenderer>().enabled = false;
                else
                {
                    lampe_pos.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }

            }
        }
        
    }
}
