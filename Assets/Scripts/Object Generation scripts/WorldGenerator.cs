using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Default roads")]
    [SerializeField] private Poolable startArea;
    [SerializeField] private Poolable leftRoad;
    [SerializeField] private Poolable rightRoad;
    [SerializeField] private Poolable leftTurnRoad;
    [SerializeField] private Poolable rightTurnRoad;
    [Header("Specials")]
    [SerializeField] private float specialsCooldownMin = 0f;
    [SerializeField] private float speciaCooldownMax = 0f;
    [SerializeField] private Poolable leftWoodBridge;
    [SerializeField] private Poolable rightWoodBridge;
    [SerializeField] private Poolable leftLogBridge;
    [SerializeField] private Poolable rightLogBridge;

    [SerializeField] private float maxY;
    [Range(0,1)]
    [SerializeField] private float directionSwitchChance;
    [Header("References")]
    [SerializeField] private Vector3Reference direction;
    [SerializeField] private FloatReference worldSpeed;
    [Header("Scriptable objects")]
    [SerializeField] private BoolReference gameActive;

    private int startAreaKey;

    private int leftRoadKey;
    private int rightRoadKey;

    private int leftTurnRoadKey;
    private int rightTurnRoadKey;

    private int leftWoodBridgeKey;
    private int rightWoodBridgeKey;

    private int leftLogBridgeKey;
    private int rightLogBridgeKey;

    private int[] leftSpecialKeys;
    private int[] rightSpecialKeys;

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

        leftWoodBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(leftWoodBridge, leftWoodBridgeKey, 3);

        rightWoodBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(rightWoodBridge, rightWoodBridgeKey, 3);

        leftLogBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(leftLogBridge, leftLogBridgeKey, 3);

        rightLogBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(rightLogBridge, rightLogBridgeKey, 3);

        leftSpecialKeys = new int[] { leftWoodBridgeKey, leftLogBridgeKey };
        rightSpecialKeys = new int[] { rightWoodBridgeKey, rightLogBridgeKey };

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

        yield return new WaitUntil(() => gameActive.value);

        float timeElapsed=0f;
        float timeToWait = Random.Range(specialsCooldownMin,speciaCooldownMax);

        while (gameActive.value)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed > timeToWait)
            {
                timeToWait = Random.Range(specialsCooldownMin, speciaCooldownMax);
                timeElapsed = 0f;

                int leftKey = Random.Range(0f, 1f) > 0.5f ? leftWoodBridgeKey : leftLogBridgeKey;
                int rightKey = Random.Range(0f, 1f) > 0.5f ? rightWoodBridgeKey : rightLogBridgeKey;

                GenerateTrack(GetKey(leftKey,rightKey)); //generate special

                IncrementTracks();
            }

            if (lastTrack.transform.position.y > maxY)
            {
                GenerateTrack(GetKey(leftRoadKey,rightRoadKey)); //generate normal
                IncrementTracks();

                if (RollDirectionChange())
                {
                    worldDirection = SwitchDirection(worldDirection);
                    GenerateTrack(GetKey(leftTurnRoadKey, rightTurnRoadKey)); //generate turn
                    IncrementTracks();
                }
            }

            yield return null;
        }
    }

    private void IncrementTracks()
    {
        for (int i = pooler.activeObjects.Count - 1; i >= 0; i--)
        {
            pooler.activeObjects[i].transform.position += new Vector3(0, 0, 0.5f);
        }
    }

    private void GenerateTrack(int key)
    {
        Poolable track = pooler.GetFromPool(key);
        TrackData newTrack = track.GetComponent<TrackData>();

        newTrack.transform.position = lastTrack.BackConnection;
        lastTrack = newTrack;
    }

    public void OnGameReload()
    {
        StopCoroutine(loop);
        loop = SpawnLoop();
        StartCoroutine(loop);
    }

    private int GetKey(int left, int right)
    {
        switch (worldDirection)
        {
            case WorldDirection.left:
                return left;
            case WorldDirection.right:
                return right;
        }

        return 0;
    }
    
    private WorldDirection SwitchDirection(WorldDirection current)
    {
        switch (current)
        {
            case WorldDirection.left:
                return WorldDirection.right;
            case WorldDirection.right:
                return WorldDirection.left;
        }
        return 0;
    }

    private bool RollDirectionChange()
    {
        return Random.Range(0f, 1f) <= directionSwitchChance;
    }

    private void Update()
    {
        if(gameActive.value)
            MoveActiveObjects();
    }

    private void MoveActiveObjects()
    {
        for (int i = pooler.activeObjects.Count - 1; i >=0; i--)
        {
            pooler.activeObjects[i].transform.position += direction.value*Time.deltaTime* worldSpeed.value;

            if (pooler.activeObjects[i].transform.position.y > 18)
                pooler.activeObjects[i].EndReached();
        }
    }
}