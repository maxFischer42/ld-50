using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public EnemyBase enemyBase;
    private LayerMask myLayermask;
    public bool isColliding;
    public string collisionMessage;

    private void Start()
    {
        GetLayerMask();
    }

    void GetLayerMask()
    {
        myLayermask = enemyBase.layermask;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SendMessageUpwards("getCollision", collisionMessage);
        isColliding = true;
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == myLayermask.value)
            isColliding = false;
    }


}
