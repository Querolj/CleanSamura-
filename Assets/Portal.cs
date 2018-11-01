using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Portal other_tp;
    [HideInInspector]
    public GameObject ignored_obj;
    public float tp_time = 1f;
    public FollowPlayer fp;

    private Transform trans;
    private Transform trans_to_anime;
    private Vector3 init_pos;
    private SpriteRenderer sprite_to_anime;
    private Quaternion q;
    private Rigidbody2D rb;
  
    private bool anime_running = false;
    private float anime_time;
    private float anime_rotation;
    
    void Start () {
        trans = this.GetComponent<Transform>();
        ignored_obj = null;
        trans_to_anime = null;
        anime_time = tp_time;
        
    }
	
	void Update () {
	    if(anime_running)
        {
            anime_time -= Time.deltaTime;
            
            Animation();
        }
        if(anime_running && anime_time <= 0)
        {
            anime_time = tp_time;
            anime_running = false;
            trans_to_anime.localScale = new Vector3(1, 1, 1);
            
            trans_to_anime.rotation = q;
            sprite_to_anime.color = new Color(1, 1, 1, 1);
            other_tp.ignored_obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            Vector3 v = other_tp.GetLocation();
            v.z = 0;
            fp.SetPosition();
            trans_to_anime.position = v;
        }
	}

    private void Animation()
    {
        Vector3 size = trans_to_anime.localScale;
        Quaternion rotation = trans_to_anime.rotation;
        Color c = sprite_to_anime.color;
        size.x -= 0.03f;
        size.y -= 0.03f;
        //anime_rotation += Time.deltaTime ;
        rotation.z -= 0.03f;
        trans_to_anime.Rotate(Vector3.forward* 100);
        c.a -= 0.005f;

        sprite_to_anime.color = c;
        trans_to_anime.localScale = size;
        trans_to_anime.rotation = rotation;
    }


    public Vector3 GetLocation()
    {
        return trans.position;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {

        if((other.tag == "Player" || other.tag == "Ennemi") && ignored_obj == null && !anime_running)
        {
            anime_running = true;
            other_tp.ignored_obj = other.gameObject;
            lock(other_tp.ignored_obj)
            {
                trans_to_anime = other_tp.ignored_obj.GetComponent<Transform>();
                init_pos = trans_to_anime.position;
                other_tp.ignored_obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                sprite_to_anime = other_tp.ignored_obj.GetComponent<SpriteRenderer>();
                sprite_to_anime.color = new Color(0, 0, 0, 1);
                q = trans_to_anime.rotation;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Ennemi")
        {
            if(ignored_obj == other.gameObject)
            {
                lock(ignored_obj)
                {
                    ignored_obj = null;
                }
                
            }
        }
    }
}
