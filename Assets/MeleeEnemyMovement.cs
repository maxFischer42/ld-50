using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMovement : EnemyBase
{
    public float distanceToPlayer = 20f;
    public float jumpingDistance = 5f;
    public float stopDistance = 0.2f;
    public float chaseSpeed = 5f;
    public float jumpForce = 200f;

    public float disToGround = 1.1f;
    public LayerMask groundLayermask;

    public bool isGrounded = false;

    public Animator a;
    public Animator a2;

    private Vector3 offsetA = new Vector3(-0.5f, 0f, 0f);
    private Vector3 offsetB = new Vector3(0.5f, -0.3f, 0f);

    private void Start()
    {
        SetPlayer(GameObject.Find("Player").transform);
        SetRigidBody(GetComponent<Rigidbody2D>());
        //a = transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GetRigidBody().velocity.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (GetRigidBody().velocity.x < 0) transform.localScale = new Vector3(1, 1, 1);

        bool p = isGrounded;
        isGrounded = SetGrounded();
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        // CHECK IF HUGGING A WALL
        var hit = Physics2D.OverlapArea(transform.position + offsetA, transform.position + offsetB, groundLayermask);
        if (hit)
        {
            transform.position += Vector3.up / 2;
            return;
        }


        if (playerInRange(stopDistance) && isGrounded)
        {
            GetRigidBody().velocity = Vector2.zero;
            a.enabled = true;
            a2.enabled = true;
            return;
        }
        a.enabled = false;
        a2.enabled = false;
        if (playerInRange(distanceToPlayer))
        {
            Vector2 dir = (GetPlayer().transform.position - transform.position).normalized;
            dir *= chaseSpeed;
            dir = new Vector2(dir.x, GetRigidBody().velocity.y);
            GetRigidBody().velocity = dir;

        }

        if (playerInRange(jumpingDistance) && !playerInRange(stopDistance) && isGrounded)
        {
            float a = Mathf.Abs((GetPlayer().transform.position.y - transform.position.y) * jumpForce);
            GetRigidBody().velocity = new Vector2(GetRigidBody().velocity.x, a);
            isGrounded = false;
        }
    }

    bool SetGrounded()
    {
        bool checkLand = false;
        if (!isGrounded)
        {

            checkLand = true;
        }
        var hit = Physics2D.Raycast(transform.position, Vector3.down, disToGround, groundLayermask);
        bool grounded = hit.collider;
        return grounded;
    }
}
