using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffect : MonoBehaviour {
    public GameObject jump_effect;

    private SamuraiController sc;
    private SimpleAnimation anime;
    private Transform t;
    void Start () {
        sc = this.GetComponentInParent<SamuraiController>();
        anime = this.GetComponent<SimpleAnimation>();
        t = this.GetComponent<Transform>();
	}
	
	void Update () {
        if(sc.IsJumping())
        {
            sc.SetIsJumping(false);
            StartEffect();
        }
    }
    private void StartEffect()
    {
        Instantiate(jump_effect, t.position, Quaternion.identity);
    }
}
