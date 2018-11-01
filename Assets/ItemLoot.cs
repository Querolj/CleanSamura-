using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoot : MonoBehaviour {
    public bool sandal_wall_running;
    public GameObject anime_to_delete;

    private SkillGestion skill_gestion;
	void Start () {
        skill_gestion = GameObject.Find("Samuraï/SkillGestion").GetComponent<SkillGestion>();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (sandal_wall_running)
                skill_gestion.WallRunning = true;

            if (anime_to_delete != null)
                GameObject.Destroy(anime_to_delete);
            GameObject.Destroy(this.gameObject);
        }
    }
}
