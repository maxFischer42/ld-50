using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagePopup : MonoBehaviour
{

    public GameObject textMeshPrefab;

    public void DoDamage(Vector2 position, int damage, Color color)
    {
        GameObject popup = Instantiate(textMeshPrefab, position, Quaternion.identity);
        TextPopup p = popup.GetComponent<TextPopup>();
        p.Setup(damage, color);        
    }

    public void DoText(Vector2 position, string text, Color color)
    {
        GameObject popup = Instantiate(textMeshPrefab, position, Quaternion.identity);
        TextPopup p = popup.GetComponent<TextPopup>();
        p.Setup(text, color);
    }

    public GameObject[] UIUpgrades;
    public void UpdateUI(UpgradeManager.UpgradeType t)
    {
        switch(t)
        {
            case UpgradeManager.UpgradeType.Bullet:
                UIUpgrades[0].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Roll:
                UIUpgrades[1].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Coffee:
                UIUpgrades[2].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Feather:
                UIUpgrades[3].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Fire:
                UIUpgrades[4].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Heart:
                UIUpgrades[5].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Ammo:
                UIUpgrades[6].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Shield:
                UIUpgrades[7].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Shoe:
                UIUpgrades[8].SetActive(true);
                break;
            case UpgradeManager.UpgradeType.Stock:
                UIUpgrades[9].SetActive(true);
                break;
        }
    }

}
