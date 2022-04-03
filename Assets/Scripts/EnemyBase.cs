using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBase : MonoBehaviour
{
    private Transform playerObject;
    private Rigidbody2D rigidbody;
    public LayerMask layermask;
    public bool hasAI = false;

    public SpriteRenderer r;


    public void SetPlayer(Transform _object)
    {
        playerObject = _object;
    }

    public Transform GetPlayer() {
        return playerObject;
    }

    public void SetRigidBody(Rigidbody2D rb)
    {
        rigidbody = rb;
    }

    public Rigidbody2D GetRigidBody()
    {
        return rigidbody;
    }
    public bool playerInRange(float type)
    {
        if ((GetPlayer().transform.position - transform.position).magnitude <= type)
        {
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        if(hasAI) r.flipX = (this.gameObject.GetComponent<IAstarAI>().velocity.x < 0 ? false : true);
    }

}
