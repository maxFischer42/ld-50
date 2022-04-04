using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{

    public GameObject titleCanvas;
    public GameObject howToPlayCanvas;
    public GameObject optionsCanvas;

    public Slider sfx;
    public Slider bgm;

    public void Start()
    {
        if(PlayerPrefs.GetInt("hasPlayed") != 1)
        {
            PlayerPrefs.SetFloat("sfx", 0.8f);
            PlayerPrefs.SetFloat("bgm", 0.09f);

            sfx.value = 0.8f;
            bgm.value = 0.09f;

        } else
        {
            sfx.value = PlayerPrefs.GetFloat("sfx");
            bgm.value = PlayerPrefs.GetFloat("bgm");
        }
        PlayerPrefs.SetInt("hasPlayed", 1);
    }

    public void ChangeSfx()
    {
        PlayerPrefs.SetFloat("sfx", sfx.value);
    }

    public void ChangeBgm()
    {
        PlayerPrefs.SetFloat("bgm", bgm.value);
    }

    public void openTitle()
    {
        titleCanvas.SetActive(true);
        howToPlayCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
    }

    public void openHow()
    {
        titleCanvas.SetActive(false);
        howToPlayCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }

    public void openOptions()
    {
        titleCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("TestScene");
    }

}
