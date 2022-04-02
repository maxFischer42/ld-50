using System;
using UnityEngine;
using System.Collections;

public class SceneUtility : MonoBehaviour
{

    public static void ToggleGameObject(GameObject _obj)
    {
        _obj.SetActive(_obj.activeInHierarchy);
    }

    public static void setPlayerPref(string _name, string _value)
    {
        PlayerPrefs.SetString(_name, _value);
    }

    public static void setPlayerPref(string _name, int _value)
    {
        PlayerPrefs.SetInt(_name, _value);
    }

    public static void setPlayerPref(string _name, float _value)
    {
        PlayerPrefs.SetFloat(_name, _value);
    }

}
