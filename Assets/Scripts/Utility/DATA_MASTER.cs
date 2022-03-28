using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using GameManager;


/*
2022 -- Maxwell Fischer

This script is to be placed on an omnipresent game object between scenes.
It's use is to handle the processing of multiple forms of data between the game, 
gameobjects, and other values throughout the runtime of the game.

To make use of this class in a script, include:
"using GameManager;"
at the top of a script

*/

namespace GameManager {

public class DATA_MASTER : MonoBehaviour {
    
    void Awake() {
        DontDestroyOnLoad(this); // Set this gameobject to not unload when the scene changed
    }

    // PlayerPref Names //
    string audio_sfx_pref = "AUDIO_SFX_PREF",
    audio_bgm_pref = "AUDIO_BGM_PREF";

    //// PLAYERPREF MANAGEMENT ////
    public static void SetPref(string _prefName, int _prefValue) {
        PlayerPrefs.SetInt(_prefName, _prefValue);
    }

    public static void SetPref(string _prefName, float _prefValue) {
        PlayerPrefs.SetFloat(_prefName, _prefValue);
    }
    
    public static void SetPref(string _prefName, string _prefValue) {
        PlayerPrefs.SetString(_prefName, _prefValue);
    }

    public static string GetString(string _prefName) {
        return PlayerPrefs.GetString(_prefName);
    }

    public static int GetInt(string _prefName) {
        return PlayerPrefs.GetInt(_prefName);
    }

    public static float GetFloat(string _prefName) {
        return PlayerPrefs.GetFloat(_prefName);
    }


    //// GAMEOBJECT HANDLING ////
    
    // Spawns the given gameobject in the scene
    public static void SpawnGameObject(GameObject obj) {
        Instantiate(obj);
    } 

    // Spawns the given gameobject at the given vector position
    public static void SpawnGameObjectAtPoint(GameObject obj, Vector3 pos) {
        Instantiate(obj, pos, Quaternion.identity);
    }

    // Spawns the given gameobject at the given vector position and as a child of a given parent
    public static void SpawnGameObjectAtPoint(GameObject obj, Vector3 pos, Transform parent) {
        Instantiate(obj, pos, Quaternion.identity, parent);
    }

    // Returns a spawned gameobject at a given vector position and as a child of a given parent
    public static GameObject CreateGameObjectAtPoint(GameObject obj, Vector3 pos, Transform parent) {
        return (GameObject)Instantiate(obj, pos, Quaternion.identity, parent);
    }

    public static void DisableGameObject(GameObject _obj) {
        _obj.SetActive(false);
    }

    public static void EnableGameObject(GameObject _obj) {
        _obj.SetActive(true);
    }

    public static void ToggleActiveGameObject(GameObject _obj) {
        _obj.SetActive(_obj.activeInHierarchy);
    }


    //// MATH ////
    
    // Returns the given distance between two given points in the scene
    public static float DistanceBetweenPoints(Vector3 posA, Vector3 posB) {
        Vector3 transform = posB - posA;
        return transform.magnitude;
    }


    //// SCENE MANAGEMENT ////
    public static void ChangeScene(string _targetSceneName) {
        SceneManager.LoadScene(_targetSceneName);
    }

    
    //// AUDIO ////
    public enum Audio_Type {SFX, BGM}

    // Sets the volume for the given AudioSource class
    public void SetVolumeOnLoad(AudioSource _audio, Audio_Type _type) {
        switch(_type) {
            case Audio_Type.SFX:
                _audio.volume = GetFloat(audio_sfx_pref);
                break;
            case Audio_Type.BGM:
                _audio.volume = GetFloat(audio_bgm_pref);
                break;
        }
    }

    // Change the specific volume parameter
    public void SetVolume(float _percent, Audio_Type _type) {
        switch(_type) {
            case Audio_Type.SFX:
                SetPref(audio_sfx_pref, _percent);
                break;
            case Audio_Type.BGM:
                SetPref(audio_bgm_pref, _percent);
                break;
        }
    }

