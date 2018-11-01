using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {
    private GameObject mC;
    private bool isTakingObject = false;
    // Use this for initialization
    void Start ()
    {
        mC = GameObject.Find("Mayo/MayoController");
    }

    public void setIsTakingObject(bool t)
    {
        isTakingObject = t;
    }

    // Update is called once per frame
    void Update () {

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("collision détectée");
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("PortableObjectS") && !isTakingObject)
        {
            
            mC.SendMessage("canTake", other.gameObject);
        }
    }

}
