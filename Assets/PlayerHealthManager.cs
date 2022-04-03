using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public float maxHp = 20;
    public float currentHp;
    public bool canMove = true;
    public float reactTime = 0.4f;

    public Color defaultColor = Color.white;
    public Color hurtColor = Color.white;
    public Animator anim;

    private SpriteRenderer spr;

    public Image healthBar;

    private float healthbarMax;

    public bool hurt;

    public int def = 0;

    public float hurtLength = 0.3f;
    float hurtTimer = 0;

    private void Update()
    {
        UpdateHp();
        if(hurt)
        {
            hurt = false;
            HurtPlayer(1);
        }
        if(!canMove)
        {
            hurtTimer -= Time.deltaTime;
            if(hurtTimer < 0)
            {
                canMove = true;
            }
        }
    }

    private void Start()
    {
        currentHp = maxHp;
        spr = GetComponent<SpriteRenderer>();
        healthbarMax = healthBar.rectTransform.sizeDelta.x;
        anim = GetComponent<Animator>();
    }

    public void UpdateHp()
    {
        float healthbarCurrent = (currentHp / maxHp) * healthbarMax;
        if (currentHp > maxHp) currentHp = maxHp;
        healthBar.rectTransform.sizeDelta = new Vector2(healthbarCurrent, healthBar.rectTransform.sizeDelta.y);
    }

    public void HurtPlayer(int damage)
    {
        if (!canMove) return;
        if (!canHurt) return;
        if (damage > 1 && def > 0) {
            if (def > 15)
            {
                damage--;
            } else
            {
                int a = Random.Range(0, 30);
                if(a < def)
                {
                    damage--;
                }
            }
        }
        currentHp -= damage;

        UpdateHp();
        anim.SetBool("Hurt", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Roll", false);
        anim.SetBool("Grounded", false);
        anim.SetFloat("Xmove", -1);
        StartCoroutine(hurtCooldown());
        canMove = false;
        hurtTimer = hurtLength;
        canHurt = false;
        if (currentHp <= 0)
        {
            // end sequence
            Time.timeScale = 0f;
        }
    }

    bool canHurt = true;

    public IEnumerator hurtCooldown()
    {
        spr.color = hurtColor;
        yield return new WaitForSeconds(reactTime);
        canHurt = true;
        spr.color = defaultColor;
    }
}
