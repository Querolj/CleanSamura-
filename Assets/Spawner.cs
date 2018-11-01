using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public GameObject ennemy;
    public int max_spawn;
    public int spawnerID = 0;
    public bool infinite_spawn = false;
    public bool is_objective = false;

    private Transform trans;
    private EnnemiStates current_ennemy_st;
    //private SamuraiController sc;
    private int number_of_spawn = 0;
    [HideInInspector]
    public bool spawn = true;
    public float time_unil_spawn = 1f;
    private string obj_name;
    private string org_name;
    private bool stop = false;

    //ennemy characteristics
    public Transform[] waypoints;
    //display
    private EnnemyCount ec;
    void Start () {
        trans = this.GetComponent<Transform>();
        //sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        if(is_objective)
        {
            //sc.SpawnCount();
            ec = GameObject.Find("GeneralSettings/LifeDisplay/WavesDisplay/EnnemiesNumber").GetComponent<EnnemyCount>();
            ec.SetTotal(max_spawn + 1);
        }
        
    
        if (is_objective && infinite_spawn)
            Debug.LogError("spawn infini et objectif true impossible");
             
    }
	
	void Update () {
        if(!stop)
        {
            if (spawn)
            {
                if (number_of_spawn == 0)
                {
                    obj_name = ennemy.name;
                    org_name = obj_name;
                }
                else
                {
                    obj_name = org_name;
                }
                obj_name += spawnerID.ToString() + "_";
                obj_name += number_of_spawn.ToString();
                ennemy.name = obj_name;
                GameObject e = Instantiate(ennemy, trans.position, Quaternion.identity);
                if(waypoints.Length !=0)
                    current_ennemy_st = e.GetComponent<EnnemiStates>();
                e.GetComponent<EnnemiAI>().waypoints = waypoints;
                e.SetActive(true);
                spawn = false;
            }
            if (current_ennemy_st != null && current_ennemy_st.IsDead() && (number_of_spawn < max_spawn || infinite_spawn)) //Un mort
            {
                current_ennemy_st = null;
                Invoke("SpawnEnnemy", time_unil_spawn);
                spawn = false;
                //if(!infinite_spawn)
                number_of_spawn++;
                if(is_objective)
                    ec.KillOne();
            }

            if (!infinite_spawn && current_ennemy_st != null && number_of_spawn >= max_spawn && current_ennemy_st.IsDead())
            {
                stop = true;
                if (is_objective)
                    ec.KillOne();
            }
        }
	}

    public void SpawnEnnemy()
    {
        spawn = true;
    }
}
