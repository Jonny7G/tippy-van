using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [Header("Variables")]
    [SerializeField] private FloatVariable worldSpeed;
    [SerializeField] private WorldDirectionVariable direction;
    [SerializeField] private Vector3Variable directionCoords;
    [Header("References")]
    [SerializeField] private BoolReference gameActive;

    public float DefaultWorldSpeed { get { return _defaultWorldSpeed; } }

    private bool canTurn;
    private IEnumerator accelerate;
    private IEnumerator decelerate;
    private Collider2D[] results = new Collider2D[10];
    private void Update()
    {
        if (gameActive.value)
        {
            if (Grounded())
            {
                if (canTurn)
                {
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Began)
                        {
                            activateTurnAnim?.Invoke();
                            SlowDown();
                            canTurn = false;
                        }
                    }
                }
            }
            else
            {
                //gameover stuff
                //TODO: implement reseting on game reset in this script that isnt buggy as fuck.
            }
        }
    }
    private bool Grounded()
    {
        return Physics2D.OverlapCircleNonAlloc(transform.position, groundCheckRadius, results, allTiles) > 0;
    }
    #region helper functions
    private void SwitchDirection()
    {
        if (direction.value == WorldDirection.left)
            SetDirection(WorldDirection.right, Vector2.up);
        else
            SetDirection(WorldDirection.left, Vector2.right);
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
    public void SlowDown()
    {
        if (accelerate != null)
            StopCoroutine(accelerate);
        if (decelerate != null)
            StopCoroutine(decelerate);

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
        canTurn = true;

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
        float perc = 0;
        while (worldSpeed.Value < DefaultWorldSpeed)
        {
            worldSpeed.Value = DefaultWorldSpeed * accelaration.Evaluate(perc);
            perc += Time.deltaTime * 2.3f;

            yield return null;
        }
        worldSpeed.Value = Mathf.Clamp(worldSpeed.Value, 0, DefaultWorldSpeed);
        canTurn = true;
    }
    #endregion /coroutines

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}