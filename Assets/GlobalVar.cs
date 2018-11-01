using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVar {

	public static bool OnGroundCondition(GameObject other)
    {
        return (other.tag == "Surface" || other.tag == "UnusualSurface");
    }
}
