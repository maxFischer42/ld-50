using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public UpgradeManager.UpgradeType myUpgrade;
    public List<Sprite> sprites = new List<Sprite>();
    public Color myColor;
    public RuntimeAnimatorController healAnim;
    public bool isHeal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isHeal)
            {
                GameObject.FindObjectOfType<PlayerHealthManager>().currentHp = GameObject.FindObjectOfType<PlayerHealthManager>().maxHp;
                GameObject.Find("Core").GetComponent<DamagePopup>().DoText(transform.position, "HP+", myColor);
            }
            else {
                collision.gameObject.GetComponent<UpgradeManager>().Upgrade(myUpgrade);
                // display NEW ITEM message or something
                GameObject.Find("Core").GetComponent<DamagePopup>().DoText(transform.position, myUpgrade.ToString() + "+", myColor);
            }
            Destroy(this.gameObject);
        }
    }

    public void Awake()
    {
        int r = Random.Range(0, 13);
        switch(r)
        {
            case 0:
                myUpgrade = UpgradeManager.UpgradeType.Ammo;
                break;
            case 1:
                myUpgrade = UpgradeManager.UpgradeType.Stock;
                break;
            case 2:
                myUpgrade = UpgradeManager.UpgradeType.Heart;
                break;
            case 3:
                myUpgrade = UpgradeManager.UpgradeType.Coffee;
                break;
            case 4:
                myUpgrade = UpgradeManager.UpgradeType.Feather;
                break;
            case 5:
                myUpgrade = UpgradeManager.UpgradeType.Shoe;
                break;
            case 6:
                myUpgrade = UpgradeManager.UpgradeType.Fire;
                break;
            case 7:
                myUpgrade = UpgradeManager.UpgradeType.Roll;
                break;
            case 8:
                myUpgrade = UpgradeManager.UpgradeType.Shield;
                break;
            case 9:
                myUpgrade = UpgradeManager.UpgradeType.Bullet;
                break;
            case 10:
                isHeal = true;
                break;
            case 11:
                isHeal = true;
                break;
            case 12:
                isHeal = true;
                break;
        }
        if (!isHeal) { transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[r]; } else GetComponentInChildren<Animator>().runtimeAnimatorController = healAnim;
    }
}
