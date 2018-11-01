using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 */
public class SkillGestion : MonoBehaviour {

    public bool wall_running;
    private GameObject wall_slide_obj;
    private SamuraiController sc;
    void Start () {
        wall_slide_obj = GameObject.Find("Samuraï/WallSlide");
        if (wall_slide_obj == null)
            print("wall_slide_obj null");
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        //WallSlide = wall_slide;
    }
	
	void Update () {
		
	}


    //------------------------------------------------------------------------------------------------------------------------------------------------
    public bool WallRunning
    {
        get { return wall_running; }
        set { wall_running = value;
            if (value){ sc.jump_wall_cd = 0.1f; }
            else { sc.jump_wall_cd = 0.4f; }
            }
        //wall_slide_obj.SetActive(value);
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------
    public void LoadSkill()
    {

    }
}
