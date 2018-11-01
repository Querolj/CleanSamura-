using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : MonoBehaviour {
    public bool can_add = true; //true : l'objet peut être rangé
    public Sprite inventory_sprite;
    public string item_name = "";
    public string description = "";
    public int weight = 0;
    public bool usable;
    public bool throwable;

    [HideInInspector]
    public int place;
    [HideInInspector]
    public GameObject game_object;
    [HideInInspector]
    public Transform obj_trans;
    [HideInInspector]
    public Rigidbody2D obj_rb;
    [HideInInspector]
    public Collider2D obj_col;
    private Collider2D mcol;
    void Start()
    {
        game_object = this.gameObject;
        obj_trans = gameObject.transform;
        obj_rb = gameObject.GetComponent<Rigidbody2D>();
        obj_col = gameObject.GetComponent<Collider2D>();
        mcol = GameObject.Find("Mayo").GetComponent<Collider2D>();
    }
	public void Interaction()
    {
        //inventory_sprite = this.GetComponent<SpriteRenderer>().sprite;
        this.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            Physics2D.IgnoreCollision(obj_col, mcol, false);
        }

    }
}
