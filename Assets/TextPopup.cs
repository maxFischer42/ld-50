using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{

    public TextMeshPro text;

    // Update is called once per frame
    public void Setup(int t, Color c)
    {
        text.SetText(t.ToString());
        tColor = c;
        text.color = tColor;
        Destroy(this.gameObject, 3f);
    }

    public void Setup(string t, Color c)
    {
        text.SetText(t);
        tColor = c;
        text.color = tColor;
        Destroy(this.gameObject, 3f);
    }

    private Color tColor;
    public float timer;

    private void Update()
    {
        float s = 1.5f;
        transform.position += new Vector3(0, s) * Time.deltaTime;

        timer -= Time.deltaTime;
        if(timer < 0)
        {
            float dSpeed = 3f;
            tColor.a -= dSpeed * Time.deltaTime;
            text.color = tColor;
        }

    }
}
