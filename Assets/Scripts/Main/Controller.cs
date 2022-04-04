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
    public LayerMask collideLayers;
    public LayerMask rollLayers;

    public enum state { idle, move, jump, hurt, roll};
    public state myState;

    public GameObject jumpEffect;
    public GameObject landEffect;
    public Transform feet;

    public GameObject Arms;

    private float slopeFriction = 1f;

    bool isSlide = false;

    bool canRoll = true;

    public UpgradeManager upgrade;

    public float rollSpeed;
    public float rollTimer = 2f;
    bool isRoll;
    float tempRoll;
    private PlayerHealthManager health;

    public float launchforce = 10f;

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
        if (isRoll) {
            tempRoll -= Time.deltaTime;
            if(tempRoll < 0)
            {
                isRoll = false;
                if (Input.GetButton("Jump"))
                {
                    isGrounded = false;
                    myState = state.jump;
                    rolljump = true;
                    rb.velocity = new Vector2(dir * (launchforce + speedIncrease), jumpForce + jumpOffset);
                    s.PlayOneShot(jumpS);
                }
                else
                {
                    myState = state.idle;
                }
                Arms.SetActive(true);
                canRoll = false;
                gameObject.layer = 8;
                StartCoroutine(startRollCooldown());
            }
        } else { 
            if (!health.canMove) return;
            if (rb.velocity.x == 0 && isGrounded) { myState = state.idle; }
            isGrounded = SetGrounded();
            doJump();
        }
        HandleAnimations();
    }

    void FixedUpdate()
    {
        if (!health.canMove) return;
        if (myState == state.idle) rb.velocity = Vector2.zero;
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

    bool rolljump = false;

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
                a.SetBool("Hurt", false);
                break;
            case state.move:
                a.SetBool("Idle", false);
                a.SetBool("Roll", false);
                a.SetBool("Grounded", true);
                a.SetFloat("Xmove", 1);
                a.SetBool("Hurt", false);
                break;
            case state.jump:
                a.SetBool("Idle", false);
                a.SetBool("Roll", false);
                a.SetBool("Grounded", false);
                a.SetFloat("Xmove", -1);
                a.SetBool("Hurt", false);
                break;
            case state.roll:
                a.SetBool("Idle", false);
                a.SetBool("Roll", true);
                a.SetBool("Grounded", true);
                a.SetFloat("Xmove", -1);
                a.SetBool("Hurt", false);
                break;
        }
    }

    void Move()
    {
        if (rolljump) return;
        if(myState == state.roll) return;
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift))
        {
           MoveRoll(); return;
        }
        float x = getX();
        dir = (Input.GetAxisRaw("Horizontal") != 0 ? (int)Input.GetAxisRaw("Horizontal") : dir);
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
            s.PlayOneShot(jumpS);

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
        var hit = Physics2D.Raycast(feet.position, Vector3.down, disToGround, groundLayermask);
        bool grounded = hit.collider;
        if(grounded && checkLand)
        {
            GameObject j = (GameObject)Instantiate(landEffect, feet);
            j.transform.parent = null;
            rolljump = false;
            Destroy(j, 0.5f);
        }        
        return grounded;
    }

    IEnumerator startRollCooldown()
    {        
        yield return new WaitForSeconds(rollCooldown - rollOffset);
        canRoll = true;
        GameObject.Find("Core").GetComponent<DamagePopup>().DoText(transform.position, "ROLL RESTORED", Color.white);
    }

    void MoveHorizontally(float i)
    {
        float newX = movementSpeed;
        if(isGrounded) { myState = state.move; }
        if(!rolljump) rb.velocity = new Vector2(i * newX, rb.velocity.y);
    }

    void MoveRoll()
    {
        if (!isGrounded) return;
        if (!canRoll) return;
        s.PlayOneShot(rollS);
        float newX = dir * rollSpeed;
        rb.velocity = new Vector2(newX, rb.velocity.y);
        myState = state.roll;
        tempRoll = rollTimer;
        isRoll = true;
        gameObject.layer = 11;
        Arms.SetActive(false);
    }

    public float getX() { return Input.GetAxis("Horizontal"); }
    public float getYX() { return Input.GetAxis("Vertical"); }
    public bool getAttack() { return Input.GetButton("Fire1"); }

    public float fallOffset = 0f;
    public float jumpOffset = 0f;
    public float speedIncrease = 0f;
    public float rollOffset = 0f;
    public float rollCooldown = 1.5f;

    public float gravity = 2;
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
        if (r >= 3.5f) r = 3.5f;

        rb.gravityScale = gravity - f;
        jumpForce = jump + j;
        rollOffset = r;
        movementSpeed = speed + s;

        int h = upgrade.healthIncrease;
        GameObject.FindObjectOfType<PlayerHealthManager>().maxHp = 20 + h;
        int d = upgrade.defIncrease;
        GameObject.FindObjectOfType<PlayerHealthManager>().def = d;

        GameObject.Find("GunManager").SendMessage("UpgradeCheck");

    }

    public AudioClip jumpS;
    public AudioClip rollS;

    public AudioSource s;
}
