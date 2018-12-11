using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerManager : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private LayerMask allTiles;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private float _defaultWorldSpeed;
    [Range(0, 1)]
    [SerializeField] private float decelarationMagnitude;
    [Range(0, 1)]
    [SerializeField] private float decelarationRate;
    [SerializeField] private float bottomOutValue;
    [Range(0, 1)]
    [SerializeField] private float accelarationRate;
    [Range(0,3)]
    [SerializeField] private float accelarationMagnitude;
    [SerializeField] private float yDeathPos;
    [Header("SO's")]
    [SerializeField] private GameStateManager gameStateManager;
    [Header("Variables")]
    [SerializeField] private WorldDirectionVariable direction;
    [SerializeField] private Vector3Variable directionCoords;
    [SerializeField] private FloatVariable worldSpeed;
    [SerializeField] private BoolVariable inTurn;
    [SerializeField] private BoolVariable executingTurn;
    [Header("References")]
    [SerializeField] private TurnTileReference activeTurn;
    [SerializeField] private TransformReference WorldObject;
    [Space()]
    [Header("Events")]
    [SerializeField] private GameEvent OnDirectionReset;
    [SerializeField] private GameEvent OnDirectionSwitch;
    [SerializeField] private GameEvent onTurn;
    [SerializeField] private GameEvent onTurnAchieved;//finished moving to position
    [SerializeField] private GameEvent onWrongTurn;
    [SerializeField] private GameEvent onMissedTurn;
    [Space()]
    #region Debug stuff
    [Header("DEBUG OPTIONS")]
    [SerializeField] private bool autoDirectionChange;
    #endregion

    public float DefaultWorldSpeed { get { return _defaultWorldSpeed; } }

    private bool acceptingInput = true;
    private IEnumerator accelerate;
    private IEnumerator decelerate;
    private bool roadEntered=false;
    private bool turnMissed = false;
    private RaycastHit2D[] results;

    private void Start()
    {
        results = new RaycastHit2D[20];
        ResetOnReload();
    }

    #region turning behaviour
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
        if (gameStateManager.GameActive)
        {
            if (!autoDirectionChange)
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
        }
        if (roadEntered&&!turnMissed) //checks for ground to start loose condition.
        {
            if (Physics2D.RaycastNonAlloc(raycastPoint.position, Vector3.forward, results, 100f, allTiles) == 0)
            {
                turnMissed = true;
                onMissedTurn.Raise();
            }
        }
        else if (!roadEntered) //once ground is found checks if it left ground then triggers loss if so.
            if (Physics2D.RaycastNonAlloc(raycastPoint.position, Vector3.forward, results, 100f, allTiles) > 0)
                roadEntered = true;                    
            
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

    public void ResetOnReload()
    {
        StopAllCoroutines();
        roadEntered = false;
        turnMissed = false;
        transform.position = Vector3.zero + new Vector3(0, 0, -20);
        ResetDirection();
        inTurn.Value = false;
        acceptingInput = true;
        worldSpeed.Value = DefaultWorldSpeed;
        
        executingTurn.Value = false;
        SetDirection(WorldDirection.left, Vector2.right);
    }
    #endregion

    #region coroutines
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
            Debug.Log("Decelarating");
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
            Debug.Log("Accelerating");
            worldSpeed.Value *= accelarationMagnitude;
            worldSpeed.Value = Mathf.Clamp(worldSpeed.Value, 0.1f, DefaultWorldSpeed);

            yield return new WaitForSeconds(accelarationRate);
        }
    }
    #endregion


}