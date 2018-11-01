using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableJoint : MonoBehaviour {
    public bool has_prison;
    public Prison prison;
    public HingeJoint2D hj_pred;

    private HingeJoint2D hj;
    private bool broke = false;
    private AudioSource source;
    void Start () {
        hj = this.GetComponent<HingeJoint2D>();
        source = this.GetComponent<AudioSource>();
    }

	public void BreakJoint()
    {
        hj.enabled = false;
        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.min = -30;
        limits.max = 30;
        hj_pred.limits = limits;
        if (has_prison && !prison.chain_broke)
        {
            source.Play();
            prison.chain_broke = true;
        }
    }
    public bool IsBroken()
    {
        return broke;
    }
	
}
