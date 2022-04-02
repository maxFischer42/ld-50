using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float weaponCooldown;
    public float cooldown_t;
    float cooldown;
    bool isCooldown;
    public Transform barrel;
    private LineRenderer lr;
    public Animator a;
    public LayerMask hitLayerMask;
    public Transform target;

    public UpgradeManager upgrade;
    public int ammo;
    public int currentammo;
    public float reloadTime = 0.8f;
    public float reloadOffset;

    public float accuracy = 1f;

    public bool isReload = false;

    IEnumerator reload(float t)
    {
        isReload = true;
        yield return new WaitForSeconds(t);
        currentammo = ammo;
        isReload = false;
    }

    public float weaponRange;

    float lineVisible = 0.05f;
    

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        UpgradeCheck();
    }

    private void Update()
    {
        if (isReload) return;
        HandleCooldown();
        HandleWeapon();
        if(currentammo <= 0 && weaponCooldown > 0)
        {
            StartCoroutine(reload(reloadTime - reloadOffset));
        }
    }

    void HandleCooldown()
    {
        if (!isCooldown) return;
        if (cooldown <= 0)
        {
            isCooldown = false;
            return;
        }
        cooldown -= Time.deltaTime;
    }

    void HandleWeapon()
    {
        if (isCooldown) return;
        if (Input.GetButton("Fire1"))
        {
            a.SetTrigger("MuzzleTrigger");
            StartCoroutine(HandleLine());
        }
    }

    IEnumerator HandleLine()
    {
        lr.positionCount = 2;
        lr.enabled = true;
        lr.SetPosition(0, new Vector3(barrel.position.x, barrel.position.y, -1));
        float _x = Random.Range(-accuracy/10, accuracy / 10);
        float _y = Random.Range(-accuracy / 10, accuracy / 10);
        Vector3 t = new Vector2(target.position.x + _x, target.position.y + _y);
        var direction = t - barrel.position;
        RaycastHit2D hit = Physics2D.Raycast(barrel.position, direction, weaponRange, hitLayerMask);
        Vector3 pos;
        if(hit.collider)
        {
            print("hit!");
            pos = hit.point;
            //Debug Damage
            if (hit.transform.gameObject.GetComponent<DebugHealth>()) hit.transform.gameObject.GetComponent<DebugHealth>().Kill();
            if (hit.transform.gameObject.GetComponent<EnemyHealthManager>()) hit.transform.gameObject.GetComponent<EnemyHealthManager>().DoDamage(1);
        } else
        {
            pos = (direction.normalized * weaponRange) + barrel.position;
        }
        lr.SetPosition(1, new Vector3(pos.x, pos.y, -1));
        isCooldown = true;
        cooldown = weaponCooldown;
        yield return new WaitForSeconds(lineVisible);
        lr.positionCount = 0;
        currentammo--;
    }

    public void UpgradeCheck()
    {
        float a = upgrade.magIncrease;
        if (a >= 0.78) a = 0.78f;
        reloadOffset = a;

        float f = upgrade.fireRateIncrease;
        weaponCooldown = cooldown_t - f;
        if (weaponCooldown < 0) weaponCooldown = 0;

    }

}
