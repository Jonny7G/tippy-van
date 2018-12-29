using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private float maxY;
    //[Range(0, 1)]
    [SerializeField] private float directionSwitchChance;

    [SerializeField] private Vector3Reference direction;
    [SerializeField] private FloatReference worldSpeed;
    [SerializeField] private BoolReference gameActive;
    [SerializeField] private GameEvent OnWorldMove;

    [SerializeField] private Poolable startArea;
    [SerializeField] private Poolable leftRoad;
    [SerializeField] private Poolable rightRoad;
    [SerializeField] private Poolable leftTurnRoad;
    [SerializeField] private Poolable rightTurnRoad;

    [SerializeField] private float specialsCooldownMin = 0f;
    [SerializeField] private float speciaCooldownMax = 0f;

    [SerializeField] private Poolable leftWoodBridge;
    [SerializeField] private Poolable rightWoodBridge;

    [SerializeField] private Poolable leftLogBridge;
    [SerializeField] private Poolable rightLogBridge;

    [SerializeField] private Poolable leftBrokenBridge;
    [SerializeField] private Poolable rightBrokenBridge;


    private int startAreaKey;
    
    private int leftRoadKey;
    private int rightRoadKey;

    private int leftTurnRoadKey;
    private int rightTurnRoadKey;

    public int leftWoodBridgeKey { get; private set; }
    public int rightWoodBridgeKey { get; private set; }

    public int leftLogBridgeKey { get; private set; }
    public int rightLogBridgeKey { get; private set; }

    public int leftBrokenBridgeKey { get; private set; }
    public int rightBrokenBridgeKey { get; private set; }

    private int[] leftSpecialKeys;
    private int[] rightSpecialKeys;

    private ObjectPooling pooler;
    private TrackData lastTrack;
    private WorldDirection worldDirection;
    private IEnumerator loop;
    public bool spawnNormally=false;
    private System.Random rnd = new System.Random();

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

        leftBrokenBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(leftBrokenBridge, leftBrokenBridgeKey, 3);

        rightBrokenBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(rightBrokenBridge, rightBrokenBridgeKey, 3);

        leftSpecialKeys = new int[] { leftWoodBridgeKey, leftLogBridgeKey, leftBrokenBridgeKey };
        rightSpecialKeys = new int[] { rightWoodBridgeKey, rightLogBridgeKey, rightBrokenBridgeKey };

        GameReload();
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

                int leftKey = leftSpecialKeys[GetKeyIndex(leftSpecialKeys.Length)];
                int rightKey = rightSpecialKeys[GetKeyIndex(rightSpecialKeys.Length)];

                GenerateTrack(GetKey(leftKey,rightKey)); //generate special

                IncrementTracks();
            }

            if (lastTrack.transform.position.y > maxY)
            {
                GenerateTrack(GetKey(leftRoadKey,rightRoadKey)); //generate normal
                IncrementTracks();

                if (RollDirectionChange())
                {
                    worldDirection = GetOppositeDirection(worldDirection);
                    GenerateTrack(GetKey(leftTurnRoadKey, rightTurnRoadKey)); //generate turn
                    IncrementTracks();
                }
            }

            yield return null;
        }
    }
    private IEnumerator DebugSpawnLoop()
    {
        worldDirection = WorldDirection.left;

        Poolable startRoad = pooler.GetFromPool(startAreaKey);
        TrackData startTrack = startRoad.GetComponent<TrackData>();
        startTrack.transform.position = transform.position;
        lastTrack = startTrack;

        yield return new WaitUntil(() => gameActive.value);

        float timeElapsed=0;
        float timeToWait = Random.Range(specialsCooldownMin, speciaCooldownMax);

        while (gameActive.value)
        {
            if (spawnNormally)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed > timeToWait)
                {
                    timeToWait = Random.Range(specialsCooldownMin, speciaCooldownMax);
                    timeElapsed = 0f;

                    int leftKey = leftSpecialKeys[GetKeyIndex(leftSpecialKeys.Length)];
                    int rightKey = rightSpecialKeys[GetKeyIndex(rightSpecialKeys.Length)];

                    GenerateTrack(GetKey(leftKey, rightKey)); //generate special

                    IncrementTracks();
                }
            }

            if (lastTrack.transform.position.y > maxY)
            {
                GenerateTrack(GetKey(leftRoadKey, rightRoadKey)); //generate normal
                IncrementTracks();

                if (spawnNormally)
                {
                    if (RollDirectionChange())
                    {
                        worldDirection = GetOppositeDirection(worldDirection);
                        GenerateTrack(GetKey(leftTurnRoadKey, rightTurnRoadKey)); //generate turn
                        IncrementTracks();
                    }
                }
            }
            yield return null;
        }
    }

    private int GetKeyIndex(int max)
    {
        int index = rnd.Next(max);
        return index;
    }

    private void IncrementTracks()
    {
        for (int i = pooler.activeObjects.Count - 1; i >= 0; i--)
        {
            pooler.activeObjects[i].transform.position += new Vector3(0, 0, 0.5f);
        }
    }

    public void GenerateTrack(int key)
    {
        Poolable track = pooler.GetFromPool(key);
        TrackData newTrack = track.GetComponent<TrackData>();

        newTrack.transform.position = lastTrack.BackConnection;
        lastTrack = newTrack;
    }

    public void GameReload()
    {
#if UNITY_ANDROID
        if(loop!=null)
            StopCoroutine(loop);
        loop = SpawnLoop();
        StartCoroutine(loop);
#endif
#if UNITY_EDITOR
        if(loop!=null)
            StopCoroutine(loop);
        loop = DebugSpawnLoop();
        StartCoroutine(loop);
#endif
    }

    public int GetKey(int left, int right)
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
    
    private WorldDirection GetOppositeDirection(WorldDirection current)
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

    public void SwitchDirection(WorldDirection newDirection)
    {
        worldDirection = newDirection;
        GenerateTrack(GetKey(leftTurnRoadKey, rightTurnRoadKey)); //generate turn
        IncrementTracks();
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

            if (pooler.activeObjects[i].transform.position.y > 28)
                pooler.activeObjects[i].EndReached();
        }
        OnWorldMove.Raise();
    }
}