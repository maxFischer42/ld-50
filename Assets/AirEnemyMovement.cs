using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemyMovement : EnemyBase
{
    public float distanceToPlayer = 35f;
    public float stopDistance = 9f;
    public float xSpeed = 5f;
    public float ySpeed = 3f;

    private void Start()
    {
        SetPlayer(GameObject.Find("Player").transform);
        SetRigidBody(GetComponent<Rigidbody2D>());
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (playerInRange(stopDistance))
        {
            GetRigidBody().velocity = Vector2.zero;
            return;
        }

        if (playerInRange(distanceToPlayer))
        {
            Vector2 dir = (GetPlayer().transform.position - transform.position).normalized;
            dir = new Vector2(dir.x * xSpeed, dir.y * ySpeed);
            GetRigidBody().velocity = dir;

        }
    }
}
