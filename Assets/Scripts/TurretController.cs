using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementPredict))]
[RequireComponent(typeof(LineRenderer))]
public class TurretController : MonoBehaviour
{

    public bool playerVisible = false;
    Transform player;
    public float sightRange = 8f;
    public LayerMask layermask;
    private PlayerMovementPredict prediction;
    public LineRenderer targetRay;
    public LineRenderer shootRay;
    public float timeToShoot = 1f;
    private float lineVisible = 0.1f;
    private float shootTimer;
    public Transform barrel;
    public float weaponRange = 20f;
    public LayerMask hitLayerMask;
    public float rotateOffset;

    public AudioClip sound;

    public int damage;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        prediction = GetComponent<PlayerMovementPredict>();
    }

    private void Update()
    {
        CheckPlayerVisible();
        DoTimerTick();
    }

    void DoTimerTick()
    {
        if (playerVisible)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= timeToShoot)
            {
                shootTimer = 0f;
                StartCoroutine(shoot());
            }
        }
    }

    IEnumerator shoot()
    {
        shootRay.positionCount = 2;
        shootRay.enabled = true;
        shootRay.SetPosition(0, barrel.position);
        var direction = prediction.pos - (Vector2)barrel.position;
        RaycastHit2D hit = Physics2D.Raycast(barrel.position, direction, weaponRange, hitLayerMask);
        Vector3 pos;
        if (hit.collider)
        {
            //Debug Damage
            if (hit.transform.gameObject.GetComponent<PlayerHealthManager>())
            {
                hit.transform.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damage);
            }
            pos = hit.point;
        }
        else
        {
            pos = (direction.normalized * weaponRange) + (Vector2)barrel.position;
        }
        GameObject.Find("GameManagerAudio").GetComponent<AudioSource>().PlayOneShot(sound);
        shootRay.SetPosition(1, pos);
        yield return new WaitForSeconds(lineVisible);
        shootRay.positionCount = 0;
    }

    void CheckPlayerVisible()
    {
        Vector2 dir = player.position - transform.position;
        dir.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, sightRange, layermask);
        if (hit.collider)
        {
            if(hit.collider.tag == "Player")
            {
                var targetPos = hit.collider.transform.position;
                var thisPos = transform.position;
                targetPos.x = targetPos.x - thisPos.x;
                targetPos.y = targetPos.y - thisPos.y;
                var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotateOffset));

                playerVisible = true;               
                targetRay.positionCount = 2;
                targetRay.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
                targetRay.SetPosition(1, new Vector3(player.position.x, player.position.y, -1));
            } else
            {
                playerVisible = false;
                targetRay.positionCount = 0;
                print(hit.point + " " + hit.collider.name);

            }
        } else
        {
            playerVisible = false;
            targetRay.positionCount = 0;
        }
    }

}
