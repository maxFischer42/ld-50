using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class gameover : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource s;
    void Awake()
    {
        GameObject.Find("TimeSet").GetComponent<TextMeshProUGUI>().SetText(Mathf.RoundToInt(PlayerPrefs.GetFloat("time")).ToString() + " Seconds");
        s.volume = PlayerPrefs.GetFloat("sfx");
        s.Play();
    }

}
