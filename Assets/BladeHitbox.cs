using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeHitbox : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            collision.GetComponent<PlayerHealthManager>().HurtPlayer(damage);
            Destroy(this.gameObject);
        }
    }
}
