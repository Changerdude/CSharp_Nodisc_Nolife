using System;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    [SerializeField] private UIManager uiManager;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<PlayerMovement>();
        if (player) uiManager.Restart();

    }
}
