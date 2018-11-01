using UnityEngine;
using System.Collections;
/*
 * instructions : 
 * 'r' = movingPNJ(1)
 * 'l' = movingPNJ(-1)
 * 
 * Fonctions :
 * movingPNJ(int dir) = Bouge un PNJ à droite (1) ou à gauche (-1)
 * 
 * otherObject.GetComponent(NameOfScript).enabled = false;
 * 
 */
public class ScriptedMove : MonoBehaviour {
    private Rigidbody2D charBody;
    private Collider2D charCollider;
    private Transform charTrans;
    private float angle;

    //Waiting
    private bool wait = false; // waiting on/off
    private float chrono;
    private float timeWaited;

    //Instructions control
    public Collider2D[] scriptCollider;
    public string[] instructions;

    //Directions
    public float speed = 3;

    //Fov
    public Transform sightStart, sightEnd;
    public bool spotted = false;

	void Start () {
        charBody = this.GetComponent<Rigidbody2D>();
        charTrans = this.GetComponent<Transform>();
        charCollider = this.GetComponent<Collider2D>();

    }
	
    void Update()
    {
        /*
         * Field of view
         */
        raycasting();
        behaviours();


        /*
         * Instructions control 
         */
        /*instructionChoice(instructions[i]);
            
        if (touchTrigger())
        {
            if (!colliding&&i<=instructions.Length)
            {
                Debug.Log("temps actuel : " + Time.time);
                i++;
                colliding = true;
            }
        }else
        {
            colliding = false;
        }*/
        angleCalcul();
    }


    public void movingPNJ(int dir) 
    {
        Vector2 moveVel = charBody.velocity;
        moveVel.x = dir * speed;


        if (((charBody.velocity.y < 0) || (charBody.velocity.y > 0)) && angle > 1)
        {
            // DescendSlope(moveVel);
            charBody.velocity = moveVel;
        }
        else
        {
            charBody.velocity = moveVel;
        }
    }

    public void instructionChoice(string i)
    {
        if(i.Equals("r"))
        {
            movingPNJ(1);
        }
        else if(i.Equals("l"))
        {
            movingPNJ(-1);
        }
        else
        {
            waiting(float.Parse(i));
        }
    }
    
    public void waiting(float ms)
    {
        if(!wait)
        {
            chrono = Time.time;
            timeWaited = ms;
            wait = true;
        }
        else
        {
            if(Time.time>chrono+timeWaited)
            {
                //Stop waiting
            }
        }
        
    }
    public bool touchTrigger()
    {
        for (int i = 0; i < scriptCollider.Length; i++)
        {
            if (charCollider.IsTouching(scriptCollider[i]))
                return true;
        }
        return false;
    }
    public void angleCalcul()
    {
        RaycastHit2D[] hits = new RaycastHit2D[3];
        int h = Physics2D.RaycastNonAlloc(charTrans.position, -Vector2.up, hits);
        if (h > 1)
        { //if we hit something do stuff

            angle = Mathf.Abs(Mathf.Atan2(hits[2].normal.x, hits[2].normal.y) * Mathf.Rad2Deg); //get angle

            if (angle > 1)
            {
                Debug.Log("Angle :" + angle);

            }
        }
    }
    public void antiSlide()
    {
        Vector2 noSlide = charBody.velocity;

        /*if (!moving && isGround)
        {
            charBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            charBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }*/

    }


    /*
     * Champs de vision
     */

    public void raycasting()
    {
        Debug.DrawLine(sightStart.position, sightEnd.position, Color.green);
        spotted = Physics2D.Linecast(sightStart.position, sightEnd.position,1 << LayerMask.NameToLayer("Player")); 


    }

    public void behaviours()
    {

    }


}
