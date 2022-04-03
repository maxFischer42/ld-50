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

}