    // Get the specific volume parameter
    public float GetVolume(Audio_Type _type) {
        float _volume = 0f;
        switch(_type) {
            case Audio_Type.SFX:
                _volume = GetFloat(audio_sfx_pref);
                break;
            case Audio_Type.BGM:
                _volume = GetFloat(audio_bgm_pref);
                break;
        }
        return _volume;
    }

}

/// Handles transitions between scenes, uses 
public class SceneTransition : MonoBehaviour {

    // This class is taken from the Unity tutorial found HERE:
    // https://docs.unity3d.com/2020.1/Documentation/Manual/HOWTO-UIScreenTransition.html

    //Screen to open automatically at the start of the Scene
    public Animator initiallyOpen;

    //Currently Open Screen
    private Animator m_Open;

    //Hash of the parameter we use to control the transitions.
    private int m_OpenParameterId;

    //The GameObject Selected before we opened the current Screen.
    //Used when closing a Screen, so we can go back to the button that opened it.
    private GameObject m_PreviouslySelected;

    //Animator State and Transition names we need to check against.
    const string k_OpenTransitionName = "Open";
    const string k_ClosedStateName = "Closed";

    public void OnEnable()
    {
        //We cache the Hash to the "Open" Parameter, so we can feed to Animator.SetBool.
        m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);

        //If set, open the initial Screen now.
        if (initiallyOpen == null)
            return;
        OpenPanel(initiallyOpen);
    }

    //Closes the currently open panel and opens the provided one.
    //It also takes care of handling the navigation, setting the new Selected element.
    public void OpenPanel (Animator anim)
    {
        if (m_Open == anim)
            return;

        //Activate the new Screen hierarchy so we can animate it.
        anim.gameObject.SetActive(true);
        //Save the currently selected button that was used to open this Screen. (CloseCurrent will modify it)
        var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;
        //Move the Screen to front.
        anim.transform.SetAsLastSibling();

        CloseCurrent();

        m_PreviouslySelected = newPreviouslySelected;

        //Set the new Screen as then open one.
        m_Open = anim;
        //Start the open animation
        m_Open.SetBool(m_OpenParameterId, true);

        //Set an element in the new screen as the new Selected one.
        GameObject go = FindFirstEnabledSelectable(anim.gameObject);
        SetSelected(go);
    }

    //Finds the first Selectable element in the providade hierarchy.
    static GameObject FindFirstEnabledSelectable (GameObject gameObject)
    {
        GameObject go = null;
        var selectables = gameObject.GetComponentsInChildren<Selectable> (true);
        foreach (var selectable in selectables) {
            if (selectable.IsActive () && selectable.IsInteractable ()) {
                go = selectable.gameObject;
                break;
            }
        }
        return go;
    }

    //Closes the currently open Screen
    //It also takes care of navigation.
    //Reverting selection to the Selectable used before opening the current screen.
    public void CloseCurrent()
    {
        if (m_Open == null)
            return;

        //Start the close animation.
        m_Open.SetBool(m_OpenParameterId, false);

        //Reverting selection to the Selectable used before opening the current screen.
        SetSelected(m_PreviouslySelected);
        //Start Coroutine to disable the hierarchy when closing animation finishes.
        StartCoroutine(DisablePanelDeleyed(m_Open));
        //No screen open.
        m_Open = null;
    }

    //Coroutine that will detect when the Closing animation is finished and it will deactivate the
    //hierarchy.
    IEnumerator DisablePanelDeleyed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true;
        while (!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

            wantToClose = !anim.GetBool(m_OpenParameterId);

            yield return new WaitForEndOfFrame();
        }

        if (wantToClose)
            anim.gameObject.SetActive(false);
    }

    //Make the provided GameObject selected
    //When using the mouse/touch we actually want to set it as the previously selected and 
    //set nothing as selected for now.
    private void SetSelected(GameObject go)
    {
        //Select the GameObject.
        EventSystem.current.SetSelectedGameObject(go);

        //If we are using the keyboard right now, that's all we need to do.
        var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
        if (standaloneInputModule != null)
            return;

        //Since we are using a pointer device, we don't want anything selected. 
        //But if the user switches to the keyboard, we want to start the navigation from the provided game object.
        //So here we set the current Selected to null, so the provided gameObject becomes the Last Selected in the EventSystem.
        EventSystem.current.SetSelectedGameObject(null);
    }
}


}