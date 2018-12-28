using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerManager : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private LayerMask allTiles;
    [SerializeField] private float _defaultWorldSpeed=0f;
    [Range(0, 1)]
    [SerializeField] private float decelarationMagnitude=0f;
    [Range(0, 1)]
    [SerializeField] private float decelarationRate=0f;
    [SerializeField] private float bottomOutValue=0f;
    [Range(0, 1)]
    [SerializeField] private float accelarationRate=0f;
    [Range(0,3)]
    [SerializeField] private float accelarationMagnitude=0f;
    [Header("Variables")]
    [SerializeField] private TransformVariable playerTransform;
    [SerializeField] private WorldDirectionVariable direction;
    [SerializeField] private Vector3Variable directionCoords;
    [SerializeField] private FloatVariable worldSpeed;
    [SerializeField] private BoolVariable inTurn;
    [SerializeField] private BoolVariable executingTurn;
    [Header("References")]
    [SerializeField] private BoolReference gameActive;
    [SerializeField] private TransformReference WorldObject;
    [Space()]
    [Header("Events")]
    [SerializeField] private GameEvent OnDirectionReset;
    [SerializeField] private GameEvent OnDirectionSwitch;
    [SerializeField] private GameEvent onTurn;
    [SerializeField] private GameEvent onMissedTurn;
    [SerializeField] private GameEvent OnGameOver;

    public float DefaultWorldSpeed { get { return _defaultWorldSpeed; } }

    private bool acceptingInput = true;
    private IEnumerator accelerate;
    private IEnumerator decelerate;
    private bool roadEntered = false;
    private bool activatedAnim = false;
    private RaycastHit2D[] results;
    private SpriteRenderer sr;
    private Vector2 lastHitPos;
    private Transform lastWorldPoint;
    private float checkCooldown=0.1f;

    private void Start()
    {
        lastWorldPoint = new GameObject("last world point").transform;
        playerTransform.Value = this.transform.parent;
        sr = GetComponent<SpriteRenderer>();
        worldSpeed.Value = 0;
        results = new RaycastHit2D[20];
        ResetOnReload();
    }

    private void SetDirection(WorldDirection direction, Vector3 coordinates)
    {
        this.direction.value = direction;
        coordinates = GetIsometricCordinate(coordinates);
        directionCoords.Value = coordinates;
    }

    private void ActivateAnim()
    {
        onTurn.Raise();
    }

    private void Update()
    {
        if (gameActive.value)
        {
            if (roadEntered) //checks for ground to start loose condition.
            {
                    if (!activatedAnim)
                    {
                        if (Physics2D.RaycastNonAlloc(transform.position, Vector3.forward, results, 100f, allTiles) == 0)
                        {
                            activatedAnim = true;
                            acceptingInput = false;
                            onMissedTurn.Raise();
                        }
                        else
                            lastWorldPoint.position = transform.position;
                    }
            }
            else if (!roadEntered) //once ground is found checks if it left ground then triggers loss if so.
            {
                if (Physics2D.RaycastNonAlloc(transform.position, Vector3.forward, results, 100f, allTiles) != 0)
                {
                    acceptingInput = true;
                    roadEntered = true;
                }
            }
        }

        if (!acceptingInput)
        {
            if (!gameActive.value || !roadEntered)
            {
                if (direction.value != WorldDirection.left)
                    SetDirection(WorldDirection.left, Vector2.right);
                if (sr.flipX)
                    sr.flipX = false;
            }
        }
#if UNITY_ANDROID
        if (gameActive.value)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (acceptingInput)
                    {
                        acceptingInput = false;
                        SlowDown();
                        ActivateAnim();
                    }
                }
            }
        }
#endif
#if UNITY_EDITOR
        if (gameActive.value)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (acceptingInput)
                {
                    acceptingInput = false;
                    SlowDown();
                    ActivateAnim();
                }
            }
        }
#endif
    }

    private void SwitchDirection()
    {
        if (direction.value == WorldDirection.left)
            SetDirection(WorldDirection.right, Vector2.up);
        else
            SetDirection(WorldDirection.left, Vector2.right);

        OnDirectionSwitch.Raise();
    }

    public void ResetDirection()
    {
        OnDirectionReset.Raise();
        executingTurn.Value = false;
        inTurn.Value = false;

        SetDirection(WorldDirection.left, Vector2.right);
    }
    
    private Vector2 GetIsometricCordinate(Vector2 cord)
    {
        Vector2 newCord = new Vector2();
        newCord.x = cord.x - cord.y;
        newCord.y = (cord.x + cord.y) / 2;

        return newCord;
    }
    public void StartGame()
    {
        worldSpeed.Value = DefaultWorldSpeed;
    }
    public void ResetOnReload()
    {
        StopAllCoroutines();
        activatedAnim = false;
        roadEntered = false;
        transform.parent.position = Vector3.zero + new Vector3(0, 0, -20);
        ResetDirection();
        inTurn.Value = false;
        acceptingInput = false;
        worldSpeed.Value = DefaultWorldSpeed;
        executingTurn.Value = false;
        SetDirection(WorldDirection.left, Vector2.right);
    }
    
    public void SlowDown()
    {
        if(accelerate!=null)
            StopCoroutine(accelerate);
        if (decelerate != null)
            StopCoroutine(decelerate);

        decelerate = Decelerate();
        StartCoroutine(decelerate);
    }

    private IEnumerator Decelerate()
    {
        while (worldSpeed.Value > bottomOutValue)
        {
            worldSpeed.Value *= decelarationMagnitude;
            
            yield return new WaitForSeconds(decelarationRate);
        }
        SpeedUp();
    }

    public void SpeedUp()
    {
        acceptingInput = true;

        if (accelerate != null)
            StopCoroutine(accelerate);
        if (decelerate != null)
            StopCoroutine(decelerate);

        accelerate = Accelerate();
        StartCoroutine(accelerate);
    }

    private IEnumerator Accelerate()
    {
        SwitchDirection();
        while (worldSpeed.Value < DefaultWorldSpeed)
        {
            worldSpeed.Value *= accelarationMagnitude;
            worldSpeed.Value = Mathf.Clamp(worldSpeed.Value, 0.1f, DefaultWorldSpeed);

            yield return new WaitForSeconds(accelarationRate);
        }
    }
}