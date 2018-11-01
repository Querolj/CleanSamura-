using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * TODO :
 *   
 */
public class WolfController : MonoBehaviour {
    private Rigidbody2D wBody;
    private Transform wTrans;
    private Transform mobsTrans;
    private MayoController mc;
    private Transform sight_start;
    private CircleCollider2D circle_detector;
    private bool force_barking = false;

    public float walking_speed = 2;
    public float running_speed = 6;
    public float initial_view_range_x = 4f;
    [HideInInspector]
    public float view_range_x = 4f;
    public float view_range_y = 0.8f;
    public float view_range_x_pursuit = 7f; // Added to view_range_y
    public float view_range_x_waiting = 3f; // Subsracted to view_range_y
    public int mode = 0; // 0= patrouille, 1= poursuite, 2= barking, 3 waiting
    public bool circle_detect_mode = false;
    public Transform[] waypoints;


    public bool directed; //true= droite
    public bool slepping = false;
    private int current_waypoint;
    void Start () {
        mc = GameObject.Find("Mayo").GetComponent<MayoController>();
        wBody = this.GetComponent<Rigidbody2D>();
        wTrans = this.GetComponent<Transform>();
        sight_start = GameObject.Find(this.name + "sight_start").transform;
        mobsTrans = GameObject.Find("Mobs").GetComponent<Transform>();
        if (circle_detect_mode)
            circle_detector = this.GetComponent<CircleCollider2D>();

        current_waypoint = 0;
        view_range_x = initial_view_range_x;
    }
	
	void Update () {

        Vector3 position_range = sight_start.position;
        RaycastHit2D ray;
        UpdateViewRange();
        if(!directed)
        {
            Vector2 v = sight_start.localPosition;
            v.x = -0.9f;
            sight_start.localPosition = v;
            position_range.x -= view_range_x;
            Debug.DrawLine(sight_start.position, position_range, Color.red);
        }
        else
        {
            Vector2 v = sight_start.localPosition;
            v.x = 1.8f;
            sight_start.localPosition = v;
            position_range.x += view_range_x;
            Debug.DrawLine(sight_start.position, position_range, Color.red);
        }
        if (!slepping && (ray = Physics2D.Linecast(sight_start.position, position_range)) && ((ray.transform.gameObject.layer == LayerMask.NameToLayer("Player")) ||
            (ray.transform.gameObject.layer == LayerMask.NameToLayer("Player") && mc.object_locked != null)) )
        {
            Pursuit();
        }
        else if(force_barking)
        {
            Barking();
        }
        else if(mode == 1)
        {
            mode = 2;
            Barking();
        }
        else if(mode == 2)
        {
            Barking();
        }
        else if(!slepping && mode != 3)
        {
            patrolWaypoint();
        }
        else if(mode == 3)
        {
            Waiting();
        }

        if(mode != 2 && circle_detect_mode)
        {
            if(!circle_detector.isActiveAndEnabled)
                circle_detector.enabled = true;
        }
            
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void UpdateViewRange()
    {
        switch(mode)
        {
            case 0:
                view_range_x = initial_view_range_x;
                break;
            case 1:
                view_range_x = view_range_x_pursuit;
                break;
            case 2:
                view_range_x = view_range_x_pursuit;
                break;
            case 3:
                view_range_x = view_range_x_waiting;
                break;
            default:
                view_range_x = initial_view_range_x;
                break;
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void Move(float direction)
    {
        Vector2 moveVel = wBody.velocity;
        if(mode == 1)
            moveVel.x = running_speed * direction;
        else
            moveVel.x = walking_speed*direction;
        if (direction > 0)
            directed = true;
        else if (direction < 0)
            directed = false;
        if ((wBody.velocity.y < 0) || (wBody.velocity.y > 0))
        {
            wBody.velocity = moveVel;
        }
        else
        {
            wBody.velocity = moveVel;
        }
        
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void patrolWaypoint()
    {
        if (mode != 0)
        {
            mode = 0;
            UpdateViewRange();
        }
            
        float diff;
        if(current_waypoint < waypoints.Length)
        {
            //Vector3 waypoint = waypoints[current_waypoint].TransformPoint(new Vector3(1, 1, 1));
        }
            
        if (current_waypoint < waypoints.Length)
            diff = waypoints[current_waypoint].position.x - wTrans.position.x;
        else
        {
            diff = waypoints[0].position.x - wTrans.position.x;
            current_waypoint = 0;
        }
            
        if (diff < 0.1 && diff > -0.1)
        {
            if(waypoints.Length > current_waypoint)
            {
                current_waypoint++;
            }
            else
            {
                current_waypoint = 0;
            }
        }
        else if (diff > 0)
        {
            Move(1);
        }
        else if (waypoints[current_waypoint].position.x - wTrans.position.x < 0)
        {
            Move(-1);
        }
        else
        {
            Debug.Log("Plus de waypoints");
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void Pursuit()
    {
        Debug.Log("Pursuit");
        //Vector3 worldWTrans = mobsTrans.TransformPoint(wTrans.position);
        float direction = mc.myTrans.position.x - wTrans.position.x;
        //Debug.Log("direction : " + direction);
        if (mode != 1)
            mode = 1;
        if (force_barking)
            force_barking = false;
        if (direction > 0)
            Move(1);
        else
            Move(-1);

    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void Barking()
    {
        Debug.Log("Barking");
        if (mode != 2)
        {
            mode = 2;
            UpdateViewRange();
        }
            
        Vector3 worldWTrans = mobsTrans.TransformPoint(wTrans.position);
        float direction = mc.myTrans.position.x - wTrans.position.x;
        float range = 1.2f;
        
        if ((Math.Abs(wTrans.position.x - mc.myTrans.position.x) > range) ) //Anti trisomiquage
        {
            //Debug.Log("anti triso : "+ Math.Abs(worldWTrans.x - mc.myTrans.position.x)+" w : "+ worldWTrans.x+", m"+ mc.myTrans.position.x);
            if (direction > 0)
                Move(1);
            else if (direction < 0)
                Move(-1);
        }

        //Debug.Log("w f : " + worldWTrans.y + ", mayo : " + mc.myTrans.position.y);
        if (worldWTrans.y + 2.5f < mc.myTrans.position.y )
        {
            Debug.Log("stop bark");
            mode = 0;
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    private void Waiting()
    {
        if (mode != 3)
        {
            mode = 3;
            view_range_x -= view_range_x_waiting;
            wBody.velocity = new Vector2(0, 0);
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("Forêt");
            //mc.respawn();
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (circle_detect_mode && LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            mode = 2;
            if (circle_detector.isActiveAndEnabled)
                circle_detector.enabled = false;
        }
    }
    /*void OnTriggerExit2D(Collider2D other)
    {
        if (circle_detect_mode && LayerMask.LayerToName(other.gameObject.layer).Equals("Player"))
        {
            if (mode == 2)
                mode = 0;
            if (!circle_detector.isActiveAndEnabled)
                circle_detector.enabled = true;
        }
    }*/

}
