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
        Destroy(a, 0.6f);
        Destroy(gameObject);
    }
}
