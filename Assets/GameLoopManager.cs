using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    public List<Transform> marker_positions = new List<Transform>();
    private Transform previousPosition;
    public Transform currentPosition;
    public Image timer;

    public GameObject marker_object;

    public float countdown = 120;
    public float timer_ceiling = 100;

    public float globalTime = 0f;
    public float timeToIncrease = 200f;
    public float timeToIncreasea = 200f;
    public RectTransform UI_arrow;
    public RectTransform node;

    public RectTransform[] difficulties;

    public float timePerAdd;

    public Transform player;

    public float seconds;

    public GameObject warningOverlay;

    public AudioSource audioSource;
    public AudioClip tick;
    public GameObject effect;
    bool uhoh = false;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        UpdateArrow();
        seconds += Time.deltaTime;
        globalTime += Time.deltaTime;

        if (countdown <= 0) { End(); return; }

        if(globalTime >= timeToIncreasea)
        {
            GetComponent<EnemyManager>().INCREASE_DIFFICULTY();
            int n = GetComponent<EnemyManager>().currentDifficulty - 1;
            timeToIncreasea = timeToIncrease - (5 * n);
            node.position = difficulties[n].position;
            globalTime = 0f;
        }
        countdown -= Time.deltaTime;

        if(countdown <= timer_ceiling)
        {
            timer.fillAmount = countdown / timer_ceiling;
        } else
        {
            countdown = timer_ceiling;
        }

        if(countdown < 30f && !uhoh) // UH OH TIME
        {
            audioSource.PlayOneShot(tick);
            warningOverlay.SetActive(true);
            effect.SetActive(true);
            uhoh = true;
        } else if (countdown > 30f)
        {
            warningOverlay.SetActive(false);
            effect.SetActive(false);
            uhoh = false;
        }
    }

    public void End()
    {
        PlayerPrefs.SetFloat("time", seconds);
        SceneManager.LoadScene("GameOver");
    }

    void ChangePosition()
    {
        var p = marker_positions.Count;
        previousPosition = currentPosition;
        while (currentPosition == previousPosition)
        {
            int a = Random.Range(0, p);
            currentPosition = marker_positions[a];
        }
        Instantiate(marker_object, currentPosition.position, Quaternion.identity);        
    }

    public void PressButton()
    {
        countdown += timePerAdd;
        ChangePosition();
    }

    public Vector3 v = Vector3.forward;

    void UpdateArrow()
    {
        Vector3 direction = (currentPosition.position - player.position).normalized;
        var angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(direction, v));

        Vector3 cross = Vector3.Cross(direction, v);
        angle = -Mathf.Sign(cross.z) * angle;

        UI_arrow.localEulerAngles = new Vector3(0f, 0f, angle);
        
    }

    public Transform[] spawnAreas;
    public Transform currentSpawnAreas;

    public void changeArea(int area)
    {
        currentSpawnAreas = spawnAreas[area - 1];
        GetComponent<EnemyManager>().checkEnemyDespawn();
    }
}
