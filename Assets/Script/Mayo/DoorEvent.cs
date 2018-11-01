using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {

    public bool doorState = false; // false = fermée, true = ouverte
    public Sprite open;
    private Sprite closed;
    private const float doorMove= 0.48f;

    private Collider2D[] trigger;
    private Transform doorTrans;

	void Start () {
        trigger = this.GetComponents<BoxCollider2D>();
        closed = this.GetComponent<SpriteRenderer>().sprite;
        doorTrans = this.GetComponent<Transform>();
    }
	

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown("left ctrl"))
        {
            Debug.Log("Touching door");
            if (!doorState)
            {
                this.GetComponent<SpriteRenderer>().sprite = open;
                trigger[0].enabled = false;
                doorState = true;
                doorTrans.position = new Vector2(doorTrans.position.x - doorMove, doorTrans.position.y);
            }
            else
            {
                this.GetComponent<SpriteRenderer>().sprite = closed;
                trigger[0].enabled = true;
                doorState = false;
                doorTrans.position = new Vector2(doorTrans.position.x + doorMove, doorTrans.position.y);
            }
        }
    }

}
