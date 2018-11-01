using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetectorSamurai : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            print("un mur");
        }


    }
}
