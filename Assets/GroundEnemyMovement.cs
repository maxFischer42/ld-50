using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyMovement : EnemyBase
{

    public float distanceToPlayer = 20f;
    public float jumpingDistance = 5f;
    public float stopDistance = 2f;
    public float chaseSpeed = 5f;
    public float jumpForce = 200f;

    public float disToGround = 1.1f;
    public LayerMask groundLayermask;

    public bool isGrounded = false;

    private void Start()
    {
        SetPlayer(GameObject.Find("Player").transform);
        SetRigidBody(GetComponent<Rigidbody2D>());
    }

    // Update is called once per frame
    void Update()
    {
        bool p = isGrounded;
        isGrounded = SetGrounded();
        MoveTowardsPlayer();
    }

    private Vector3 offsetA = new Vector3(-0.7f, 0f, 0f);
    private Vector3 offsetB = new Vector3(0.7f, -0.7f, 0f);

    void MoveTowardsPlayer()
    {
        // CHECK IF HUGGING A WALL
        var hit = Physics2D.OverlapArea(transform.position + offsetA, transform.position + offsetB, groundLayermask);
        if(hit)
        {
            transform.position += Vector3.up / 2;
            return;
        }

        if(playerInRange(stopDistance) && isGrounded)
        {
            GetRigidBody().velocity = Vector2.zero;
            return;
        }

        


        if(playerInRange(jumpingDistance) && !playerInRange(stopDistance) && GameObject.Find("Player").transform.position.y - transform.position.y > 5f)
        {
            //float a = Mathf.Abs((GetPlayer().transform.position.y - transform.position.y) * jumpForce);
            GetRigidBody().velocity = new Vector2(GetRigidBody().velocity.x, jumpForce);
            isGrounded = false;
        } else
        {
            Vector2 dir = (GetPlayer().transform.position - transform.position).normalized;
            dir *= chaseSpeed;
            dir = new Vector2(dir.x, GetRigidBody().velocity.y);
            GetRigidBody().velocity = dir;
        }
        /*

        else if(GetRigidBody().velocity.y < 0 && isJumping)
        {
            Vector2 dir = (GetPlayer().transform.position - transform.position).normalized;
            dir *= chaseSpeed;
            GetRigidBody().velocity = new Vector2(dir.x, GetRigidBody().velocity.y);
        }*/
    }

    bool SetGrounded()
    {
        bool checkLand = false;
        if (!isGrounded)
        {

            checkLand = true;
        }
        //lr.SetPosition(0, transform.position);
        //lr.SetPosition(1, new Vector2(transform.position.x, transform.position.y - disToGround));
        var hit = Physics2D.Raycast(transform.position, Vector3.down, disToGround, groundLayermask);
        bool grounded = hit.collider;
        return grounded;
    }
}
