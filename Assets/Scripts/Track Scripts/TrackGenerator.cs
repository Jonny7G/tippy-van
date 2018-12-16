using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private float spawnDelay;
    [Header("Track prefabs")]
    [SerializeField] private TrackData startArea;
    [SerializeField] private TrackData leftTrack;
    [SerializeField] private TrackData rightTrack;

    [SerializeField] private TrackData rightTurnTrack;
    [SerializeField] private TrackData leftTurnTrack;
    [Space()]
    [Header("Track fields")]
    [SerializeField] private float minY;
    [Space()]
    [Range(0f,1f)]
    [SerializeField] private float directionSwitchChance;
    [Space()]
    [Header("SO's")]
    [SerializeField] private GameStateManager gameManager;

    public static TrackGenerator instance;

    private bool left = true;
    private Vector3 defaultSpawnPos;
    private int sideTileGroupCount = 0;
    private int straightTileGroupCount = 0;
    private ObjectPooler objectPooler;
    private TrackData recentTrack;
    private TrackData currentTrack;
    private float timePassed;
    private List<TrackData> activeTracks = new List<TrackData>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timePassed = spawnDelay;
        defaultSpawnPos = transform.position;
        objectPooler = ObjectPooler.instance;
        RestartTrackGeneration();
    }
    
    public void RestartTrackGeneration()
    {
        for (int i = activeTracks.Count - 1; i >= 0; i--)
            activeTracks[i].GetComponent<IEntity>().EndReached();

        recentTrack = null;
        left = true;
        activeTracks.Clear();

        StopAllCoroutines();
        StartCoroutine(TrackSpawnLoop());
        //PlaceNewTrack(currentTrack);
    }

    //private void Update()
    //{
    //    if (gameManager.GameActive)
    //    {
    //        if (recentTrack.transform.position.y > minY)
    //            Generate();
    //    }
    //}
    
    private IEnumerator TrackSpawnLoop()
    {
        //GameObject start = objectPooler.GetFromPool(startArea.PoolTag);
        //currentTrack = start.GetComponent<TrackData>();
        currentTrack.transform.position = defaultSpawnPos;

        recentTrack = currentTrack;
        
        while (gameManager.GameActive)
        {
            if (recentTrack.transform.position.y > minY)
            {
                Generate();
            }

            yield return null;
        }
    }

    private void Generate()
    {
            if (SwitchDirection())
            {
                left = !left;

                currentTrack = GetCurrentDirectionTrack();

                TrackData turnTrack = GetTurnTrack();
                PlaceNewTrack(turnTrack);
            }
        currentTrack = GetCurrentDirectionTrack();
        PlaceNewTrack(currentTrack);
    }

    private bool SwitchDirection()
    {
        return Random.Range(0f, 1f) <= directionSwitchChance;
    }

    public void RemoveActive(TrackData item)
    {
        activeTracks.Remove(item);
    }

    private TrackData GetCurrentDirectionTrack()
    {
        return left ? leftTrack : rightTrack;
    }

    private TrackData GetTurnTrack()
    {
        return left ? leftTurnTrack : rightTurnTrack;
    }

    private void PlaceNewTrack(TrackData track)
    {
        //GameObject newTrack = objectPooler.GetFromPool(track.PoolTag);

        //if (track == startArea)
        //    Debug.Log(newTrack.activeSelf);

        //if (recentTrack != null)
        //    newTrack.transform.position = recentTrack.BackConnection;
        //else
        //{
        //    newTrack.transform.position = defaultSpawnPos;
        //}
        //recentTrack = newTrack.GetComponent<TrackData>();
        activeTracks.Add(recentTrack);

        for (int i = activeTracks.Count - 1; i >= 0; i--)
        {
            activeTracks[i].transform.position += new Vector3(0, 0, 0.5f);
        }
    }

    private Vector2 GetIsometricCordinate(Vector2 cord)
    {
        Vector2 newCord = new Vector2();
        newCord.x = cord.x - cord.y;
        newCord.y = (cord.x + cord.y) / 2;

        return newCord;
    }
}