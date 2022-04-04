using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public RectTransform UI_arrow;
    public RectTransform node;

    public RectTransform[] difficulties;

    public float timePerAdd;

    public Transform player;

    public GameObject warningOverlay;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        UpdateArrow();
        globalTime += Time.deltaTime;
        if(globalTime >= timeToIncrease)
        {
            GetComponent<EnemyManager>().INCREASE_DIFFICULTY();
            int n = GetComponent<EnemyManager>().currentDifficulty - 1;
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
