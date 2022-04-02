using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpider : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        SetPlayer(GameObject.FindGameObjectWithTag("Player").transform);
        SetRigidBody(GetComponent<Rigidbody2D>());
        rb = GetRigidBody();
        dir = startDirection;
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        tilemap = GameObject.Find("Ground Tilemap").GetComponent<Tilemap>();

    }
    private Grid grid;
    private Tilemap tilemap;

    public enum direction { left, right, up, down };
    public direction startDirection;
    public direction gravityDirection = direction.down;
    public direction dir;

    public float moveSpeed;
    private Vector2 currentDirection;
    private Rigidbody2D rb;

    public Vector3Int gridPosition;
    public bool nextPositionFront = false;
    public bool nextPositionBelow = false;
    public Vector2 localGravity = Vector2.down;
    public float warpOffset = 0.2f;

    public SpriteRenderer sprite;
    public Transform spriteTransform;

    private void Update()
    {
        Vector3Int lPos = grid.WorldToCell(transform.position);
        gridPosition = lPos;
        HandleNextPositions();
        Move();
    }

    void Move()
    {
        rb.velocity = currentDirection;
        UpdateDirection();
    }

    void HandleNextPositions()
    {
        var positions = getNextPosition();
        bool nextTile = tilemap.GetTile(positions[0]);
        bool nextSubTile = tilemap.GetTile(positions[1]);

        if (nextTile && nextSubTile) // inverse corner
        {
            if (dir == direction.right && gravityDirection == direction.down)
            {
                dir = direction.up;
                gravityDirection = direction.right;
            }
            else if (dir == direction.right && gravityDirection == direction.up)
            {
                dir = direction.down;
                gravityDirection = direction.right;
            }
            else if (dir == direction.left && gravityDirection == direction.down)
            {
                dir = direction.up;
                gravityDirection = direction.left;
            }
            else if (dir == direction.left && gravityDirection == direction.up)
            {
                dir = direction.down;
                gravityDirection = direction.right;
            }
            else if (dir == direction.up && gravityDirection == direction.right)
            {
                dir = direction.left;
                gravityDirection = direction.up;
            }
            else if (dir == direction.up && gravityDirection == direction.left)
            {
                dir = direction.right;
                gravityDirection = direction.up;
            }
            else if (dir == direction.down && gravityDirection == direction.right)
            {
                dir = direction.left;
                gravityDirection = direction.down;
            }
            else if (dir == direction.down && gravityDirection == direction.left)
            {
                dir = direction.right;
                gravityDirection = direction.down;
            }
        }
        else if (!nextSubTile) // corner
        {

            transform.position = positions[1] + new Vector3(0.5f, 0.5f, 0);
            if (dir == direction.up && gravityDirection == direction.right)
            {
                dir = direction.right;
                gravityDirection = direction.down;
            }
            else if (dir == direction.up && gravityDirection == direction.left)
            {
                dir = direction.left;
                gravityDirection = direction.down;
            }
            else if (dir == direction.down && gravityDirection == direction.right)
            {
                dir = direction.right;
                gravityDirection = direction.up;
            }
            else if (dir == direction.down && gravityDirection == direction.left)
            {
                dir = direction.left;
                gravityDirection = direction.up;
            }
            else if (dir == direction.left && gravityDirection == direction.down)
            {
                dir = direction.down;
                gravityDirection = direction.right;
            }
            else if (dir == direction.right && gravityDirection == direction.down)
            {
                dir = direction.down;
                gravityDirection = direction.left;
            }
            else if (dir == direction.left && gravityDirection == direction.up)
            {
                dir = direction.up;
                gravityDirection = direction.right;
            }
            else if (dir == direction.right && gravityDirection == direction.up)
            {
                dir = direction.up;
                gravityDirection = direction.left;
            }
        }
        switch(dir)
        {
            case direction.up:
                currentDirection = Vector2.up;
                break;
            case direction.down:
                currentDirection = Vector2.down;
                break;
            case direction.left:
                currentDirection = Vector2.left;
                break;
            case direction.right:
                currentDirection = Vector2.right;
                break;
        }
        currentDirection *= moveSpeed;
    }
    Vector3Int[] getNextPosition()
    {
        Vector3Int nextPos = gridPosition;
        Vector3Int nextPosSub = gridPosition;
        var cDir = getDirectionVector();
        var gravity = getGravityVector();
        nextPos += cDir;
        nextPosSub += gravity;
        Vector3Int[] a = new Vector3Int[2];
        a[0] = nextPos;
        a[1] = nextPosSub;
        return a;
    }

    Vector3Int getDirectionVector()
    {
        Vector3Int cDir = new Vector3Int();
        switch (dir)
        {
            case direction.down:
                cDir = Vector3Int.down;
                break;
            case direction.up:
                cDir = Vector3Int.up;
                break;
            case direction.left:
                cDir = Vector3Int.left;
                break;
            case direction.right:
                cDir = Vector3Int.right;
                break;
        }
        return cDir;
    }

    Vector3Int getGravityVector()
    {
        Vector3Int gravity = new Vector3Int();
        switch (gravityDirection)
        {
            case direction.down:
                gravity = Vector3Int.down;
                break;
            case direction.up:
                gravity = Vector3Int.up;
                break;
            case direction.left:
                gravity = Vector3Int.left;
                break;
            case direction.right:
                gravity = Vector3Int.right;
                break;
        }
        return gravity;
    }

    void UpdateDirection()
    {
        if(dir == direction.down && gravityDirection == direction.left)
        {
            sprite.flipX = true;
            spriteTransform.rotation = Quaternion.Euler(0, 0, -90);
        } else if (dir == direction.down && gravityDirection == direction.right)
        {
            sprite.flipX = false;
            spriteTransform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (dir == direction.up && gravityDirection == direction.left)
        {
            sprite.flipX = false;
            spriteTransform.rotation = Quaternion.Euler(0, 0, -90);
        } else if (dir == direction.up && gravityDirection == direction.right)
        {
            sprite.flipX = true;
            spriteTransform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (dir == direction.left && gravityDirection == direction.up)
        {
            sprite.flipX = true;
            spriteTransform.rotation = Quaternion.Euler(0, 0, 180);
        } else if (dir == direction.right && gravityDirection == direction.up)
        {
            sprite.flipX = false;
            spriteTransform.rotation = Quaternion.Euler(0, 0, 180);
        } else if (dir == direction.left && gravityDirection == direction.down)
        {
            sprite.flipX = false;
            spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (dir == direction.right && gravityDirection == direction.down)
        {
            sprite.flipX = true;
            spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        }



    }

    

}
