using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public Transform player;
    public Vector3 offset;
    public float limite_right;
    public float limite_left;
    public float limit_up;
    public float limit_down;

    void Start()
    {
        SetPosition();
    }

    void Update()
    {
        SetPosition();
    }

    bool LimiteRL()
    {
        return limite_right > player.position.x && limite_left < player.position.x;
    }
    bool LimiteUD()
    {
        return limit_up > player.position.y && limit_down < player.position.y;
    }

    public void SetPosition()
    {
        if (LimiteRL() && LimiteUD())
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
            return;
        }
        float x = player.position.x + offset.x;
        float y = player.position.y + offset.y;

        if (!(limit_down < player.position.y))
            y = limit_down + offset.y;
        else if(!(limit_up > player.position.y))
            y = limit_up + offset.y;
        if (!(limite_right > player.position.x))
            x = limite_right + offset.x;
        else if (!(limite_left < player.position.x))
            x = limite_left + offset.x;
        
        transform.position = new Vector3(x, y, offset.z);
    }
}
