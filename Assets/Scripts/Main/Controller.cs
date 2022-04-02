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

    public enum state { idle, move, jump, hurt, roll};
    public state myState;

    public GameObject jumpEffect;
    public GameObject landEffect;
    public Transform feet;

    private float slopeFriction = 1f;

    bool isSlide = false;

    bool canRoll = true;

    public UpgradeManager upgrade;

    public float rollSpeed;

    private PlayerHealthManager health;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
        health = GetComponent<PlayerHealthManager>();
        UpgradeCheck();
    }

    int dir = 1;

    void Update()
    {
        if (!health.canMove) return;
        if (rb.velocity.x == 0 && isGrounded && myState == state.roll) { myState = state.idle; }
        isGrounded = SetGrounded();
        if(myState != state.roll) doJump();
        HandleAnimations();
    }

    void FixedUpdate()
    {
        if (getRoll() && canRoll) { Roll(); StartCoroutine(startRollCooldown()); }
        if(myState == state.roll) return;
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
        if (!isGrounded && myState != state.roll)
        {
            myState = state.jump;
        }
        switch (myState)
        {
            case state.idle:
                a.SetBool("Idle", true);
                a.SetBool("Roll", false);
                a.SetBool("Grounded", true);
                a.SetFloat("Xmove", -1);
                break;
            case state.move:
                a.SetBool("Idle", false);
                a.SetBool("Roll", false);
                a.SetBool("Grounded", true);
                a.SetFloat("Xmove", 1);
                break;
            case state.jump:
                a.SetBool("Idle", false);
                a.SetBool("Roll", false);
                a.SetBool("Grounded", false);
                a.SetFloat("Xmove", -1);
                break;
            case state.roll:
                a.SetBool("Idle", false);
                a.SetBool("Roll", true);
                a.SetBool("Grounded", true);
                a.SetFloat("Xmove", -1);
                break;
        }
    }

    void Move()
    {
        if(myState == state.roll) return;
        float x = getX();
        if (x != 0) dir = (int)x;
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

    public void Roll()
    {
        float x = dir;
        if (isGrounded) {
            myState = state.roll;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(x * rollSpeed, 0f));
        }    
    }

    IEnumerator startRollCooldown()
    {
        canRoll = false;
        yield return new WaitForSeconds(rollCooldown - rollOffset);
        canRoll = true;
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
    public bool getRoll() { return Input.GetButtonDown("Roll"); }

    public float fallOffset = 0f;
    public float jumpOffset = 0f;
    public float speedIncrease = 0f;
    public float rollOffset = 0f;
    public float rollCooldown = 1.5f;

    public float gravity = 2;
    public float rollTimer = 3f;
    public float jump = 10;
    public float speed = 10;

    public void UpgradeCheck()
    {
        float f = upgrade.fallDecrease;
        if (f >= 1) f = 1;
        float j = upgrade.jumpIncrease;
        if (j >= 5) j = 5;
        float s = upgrade.movementIncrease;
        if (s >= 2) s = 2;
        float r = upgrade.rollDecrease;
        if (r >= 1) r = 1;

        rb.gravityScale = gravity - f;
        jumpForce = jump + j;
        rollOffset = r;
        movementSpeed = speed + s;
     }
}
