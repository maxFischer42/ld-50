using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHealth : MonoBehaviour
{
    public GameObject deathEffect;
    
    public void Kill()
    {
        GameObject a = (GameObject)Instantiate(deathEffect, transform);
        a.transform.parent = null;
        randomDrop();
        Destroy(a, 0.6f);
        Destroy(gameObject);
    }

    float dropChance = 0.25f;

    public void randomDrop()
    {
        float r = Random.Range(0.000f, 1.000f);
        if (r <= dropChance)
        {
            GameObject a = (GameObject)Instantiate(item, transform.position, Quaternion.identity, null);
        }
    }

    public GameObject item;
}
