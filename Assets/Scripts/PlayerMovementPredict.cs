using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementPredict : MonoBehaviour
{
    // This script is used to attempt to predict the location the player is moving towards based off of their 
    // velocity, inputs, and other aspects. This is meant to be used with AI components for things such as
    // chasing or attacking the player, or other AI objects.

    private GameObject playerObject;

    public void SetPlayer(GameObject p) { playerObject = p; }
    public GameObject GetPlayer() { return playerObject; }

    // how long to keep a position archived
    public float archiveHistory;
    private float historyTimer;

    public int maxArchive;

    public List<Vector2> positionList = new List<Vector2>();
    public float archiveDelay;
    private float delayTimer;

    public float predictionOffsetMultiplier = 2f;

    public float predictDelay;
    private float predictTimer;

    public Vector2 pos;

    public float randomizer = 0.0f;
    private Vector2 randomOffset;

    private void Start()
    {
        SetPlayer(GameObject.Find("Player"));
    }

    void Update()
    {
        HandleDelay();
        HandleHistory();
        if (randomizer != 0.0f) HandleRandomization();
        HandlePrediction();
    }

    void HandleDelay()
    {
        delayTimer += Time.deltaTime;
        if(delayTimer >= archiveDelay && positionList.Count > 0)
        {
            positionList.RemoveAt(0);
            delayTimer = 0f;
        }
    }

    void HandleHistory()
    {
        historyTimer += Time.deltaTime;
        if(historyTimer >= archiveHistory && positionList.Count < maxArchive)
        {
            positionList.Add((Vector2)GetPlayer().transform.position);
            historyTimer = 0f;
        }
    }

    void HandlePrediction()
    {
        predictTimer += Time.deltaTime;
        if(predictTimer >= predictDelay)
        {
            PredictVelocity();
            predictTimer = 0f;
        }
    }

    void PredictVelocity()
    {
        Vector2 prediction = CompilePositions();
        prediction *= predictionOffsetMultiplier;
        prediction += (Vector2)GetPlayer().transform.position;
        prediction += randomOffset;
        pos = prediction;
    }

    Vector2 CompilePositions()
    {
        Vector2 predictedLocation = new Vector2();
        foreach(Vector2 pos in positionList)
        {
            predictedLocation += pos;
            predictedLocation.Normalize();
        }
        return predictedLocation;
    }

    void HandleRandomization()
    {
        float rX = Random.Range(-randomizer, randomizer);
        float rY = Random.Range(-randomizer, randomizer);
        randomOffset = new Vector2(rX, rY);
    }
  
}
