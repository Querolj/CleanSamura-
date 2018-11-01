using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLevier : MonoBehaviour {
    public Levier levier;
    public bool opened = false;
    public Sprite SpriteOpen;
    public Sprite SpriteClosed;

    private SpriteRenderer renderer;
    private bool old_on_off;
    private Collider2D door_collider;
    void Start () {
        if (levier != null)
            old_on_off = levier.GetOnOff();
        door_collider = this.GetComponent<Collider2D>();
        renderer = this.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		if(levier.GetOnOff() != old_on_off && levier.GetActivate())
        {
            if (!opened)
                OpenDoor();
            else
                CloseDoor();
            old_on_off = levier.GetOnOff();
        }
	}
    public void OpenDoor()
    {
        opened = true;
        door_collider.enabled = false;
        renderer.sprite = SpriteOpen;
        renderer.sortingOrder = -1;
    }
    public void CloseDoor()
    {
        opened = false;
        door_collider.enabled = true;
        renderer.sprite = SpriteClosed;
        renderer.sortingOrder = 0;
    }
}
