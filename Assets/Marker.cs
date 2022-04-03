using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    private GameObject instructions;
    public bool playerInRange;
    public GameObject UpgradeObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") { instructions.SetActive(true); playerInRange = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") { instructions.SetActive(false); playerInRange = false; }
    }

    private void Start()
    {
        instructions = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GameObject a = (GameObject)Instantiate(UpgradeObj, transform.position + (Vector3.up / 2), Quaternion.identity, null);
            GameObject.Find("Core").GetComponent<GameLoopManager>().PressButton();
            Destroy(this.gameObject);
        }
    }
}
