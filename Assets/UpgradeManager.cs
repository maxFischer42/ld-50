using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public float magIncrease = 0; // Ammo
    public float recoilDecrease = 0f; // Stock
    public int healthIncrease = 0; // Heart
    public float movementIncrease = 0; //Coffee
    public float recoilIncrease = 0f;//|
    public float fallDecrease = 0f; // Feather
    public float jumpIncrease = 0f; // Shoe
    public float fireRateIncrease = 0f; // Fire
    public float rollDecrease = 0f; // Cinnamon Roll
    public int defIncrease = 0; // Shield
    public int atkIncrease = 0; // Bullet


    /*
     * ammo magazine -- ammo size +
rifle stock -- recoil -
heart -- health +
coffee -- speed +, recoil +
feather -- fall speed -
shoe -- jump height +
fire -- fire rate +
cinnamon roll -- roll cooldown -
shield -- def +
bullet -- atk +

     */

    public enum UpgradeType { Ammo, Stock, Heart, Coffee, Feather, Shoe, Fire, Roll, Shield, Bullet}
    public void Upgrade(UpgradeType type)
    {
        switch(type)
        {
            case UpgradeType.Ammo:
                magIncrease += 3;
                break;
            case UpgradeType.Stock:
                recoilDecrease += 0.1f;
                break;
            case UpgradeType.Heart:
                healthIncrease += 10;
                break;
            case UpgradeType.Coffee:
                movementIncrease += 0.05f;
                recoilIncrease += 0.04f;
                break;
            case UpgradeType.Feather:
                fallDecrease += 0.02f;
                break;
            case UpgradeType.Shoe:
                jumpIncrease += 0.05f;
                break;
            case UpgradeType.Fire:
                fireRateIncrease += 0.005f;
                break;
            case UpgradeType.Roll:
                rollDecrease += 0.01f;
                break;
            case UpgradeType.Shield:
                defIncrease += 1;
                break;
            case UpgradeType.Bullet:
                atkIncrease += 1;
                break;
        }
        SendMessage("UpgradeCheck");
    }
}