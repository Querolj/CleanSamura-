using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballVocano : MonoBehaviour {
    
    public int projectile_strenght = 1;
    public float throw_drawnback = 1.5f;
    public float fire_time;

    private SamuraiController sc;
    private int orientation = -1; //0=N, 1=E, 2=S, 3=W
    private Transform t;
    private Rigidbody2D body;
    private ShortAnimation anime;
    private ShortAnimation landing_anime;
    private bool coll_entered = false;
    //Direction
    [HideInInspector]
    public bool north = false;
    [HideInInspector]
    public bool south = false;
    [HideInInspector]
    public bool west = false;
    [HideInInspector]
    public bool est = false;
    void Start () {
        sc = GameObject.Find("Samuraï").GetComponent<SamuraiController>();
        body = this.GetComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezeRotation;

        anime = this.GetComponentsInChildren<ShortAnimation>()[0];
        landing_anime = this.GetComponentsInChildren<ShortAnimation>()[1];
        if (landing_anime == null)
            print("landing_anime null");
        else
            landing_anime.enabled = false;
        if (fire_time <= 0)
            print("fire_time set to 0");
        t = this.GetComponent<Transform>();
    }
	
	void Update () {
        if(coll_entered)
        {
            body.constraints = RigidbodyConstraints2D.FreezePosition;
            SurfaceCollision();
            if (fire_time > 0)
                fire_time -= Time.deltaTime;
            else
                GameObject.Destroy(this.gameObject);
        }
        
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            sc.ReceiveDamage(projectile_strenght, throw_drawnback, sc.GetDirection());
            GameObject.Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Surface" && !coll_entered)
        {
            body.constraints = RigidbodyConstraints2D.FreezePosition;
            SurfaceCollision();
        }
    }

    public void SurfaceCollision()
    {
        coll_entered = true;
        anime.enabled = false;
        landing_anime.enabled = true;
        //Look orientation

        if(north && est && west)
        {
            EulerAngle(180);
        }
        else if(est)
        {
            EulerAngle(90);
        }
        else if(west)
        {
            EulerAngle(270);
        }
        else if(north)
        {
            EulerAngle(180);
        }
        /*
        switch(orientation)
        {
            case 0:
                //q.z = 180;
                EulerAngle(180);
                //t.Rotate(Vector3.forward * 180);
                break;
            case 1:
                //t.Rotate(Vector3.forward * 90);
                EulerAngle(90);
                break;
            case 2:
                //t.Rotate(Vector3.forward * 1);
                EulerAngle(0);
                break;
            case 3:
                //t.Rotate(Vector3.forward * 270);
                EulerAngle(270);
                break;
            default:
                print("Pas d'orientation détectée");
                break;
        }
        */
    }
    private void EulerAngle(float z)
    {
        t.eulerAngles = new Vector3(0, 0, z);
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
    public bool IsCollEntered()
    {
        return coll_entered;
    }
}
