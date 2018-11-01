using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationTrigger : MonoBehaviour {
    public int orientation;

    private FireballVocano fireball;

	void Start () {
        fireball = this.GetComponentInParent<FireballVocano>();		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Surface" && !fireball.IsCollEntered())//&& fireball.GetOrientation() == -1
        {
            fireball.SetOrientation(orientation);
            switch (orientation)
            {
                case 0:
                    fireball.north = true;
                    break;
                case 1:
                    fireball.est = true;
                    break;
                case 3:
                    fireball.west = true;
                    break;
                default:
                    fireball.south = true;
                    break;
            }
            //fireball.SurfaceCollision();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Surface")//&& fireball.GetOrientation() == -1
        {
            fireball.SetOrientation(orientation);
            switch (orientation)
            {
                case 0:
                    fireball.north = false;
                    break;
                case 1:
                    fireball.est = false;
                    break;
                case 3:
                    fireball.west = false;
                    break;
                default:
                    fireball.south = false;
                    break;
            }
            //fireball.SurfaceCollision();
        }
    }
}
