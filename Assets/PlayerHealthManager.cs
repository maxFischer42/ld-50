using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public float maxHp = 20;
    public float currentHp;
    public bool canMove = true;
    public float hurtTime = 0.4f;

    public Color defaultColor = Color.white;
    public Color hurtColor = Color.white;
    public Animator anim;

    private SpriteRenderer spr;

    public Image healthBar;

    private float healthbarMax;

    public bool hurt;

    private void Update()
    {
        if(hurt)
        {
            hurt = false;
            HurtPlayer(1);
        }
    }

    private void Start()
    {
        currentHp = maxHp;
        spr = GetComponent<SpriteRenderer>();
        healthbarMax = healthBar.rectTransform.sizeDelta.x;
        anim = GetComponent<Animator>();
    }

    public void HurtPlayer(int damage)
    {
        if (!canMove) return;
        currentHp -= damage;
        float healthbarCurrent = (currentHp / maxHp) * healthbarMax;
        healthBar.rectTransform.sizeDelta = new Vector2(healthbarCurrent, healthBar.rectTransform.sizeDelta.y);
        anim.SetTrigger("Hurt");
        StartCoroutine(hurtCooldown());
        if (currentHp <= 0)
        {
            // end sequence
        }
    }

    public IEnumerator hurtCooldown()
    {
        canMove = false;
        spr.color = hurtColor;
        yield return new WaitForSeconds(hurtTime);
        canMove = true;
        spr.color = defaultColor;
    }
}
