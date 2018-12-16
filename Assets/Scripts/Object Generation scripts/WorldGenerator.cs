using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private Poolable startArea;
    [SerializeField] private Poolable leftRoad;
    [SerializeField] private Poolable rightRoad;
    [SerializeField] private Poolable leftTurnRoad;
    [SerializeField] private Poolable rightTurnRoad;
    [SerializeField] private float maxY;
    [Header("References")]
    [SerializeField] private Vector3Reference direction;
    [SerializeField] private FloatReference worldSpeed;
    [Header("Scriptable objects")]
    [SerializeField] private GameStateManager gameState;

    private int leftRoadKey;
    private int rightRoadKey;
    private int leftTurnRoadKey;
    private int rightTurnRoadKey;
    private int startAreaKey;

    private ObjectPooling pooler;
    private TrackData lastTrack;
    private WorldDirection worldDirection;
    private IEnumerator loop;
    private void Start()
    {
        pooler = ObjectPooling.instance;

        startAreaKey = pooler.GetUniqueID();
        pooler.SetPool(startArea, startAreaKey, 2);

        leftRoadKey = pooler.GetUniqueID();
        pooler.SetPool(leftRoad, leftRoadKey, 100);
        
        rightRoadKey = pooler.GetUniqueID();
        pooler.SetPool(rightRoad,rightRoadKey, 100);

        leftTurnRoadKey = pooler.GetUniqueID();
        pooler.SetPool(leftTurnRoad,leftTurnRoadKey, 100);

        rightTurnRoadKey = pooler.GetUniqueID();
        pooler.SetPool(rightTurnRoad,rightTurnRoadKey, 100);
        loop = SpawnLoop();
        StartCoroutine(loop);
    }
    private IEnumerator SpawnLoop()
    {
        worldDirection = WorldDirection.left;

        Poolable startRoad = pooler.GetFromPool(startAreaKey);
        TrackData startTrack = startRoad.GetComponent<TrackData>();
        startTrack.transform.position = transform.position;
        lastTrack = startTrack;
        Debug.Log(startRoad);

        while (gameState.GameActive)
        {
            if (lastTrack.transform.position.y > maxY)
            {
                Poolable newRoad = pooler.GetFromPool(GetCurrentRoadKey());
                TrackData newTrack = newRoad.GetComponent<TrackData>();

                newTrack.transform.position = lastTrack.BackConnection;
                lastTrack = newTrack;
            }

            yield return null;
        }
    }
    public void OnGameReload()
    {
        StopCoroutine(loop);
        loop = SpawnLoop();
        StartCoroutine(loop);
    }
    private int GetCurrentRoadKey()
    {
        switch (worldDirection)
        {
            case WorldDirection.left:
                return leftRoadKey;
                
            case WorldDirection.right:
                return rightRoadKey;
        }
        return 0;
    }
    private void Update()
    {
        if(gameState.GameActive)
            MoveActiveObjects();
    }
    private void MoveActiveObjects()
    {
        for (int i = pooler.activeObjects.Count - 1; i >=0; i--)
        {
            pooler.activeObjects[i].transform.position += direction.value*Time.deltaTime* worldSpeed.value;

            if (pooler.activeObjects[i].transform.position.y > 12)
                pooler.activeObjects[i].EndReached();
        }
    }
}