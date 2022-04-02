using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Transform playerObject;
    private Rigidbody2D rigidbody;
    public LayerMask layermask;


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

}
