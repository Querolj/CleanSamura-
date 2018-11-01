using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeColliderBossGMP : MonoBehaviour {
    /*public Vector2 offset_right;
    public Vector2 size_right;
    public Vector2 offset_left;
    public Vector2 size_left;*/
    public float right_x = 0.707f;
    public Transform[] stops;

    private BossGMParasiteBehaviour behaviour;
    private SamuraiController sc;
    private BoxCollider2D trigger;
    private Transform t;
    void Start () {
        behaviour = this.GetComponentInParent<BossGMParasiteBehaviour>();
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        t = this.GetComponent<Transform>();
    }
	
	void Update () {
		if(behaviour.charging)
        {
            Vector3 v = t.localPosition;
            if (!behaviour.direction)//0
                v.x = 0.021f;
            else//0.707f
                v.x = right_x;
            t.localPosition = v;
            //CheckStop();
        }
	}

    private void CheckStop()
    {
        foreach(Transform stop in stops)
        {
            if (t.position.x < stop.position.x + 0.05 && t.position.x > stop.position.x - 0.05)
                behaviour.StopCharge();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (behaviour.charging)
        {
            if(other.gameObject.tag == "Player")
                sc.ReceiveDamage(1,4, !behaviour.direction);
            else if(other.gameObject.tag == "Surface")
                behaviour.StopCharge();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (behaviour.charging)
        {
            if (other.gameObject.tag == "Surface")
                behaviour.StopCharge();
        }
    }
}
