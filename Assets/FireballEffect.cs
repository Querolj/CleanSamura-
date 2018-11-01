using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballEffect : MonoBehaviour {

    private int orientation = -1; //0=N, 1=E, 2=S, 3=W
    void Start () {
		
	}
	
	void Update () {
		
	}



    //---------------------------------------------------------------------------------------------------------
    public void SetOrientation(int i)
    {
        orientation = i;
    }
    public int GetOrientation()
    {
        return orientation;
    }
}
