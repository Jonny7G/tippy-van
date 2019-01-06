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

    [SerializeField] private TrackData startArea;
    [SerializeField] private TrackData leftRoad;
    [SerializeField] private TrackData rightRoad;
    [SerializeField] private TrackData leftTurnRoad;
    [SerializeField] private TrackData rightTurnRoad;

    [SerializeField] private float specialsCooldownMin = 0f;
    [SerializeField] private float speciaCooldownMax = 0f;

    [SerializeField] private TrackData leftWoodBridge;
    [SerializeField] private TrackData rightWoodBridge;

    [SerializeField] private TrackData leftLogBridge;
    [SerializeField] private TrackData rightLogBridge;

    [SerializeField] private TrackData leftBrokenBridge;
    [SerializeField] private TrackData rightBrokenBridge;


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
    
    private Vector2 minScreenSpace;
    private float absoluteMaxY;

    private void Start()
    {
        pooler = ObjectPooling.instance;

        startAreaKey = pooler.GetUniqueID();
        pooler.SetPool(startArea, startAreaKey, 1);

        leftRoadKey = pooler.GetUniqueID();
        pooler.SetPool(leftRoad, leftRoadKey, 50);
        
        rightRoadKey = pooler.GetUniqueID();
        pooler.SetPool(rightRoad,rightRoadKey, 50);

        leftTurnRoadKey = pooler.GetUniqueID();
        pooler.SetPool(leftTurnRoad,leftTurnRoadKey, 10);

        rightTurnRoadKey = pooler.GetUniqueID();
        pooler.SetPool(rightTurnRoad,rightTurnRoadKey, 10);

        leftWoodBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(leftWoodBridge, leftWoodBridgeKey, 1);

        rightWoodBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(rightWoodBridge, rightWoodBridgeKey, 1);

        leftLogBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(leftLogBridge, leftLogBridgeKey, 1);

        rightLogBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(rightLogBridge, rightLogBridgeKey, 1);

        leftBrokenBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(leftBrokenBridge, leftBrokenBridgeKey, 1);

        rightBrokenBridgeKey = pooler.GetUniqueID();
        pooler.SetPool(rightBrokenBridge, rightBrokenBridgeKey, 1);

        leftSpecialKeys = new int[] { leftWoodBridgeKey, leftLogBridgeKey, leftBrokenBridgeKey };
        rightSpecialKeys = new int[] { rightWoodBridgeKey, rightLogBridgeKey, rightBrokenBridgeKey };

        absoluteMaxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight)).y+12;
        float minPadding = 7;
        Vector2 minPaddingVector = new Vector2(minPadding, minPadding);
        
        minScreenSpace = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0));
        Debug.Log(minScreenSpace);
        minScreenSpace -= minPaddingVector;
        GameReload();
    }

    private IEnumerator SpawnLoop()
    {
        worldDirection = WorldDirection.left;

        TrackData startTrack = (TrackData)pooler.GetFromPool(startAreaKey);
        startTrack.transform.position = transform.position;
        lastTrack = startTrack;

        yield return new WaitUntil(() => gameActive.value);

        float timeElapsed=0f;
        float timeToWait = Random.Range(specialsCooldownMin,speciaCooldownMax);
        float timePassed = 0f;
        while (gameActive.value)
        {
            timeElapsed += Time.deltaTime;
            if(timePassed<41)
            timePassed += Time.deltaTime;
            if (timeElapsed > timeToWait)
            {
                timeToWait = Random.Range(specialsCooldownMin, speciaCooldownMax);
                timeElapsed = 0f;

                int leftKey = leftSpecialKeys[GetKeyIndex(leftSpecialKeys.Length)];
                int rightKey = rightSpecialKeys[GetKeyIndex(rightSpecialKeys.Length)];

                GenerateTrack(GetKey(leftKey,rightKey)); //generate special

                IncrementTracks();
            }

            if (lastTrack.BackConnection.x > minScreenSpace.x && lastTrack.BackConnection.y > minScreenSpace.y)
            {
                if (timePassed < 40)
                    StartingGeneration();
                else
                    MidGeneration();
            }

            yield return null;
        }
    }
    
    private IEnumerator DebugSpawnLoop()
    {
        worldDirection = WorldDirection.left;

        TrackData startTrack = (TrackData)pooler.GetFromPool(startAreaKey);
        
        startTrack.transform.position = transform.position;
        lastTrack = startTrack;

        yield return new WaitUntil(() => gameActive.value);

        float timeElapsed=0;
        float timeToWait = Random.Range(specialsCooldownMin, speciaCooldownMax);
        float timePassed=0f;
        while (gameActive.value)
        {
            timePassed += Time.deltaTime;
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

            if (lastTrack.BackConnection.x > minScreenSpace.x && lastTrack.BackConnection.y > minScreenSpace.y)
            {
                if (spawnNormally)
                {
                    if (timePassed < 40)
                        StartingGeneration();
                    else
                        MidGeneration();
                }
                else 
                    GenerateTrack(GetKey(leftRoadKey, rightRoadKey)); //generate normal
                    IncrementTracks();

            }
            yield return null;
        }
    }
    private Vector2 GetIsometricCordinate(Vector2 cord)
    {
        Vector2 newCord = new Vector2();
        newCord.x = cord.x - cord.y;
        newCord.y = (cord.x + cord.y) / 2;

        return newCord;
    }
    private void StartingGeneration()
    {
        if (RollDirectionChange())
        {
            worldDirection = GetOppositeDirection(worldDirection);
            GenerateTrack(GetKey(leftTurnRoadKey, rightTurnRoadKey)); //generate turn
            IncrementTracks();
        }
        GenerateTrack(GetKey(leftRoadKey, rightRoadKey)); //generate normal
        IncrementTracks();
        
    }
    private void MidGeneration()
    {
        if (RollDirectionChange())
        {
            worldDirection = GetOppositeDirection(worldDirection);
            GenerateTrack(GetKey(leftTurnRoadKey, rightTurnRoadKey)); //generate turn
            IncrementTracks();   
        }
        else
        {
            GenerateTrack(GetKey(leftRoadKey, rightRoadKey)); //generate normal
            IncrementTracks();
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
            pooler.activeObjects[i].PoolObject.transform.position += new Vector3(0, 0, 0.5f);
        }
    }

    public void GenerateTrack(int key)
    {
        TrackData newTrack = (TrackData)pooler.GetFromPool(key);
        
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
        //absoluteMaxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight)).y + consecutives;
    }

    private void MoveActiveObjects()
    {
        for (int i = pooler.activeObjects.Count - 1; i >=0; i--)
        {
            TrackData track = (TrackData)pooler.activeObjects[i];

            track.PoolObject.transform.position += direction.value*Time.deltaTime* worldSpeed.value;
            if(track.BackConnection.y>absoluteMaxY)
                    track.EndReached();
        }
        OnWorldMove.Raise();
    }
}