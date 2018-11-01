using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour {
    private SamuraiController sc;
    private RectTransform rt;

	void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        rt = this.GetComponent<RectTransform>();
    }
	
	void Update () {
        float p = sc.dash_time/sc.dash_cd;
        if(p<0.02)
            rt.localScale = new Vector3(1,0,1);
        else
            rt.localScale = new Vector3(1, p, 1);

    }
}
