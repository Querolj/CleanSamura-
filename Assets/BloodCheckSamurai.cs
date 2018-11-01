using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCheckSamurai : MonoBehaviour {
    public float x_right;
    public float x_left = 0.045f;

    private SamuraiController sc;
    private int count_blood = 0;
    private Transform t;
    [HideInInspector]
    public List<SpriteRenderer> blood_in_range;
    void Start () {
        sc = this.GetComponentInParent<SamuraiController>();
        t = this.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {

            Vector3 v = t.localPosition;
            if (sc.GetDirection())
            {
                v.x = x_right;
            }
            else
            {
                v.x = x_left;
            }
            t.localPosition = v;
        
        
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "blood")
        {
            sc.on_blood = true;
            blood_in_range.Add(other.gameObject.GetComponent<SpriteRenderer>());
            count_blood++;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "blood")
        {
            if (count_blood > 0)
                count_blood--;
            if (count_blood == 0)
                sc.on_blood = false;
            blood_in_range.Remove(other.gameObject.GetComponent<SpriteRenderer>());
        }
    }
}
