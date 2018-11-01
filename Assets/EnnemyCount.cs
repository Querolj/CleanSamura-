using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemyCount : MonoBehaviour {
    private int total;
    private int current;
    private bool update = false;
    private Objective obj;
    private Text display;
    void Start () {
        display = this.GetComponent<Text>();
        obj = GameObject.Find("GeneralSettings/Objective").GetComponent<Objective>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(update)
        {
            display.text = current.ToString() + " / " + total.ToString();
            update = false;
        }
        if(current == 0)
            obj.ObjectifCompleted();
	}

    public void SetTotal(int t)
    {
        total += t;
        current = total;
        update = true;
    }
    public void KillOne()
    {
        current -= 1;
        update = true;
    }
}
