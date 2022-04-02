using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private Rigidbody2D rb;
    private LineRenderer lr;
    private SpriteRenderer sr;
    private Animator a;

    //TODO change to have ground speed and air speed
    public float movementSpeed;
    public float jumpForce;
    public float jumpTime;

    public GameObject WeaponHolder;

    public bool isJumping;
    public bool isGrounded;

    private float jumpTimeCounter;

    public float disToGround;
    public LayerMask groundLayermask;

    public enum state { idle, move, jump, hurt};
    public state myState;

    public GameObject jumpEffect;
    public GameObject landEffect;
    public Transform feet;

    private float slopeFriction = 1f;

    bool isSlide = false;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = SetGrounded();
        doJump();
        HandleAnimations();
    }

    void FixedUpdate()
    {
        Move();
        //NormalizeSlope();
        HandleMouse();
    }

    void NormalizeSlope()
    {
        // Attempt vertical normalization
        if (isGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f, groundLayermask);

            if (hit.collider != null && Mathf.Abs(hit.normal.x) > 0.1f)
            {
                Rigidbody2D body = GetComponent<Rigidbody2D>();
                // Apply the opposite force against the slope force 
                // You will need to provide your own slopeFriction to stabalize movement
                body.velocity = new Vector2(body.velocity.x - (hit.normal.x * slopeFriction), body.velocity.y);

                //Move Player up or down to compensate for the slope below them
                Vector3 pos = transform.position;
                pos.y += -hit.normal.x * Mathf.Abs(body.velocity.x) * Time.deltaTime * (body.velocity.x - hit.normal.x > 0 ? 1 : -1);
                transform.position = pos;
            }
        }
    }

    void HandleAnimations()
    {
        if (!isGrounded)
        {
            myState = state.jump;
        }
        switch (myState)
        {
            case state.idle:
                a.SetBool("Idle", true);
                a.SetBool("Grounded", true);
                a.SetFloat("Xmove", -1);
                break;
            case state.move:
                a.SetBool("Idle", false);
                a.SetBool("Grounded", true);
                a.SetFloat("Xmove", 1);
                break;
            case state.jump:
                a.SetBool("Idle", false);
                a.SetBool("Grounded", false);
                a.SetFloat("Xmove", -1);
                break;
        }
    }

    void Move()
    {
        float x = getX();
        if (x != 0) MoveHorizontally(x);
        if(x == 0 && isGrounded) { myState = state.idle; }
    }

    void HandleMouse() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
        if(mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
            WeaponHolder.transform.localScale = new Vector2(-1, -1);
        } else if (mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
            WeaponHolder.transform.localScale = new Vector2(1, 1);
        }
    }

    void doJump() {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            if (isGrounded && myState != state.jump) { 
                GameObject j = (GameObject)Instantiate(jumpEffect, feet);
                j.transform.parent = null;
                Destroy(j, 0.5f);
            }
            myState = state.jump;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    bool SetGrounded() {
        bool checkLand = false;
        if(!isGrounded)
        {
            checkLand = true;
        }
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, new Vector2(transform.position.x, transform.position.y - disToGround));
        var hit = Physics2D.Raycast(transform.position, Vector3.down, disToGround, groundLayermask);
        bool grounded = hit.collider;
        if(grounded && checkLand)
        {
            GameObject j = (GameObject)Instantiate(landEffect, feet);
            j.transform.parent = null;
            Destroy(j, 0.5f);
        }
        return grounded;
    }

    void MoveHorizontally(float i)
    {
        float newX = movementSpeed;
        if(isGrounded) { myState = state.move; }
        rb.velocity = new Vector2(i * newX, rb.velocity.y);
    }

    public float getX() { return Input.GetAxis("Horizontal"); }
    public float getYX() { return Input.GetAxis("Vertical"); }
    public bool getAttack() { return Input.GetButton("Fire1"); }

}
