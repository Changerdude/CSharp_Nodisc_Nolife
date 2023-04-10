using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    public float offsetY;
    private float minX = -9.62f;
    private float maxX = 168.7f;
    public Transform player;
    public Vector3 offset;

    void Update()
    {
        if(player.position.y == -1.622973 || offsetY < 0)
        {
            offsetY = offset.y;
        }
        if(Mathf.Abs(player.position.y - offsetY) > 2)
        {
            offsetY = Mathf.Floor(player.position.y + offset.y);
        }
        transform.position = new Vector3(player.position.x + offset.x, offsetY, offset.z);
        if(transform.position.x < minX)
        {
            transform.position = new Vector3(minX, offsetY, offset.z);
        }
        if(transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, offsetY, offset.z);
        }
    }
}
