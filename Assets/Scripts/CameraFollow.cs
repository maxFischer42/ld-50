using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Vector3 velocity;
    public float dampTime;
    public Transform target;
    public Vector2 bounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float maxScreenPoint = 0.8f;
        Vector3 mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);
        //Vector3 position = (target.position + GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)) / 2f;
        Vector3 position = (target.position + GetComponent<Camera>().ScreenToWorldPoint(mousePos)) / 2f;
        if(position.x > target.position.x + bounds.x)
        {
            position.x = target.position.x + bounds.x;
        }
        if (position.x < target.position.x + -bounds.x)
        {
            position.x = target.position.x - bounds.x;
        }
        if (position.y < target.position.y + -bounds.y)
        {
            position.y = target.position.y + bounds.y;
        } else if (position.y > target.position.y + bounds.y)
        {
            position.y = target.position.y + bounds.y - 3;
        }
        Vector3 destination = new Vector3(position.x, position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

    }
}
