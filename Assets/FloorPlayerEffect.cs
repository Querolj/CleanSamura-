using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPlayerEffect : MonoBehaviour {
    //private SamuraiController sc;
    public GameObject short_anime;

    private int count_clean_done = 0;
    private GameObject to_trash;
    [HideInInspector]
    public bool clean_done;
	
	void Update () {

        if(clean_done)
        {
            to_trash = Instantiate(short_anime, this.transform.position, Quaternion.identity);
            clean_done = false;
            Invoke("DeleteAnime", 0.5f);
        }

    }

    private void DeleteAnime()
    {
        GameObject.Destroy(to_trash);
    }
    public void SetAnimationOnRenderer(SpriteRenderer sp, bool b)//Fait l'animation et détruit le gameobject du spriterenderer
    {
        clean_done = b;
    }
}
