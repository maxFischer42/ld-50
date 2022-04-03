using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missile;
    private GameObject myMissile;
    public Transform spawnSite;

    public void Update()
    {
        if(myMissile == null)
        {
            myMissile = (GameObject)Instantiate(missile, spawnSite);
        }
    }

}
