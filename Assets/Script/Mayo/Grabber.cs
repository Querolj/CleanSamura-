using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {
    public bool grabbed;
    RaycastHit2D hit;
    public float distance = 2f;
    private MayoController mayo_controller;
    // Use this for initialization
    void Start () {
        mayo_controller = GameObject.Find("Mayo").GetComponent<MayoController>();
    }
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.B))
        {
            Physics2D.queriesStartInColliders = false;
            hit = Physics2D.Raycast(mayo_controller.myTrans.position, Vector2.right * mayo_controller.myTrans.localScale.x, distance);
           
        }
        else
        {

        }*/
	}

    public void onDrawGismos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(mayo_controller.myTrans.position, mayo_controller.myTrans.position + Vector3.right * mayo_controller.myTrans.localScale.x * distance);
    }
}
