using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackData))]
public class BrokenBridge : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private BezierCurvePoints bezierPoints;
    [Header("References")]
    [SerializeField] private Transform player;

    private bool triggered=false;
    private bool inputRecieved;
    private TrackData trackData;
    [SerializeField] private GameEvent bridgeMissed;
    [SerializeField] private GameEvent bridgeEntered;
    [SerializeField] private GameEvent bridgeExecuted;
    private void Start()
    {
        player = GameObject.Find("PlayerHolder").transform;
        trackData = GetComponent<TrackData>();
        LoadCoins();
    }

    private void LoadCoins()
    {
        int coinAmount = trackData.containedCoins.Length;
        for (int i = 1; i < coinAmount+ 1; i++)
        {
            float t = i /(float)coinAmount;
            trackData.containedCoins[i - 1].transform.position = bezierPoints.GetBezierPoint(t);
        }
    }

    private void Update()
    {
        if (triggered)
        {
#if UNITY_EDITOR
            if (!inputRecieved)
                if (Input.GetMouseButtonDown(0))
                {
                    inputRecieved = true;
                    StartCoroutine(BridgeCheck());
                }
#endif
#if UNITY_ANDROID
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        if (!inputRecieved)
                        {
                            inputRecieved = true;
                            StartCoroutine(BridgeCheck());
                        }
                    }
                }
#endif
        }
    }

    private IEnumerator MovePlayer()
    {
        float t=0;
        while (t<1 )
        {
            t = Mathf.InverseLerp(bezierPoints.startPoint.position.x, bezierPoints.endPoint.position.x, player.position.x);
            player.position = new Vector3(0, bezierPoints.GetBezierPoint(t).y, 0);

            yield return null;
        }
    }

    private IEnumerator BridgeCheck()
    {
        float timeElapsed = 0;
        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime;
            if(inputRecieved)
            {
                StartCoroutine(MovePlayer());
                bridgeExecuted.Raise();
                break;
            }

            yield return null;
        }
        //bridgeMissed.Raise();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
            bridgeEntered.Raise();
        triggered = true;
    }
    private void OnDisable()
    {
        triggered = false;
        inputRecieved = false;
    }
}

#region structs
[System.Serializable]
struct BezierCurvePoints
{
    public Transform startPoint;
    public Transform midPoint;
    public Transform endPoint;

    public Vector3 GetBezierPoint(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * startPoint.position;

        point += 2 * u * t * midPoint.position;
        point += tt * endPoint.position;

        return point;
    }
}
#endregion