using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float weaponCooldown;
    float cooldown;
    bool isCooldown;
    public Transform barrel;
    private LineRenderer lr;
    public Animator a;
    public LayerMask hitLayerMask;
    public Transform target;

    public float weaponRange;

    float lineVisible = 0.05f;
    

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        HandleCooldown();
        HandleWeapon();
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
        var direction = target.position - barrel.position;
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
    }

}
