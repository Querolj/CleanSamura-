using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingZoneE : MonoBehaviour {
    private EnnemiAI AI;
    protected Transform h_trans;
    protected BoxCollider2D h_collider;
    public float pos_x_right;
    public float pos_x_left;
    public bool directed;

    public Vector2 offset_right;
    public Vector2 offset_left;
    public Vector2 size_right;
    public Vector2 size_left;
    //Movements
    protected bool changed_direction = false;
    void Start () {
        h_collider = this.GetComponent<BoxCollider2D>();
      
        AI = this.GetComponentInParent<EnnemiAI>();
        h_trans = this.GetComponent<Transform>();
 
        CustomStart();
        StartSetDirection();
    }
    void StartSetDirection()
    {
        if (CustomChangeHitZone()) { }
        else if (AI.direction)
        {
            Vector3 v = h_trans.localPosition;
            if (AI.is_ranged || pos_x_right != 0)
                v.x = pos_x_right;//2.018f;
            else
                v.x = 0.315f;
            h_trans.localPosition = v;
            if (offset_right != Vector2.zero && size_right != Vector2.zero)
            {
                h_collider.offset = offset_right;
                h_collider.size = size_right;
            }
        }
        else
        {
            Vector3 v = h_trans.localPosition;
            if (AI.is_ranged || pos_x_left != 0)
                v.x = pos_x_left;// - 0.044f;
            else
                v.x = 0;
            h_trans.localPosition = v;
            if (offset_right != Vector2.zero && size_right != Vector2.zero)
            {
                h_collider.offset = offset_left;
                h_collider.size = size_left;
            }

        }
        changed_direction = AI.direction;
    }

    void Update () {
        SetDirection();
    }
    
    void SetDirection()
    {
        if (CustomChangeHitZone()) { }
        else if (AI.direction != changed_direction && AI.direction)
        {
            Vector3 v = h_trans.localPosition;
            if (AI.is_ranged || pos_x_right != 0)
                v.x = pos_x_right;//2.018f;
            else
                v.x = 0.315f;
            h_trans.localPosition = v;
            if(offset_right != Vector2.zero && size_right != Vector2.zero)
            {
                h_collider.offset = offset_right;
                h_collider.size = size_right;
            }
        }
        else if (AI.direction != changed_direction)
        {
            Vector3 v = h_trans.localPosition;
            if (AI.is_ranged || pos_x_left != 0)
                v.x = pos_x_left;
            else
                v.x = 0;
            h_trans.localPosition = v;
            if (offset_right != Vector2.zero && size_right != Vector2.zero)
            {
                h_collider.offset = offset_left;
                h_collider.size = size_left;
            }
                
        }
        changed_direction = AI.direction;
    }

    private string IsClone()
    {
        string s = this.gameObject.GetComponentInParent<Rigidbody2D>().gameObject.name;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(')
                return s;
        }
        return null;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AI.in_range_for_attack = true;
        }
        if (other.gameObject.tag == "Surface")
            AI.SetLookAtWall(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AI.in_range_for_attack = false;
        }
        if (other.gameObject.tag == "Surface")
            AI.SetLookAtWall(false);
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------
    protected virtual void CustomStart() { }

    protected virtual bool CustomChangeHitZone()
    {
        return false;
    }
}
