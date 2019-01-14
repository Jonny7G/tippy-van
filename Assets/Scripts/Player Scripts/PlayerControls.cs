using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerAnimationStateManager))]
public class PlayerControls : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask allTiles;
    [SerializeField] private float _defaultWorldSpeed = 0f;
    [SerializeField] private float bottomOutValue = 0f;
    [SerializeField] private AnimationCurve decelaration;
    [SerializeField] private AnimationCurve accelaration;
    [SerializeField] private UnityEvent activateTurnAnim;
    [SerializeField] private UnityEvent activateDeathAnim;
    [Header("Variables")]
    [SerializeField] private FloatVariable worldSpeed;
    [SerializeField] private WorldDirectionVariable direction;
    [SerializeField] private Vector3Variable directionCoords;
    [Header("References")]
    [SerializeField] private BoolReference gameActive;
    [SerializeField] private BoolVariable canTurn;
    [Header("Events")]
    [SerializeField] private GameEvent OnTurn;
    [SerializeField] private GameEvent OnDirectionSwitch;
    public float DefaultWorldSpeed { get { return _defaultWorldSpeed; } }

    //private bool canTurn = true;
    private bool deathActivated = false;
    private bool initialStart;
    private IEnumerator accelerate;
    private IEnumerator decelerate;
    private Collider2D[] results = new Collider2D[10];
    
    #region reload behaviour
    private void Start()
    {
        GameState.instance.OnGameStart += OnGameReset;
        GameState.instance.OnGameReload += OnGameReset;
    }
    private void OnDisable()
    {
        GameState.instance.OnGameStart -= OnGameReset;
        GameState.instance.OnGameReload -= OnGameReset;
    }
    public void OnGameReset()
    {
        StopSpeedChanges();
        canTurn.Value = true;
        deathActivated = false;
        initialStart = false;
        SetDirection(WorldDirection.left, Vector2.right);
        transform.parent.position = Vector3.zero + new Vector3(0, 0, -20);
        worldSpeed.Value = DefaultWorldSpeed;
    }
    #endregion

    private void Update()
    {
        if (GameState.GameActive)
        {
            if (initialStart)
            {
                if (Grounded())
                {
                    if (canTurn.Value)
                    {
#if UNITY_ANDROID
                        if (Input.touchCount > 0)
                        {
                            Touch touch = Input.GetTouch(0);
                            if (touch.phase == TouchPhase.Began)
                            {
                                OnTurn.Raise();
                                activateTurnAnim?.Invoke();
                                SlowDown();
                                canTurn.Value = false;
                            }
                        }
#endif
#if UNITY_EDITOR
                        if (Input.GetMouseButtonDown(0))
                        {
                            OnTurn.Raise();
                            activateTurnAnim?.Invoke();
                            SlowDown();
                            canTurn.Value = false;
                        }
#endif
                    }
                }
                else
                {
                    if (!deathActivated)
                    {
                        activateDeathAnim?.Invoke();
                        deathActivated = true;
                    }
                    //gameover stuff
                    //TODO: implement reseting on game reset in this script that isnt buggy as fuck.
                }
            }
            else
            {
                if (Physics2D.OverlapCircleNonAlloc(transform.position, groundCheckRadius, results, allTiles) > 0)
                    initialStart = true;
            }
        }
    }
    private bool Grounded()
    {
        int colliderCount = Physics2D.OverlapCircleNonAlloc(transform.position, groundCheckRadius, results, allTiles);

        return colliderCount > 0;
    }

    #region helper functions
    private void SwitchDirection()
    {
        if (direction.value == WorldDirection.left)
            SetDirection(WorldDirection.right, Vector2.up);
        else
            SetDirection(WorldDirection.left, Vector2.right);

        OnDirectionSwitch?.Raise();
    }
    private void SetDirection(WorldDirection direction, Vector3 coordinates)
    {
        this.direction.value = direction;
        coordinates = GetIsometricCordinate(coordinates);
        directionCoords.Value = coordinates;
    }
    private Vector2 GetIsometricCordinate(Vector2 cord)
    {
        Vector2 newCord = new Vector2();
        newCord.x = cord.x - cord.y;
        newCord.y = (cord.x + cord.y) / 2;

        return newCord;
    }
    #endregion /helper functions
    #region coroutines 
    public void StopSpeedChanges()
    {
        if (accelerate != null)
            StopCoroutine(accelerate);
        if (decelerate != null)
            StopCoroutine(decelerate);
    }
    public void SlowDown()
    {
        StopSpeedChanges();
        decelerate = Decelerate();
        StartCoroutine(decelerate);
    }

    private IEnumerator Decelerate()
    {
        float perc = 0;
        while (worldSpeed.Value > bottomOutValue)
        {
            worldSpeed.Value = DefaultWorldSpeed * decelaration.Evaluate(perc);
            perc += Time.deltaTime * 2;

            yield return null;
        }
        worldSpeed.Value = Mathf.Clamp(worldSpeed.Value, 0, DefaultWorldSpeed);
        SpeedUp();
    }

    public void SpeedUp()
    {
        StopSpeedChanges();
        accelerate = Accelerate();
        StartCoroutine(accelerate);
    }

    private IEnumerator Accelerate()
    {
        SwitchDirection();
        canTurn.Value = true;
        float perc = 0;
        while (worldSpeed.Value < DefaultWorldSpeed)
        {
            worldSpeed.Value = DefaultWorldSpeed * accelaration.Evaluate(perc);
            perc += Time.deltaTime * 3.2f;

            yield return null;
        }
        worldSpeed.Value = Mathf.Clamp(worldSpeed.Value, 0, DefaultWorldSpeed);

    }
    #endregion /coroutines

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}