using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private float spawnDelay;
    [Header("Track prefabs")]
    [SerializeField] private TrackData straightTrack;
    [SerializeField] private TrackData sideTrack;
    [SerializeField] private TrackData upTurnTrack;
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

    private bool straight = true;
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
        currentTrack = straightTrack;
        PlaceNewTrack(currentTrack);
    }
    
    public void RestartTrackGeneration()
    {
        recentTrack = null;
        straight = true;
        currentTrack = GetCurrentDirectionTrack();
        activeTracks.Clear();
        PlaceNewTrack(currentTrack);
    }

    private void Update()
    {
        if (gameManager.GameActive)
        {
            if (recentTrack.transform.position.y > minY)
                Generate();
        }
    }
    
    private void Generate()
    {
            if (SwitchDirection())
            {
                straight = !straight;

                currentTrack = GetCurrentDirectionTrack();

                TrackData turnTrack = GetTurnTrack();
                PlaceNewTrack(turnTrack);
            }

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
        return straight ? straightTrack : sideTrack;
    }

    private TrackData GetTurnTrack()
    {
        return straight ? leftTurnTrack : upTurnTrack;
    }

    private void PlaceNewTrack(TrackData track)
    {
        GameObject newTrack = objectPooler.GetFromPool(track.PoolTag);

        if (recentTrack != null)
            newTrack.transform.position = new Vector3(recentTrack.BackConnection.x, recentTrack.BackConnection.y, defaultSpawnPos.z);
        else
            newTrack.transform.position = defaultSpawnPos;
        
        recentTrack = newTrack.GetComponent<TrackData>();
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