using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public Color currentColor;
    public Color transparent;
    public GameObject deathEffect;

    private SpriteRenderer sr;

    public int maxHealth;
    private int currentHealth = 0;
    private float colorPerDamage;
    private float currentSat = 0;
    public bool canHurt = true;

    private void Start()
    {        
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        currentColor = sr.color;
        CalcColorPerDamage();
    }

    private void CalcColorPerDamage()
    {
        float tmp = 1 / (float)maxHealth;
        colorPerDamage = tmp;
    }

    public void DoDamage(int damage)
    {
        if (!canHurt) return;
        currentHealth += damage;
        if (currentHealth >= maxHealth) Kill();
        StartCoroutine(DamageColor());
    }

    IEnumerator DamageColor()
    {
        canHurt = false;
        sr.color = transparent;
        Color newColor = Color.HSVToRGB(1, colorPerDamage + currentSat, 1);
        yield return new WaitForSeconds(0.1f);
        sr.color = newColor;
        currentColor = newColor;
        canHurt = true;
        
    }

    public void Kill()
    {
        GameObject a = (GameObject)Instantiate(deathEffect, transform);
        a.transform.parent = null;
        Destroy(a, 0.6f);
        Destroy(gameObject);
    }
}
