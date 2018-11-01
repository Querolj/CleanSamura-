using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {
    public Collider2D coll1;
    public Collider2D coll2;

    void Start () {
        Physics2D.IgnoreCollision(coll1, coll2);
	}
}
