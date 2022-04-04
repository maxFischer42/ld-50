using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHandler : MonoBehaviour
{

    public int areaId;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            GameObject.Find("Core").GetComponent<GameLoopManager>().changeArea(areaId);
        }
    }

}
