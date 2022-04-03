using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{

    public GameObject hitbox;
    public int damage;
    public Transform offsetToSpawn;
    public bool n;
    
    void doNothing() { } //lol

    public void SpawnHitbox()
    {
        if (n) return;
        // spawn the blade hitbox
        GameObject g = (GameObject)Instantiate(hitbox, offsetToSpawn);
        g.GetComponent<BladeHitbox>().damage = damage;
        Destroy(g, 0.1f);
    }

}
