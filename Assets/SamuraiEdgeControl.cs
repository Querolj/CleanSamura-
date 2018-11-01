using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiEdgeControl : MonoBehaviour {

    private SamuraiController sc;
    private EdgeCollider2D edge;
    void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        edge = this.GetComponent<EdgeCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (sc.on_ground || sc.on_unusual_ground)
        {
            edge.enabled = false;
        }
        else
            edge.enabled = true;
	}
}
