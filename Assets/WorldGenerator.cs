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

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        worldDirection = WorldDirection.left;

        Poolable startRoad = pooler.GetFromPool(startAreaKey);
        TrackData startTrack = startRoad.GetComponent<TrackData>();
        startTrack.transform.position = transform.position;
        lastTrack = startTrack;
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

    private void MoveActiveObjects()
    {
        foreach(Poolable ob in pooler.activeObjects)
        {
            ob.transform.position += direction.value;

            if (ob.transform.position.y > 12)
                ob.EndReached();
        }
    }
}