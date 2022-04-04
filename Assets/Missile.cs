using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Missile : MonoBehaviour
{
    public int damage;
    public Transform target;
    IAstarAI ai;

    private float t;

    private void FixedUpdate()
    {
        t += Time.deltaTime;
        if(t > 1f)
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    private void OnEnable()
    {
        target = GameObject.Find("Player").transform;
        ai = GetComponent<IAstarAI>();
        GetComponent<Collider2D>().enabled = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyHealthManager>())
        {
            collision.gameObject.GetComponent<EnemyHealthManager>().DoDamage(damage);
        } else if(collision.gameObject.GetComponent<PlayerHealthManager>())
        {
            collision.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damage);
        }
        GetComponent<EnemyHealthManager>().DoDamage(9999);
    }
}
