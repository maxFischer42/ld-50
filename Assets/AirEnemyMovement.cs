using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AirEnemyMovement : EnemyBase
{
    public float distanceToPlayer = 35f;
    public float stopDistance = 9f;
    public float xSpeed = 5f;
    public float ySpeed = 3f;
    public Transform target;
    IAstarAI ai;

    private void OnEnable()
    {
        target = GameObject.Find("Player").transform;
        SetRigidBody(GetComponent<Rigidbody2D>());
        ai = GetComponent<IAstarAI>();
        if (ai != null) ai.onSearchPath += Update;
    }

    void OnDisable()
    {
        if (ai != null) ai.onSearchPath -= Update;
    }

    /// <summary>Updates the AI's destination every frame</summary>
    void Update()
    {
        if (target != null && ai != null) ai.destination = target.position;
    }

}
