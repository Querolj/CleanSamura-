using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    bool object_detected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!object_detected && LayerMask.LayerToName(other.gameObject.layer).Equals("PortableObjectS"))
        {
            object_detected = true;
            //mayo_controller.setObjectLocked(other.gameObject, other.gameObject.transform, false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).Equals("PortableObjectS"))
        {
            object_detected = false;
            //mayo_controller.setObjectLocked(null, null, true);
        }
    }
}
