using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateManager : MonoBehaviour
{

    [Header("Strings")]
    [SerializeField] private string turnTrigger;
    [SerializeField] private string resetTriggerName;
    [SerializeField] private string wrongTurnTriggerName;
    [SerializeField] private string fallTriggerLeftName;
    [SerializeField] private string fallTriggerRightName;

    [Space()]
    [Header("References")]
    [SerializeField] private WorldDirectionReference playerDirection;
    [SerializeField] private VehicleReference equipedVehicle;
    [SerializeField] private BoolReference gameActive;
    [Header("Events")]
    [SerializeField]private GameEvent OnAnimEnd;

    private bool inFailTurn;
    private bool outOfTurn=true;
    private Animator playerAnimator;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerAnimator[] allVehicleAnimators;

    [SerializeField] private PlayerAnimator activeVehicleAnimator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameState.instance.OnGameReload += ResetVarialbles;
        GameState.instance.OnGameStart += TriggerStart;
    }

    #region reload behaviour
    private void ResetVarialbles()
    {
        Debug.Log("reset animator variables");
        inFailTurn = false;
        spriteRenderer.flipX = false;
        playerAnimator.SetTrigger(resetTriggerName);
    }
    private void TriggerStart()=>playerAnimator.SetTrigger("Start");
    
    private void OnDisable()
    {
        GameState.instance.OnGameReload -= ResetVarialbles;
        GameState.instance.OnGameStart -= TriggerStart;
    }
    #endregion

    public void WrongTurnAnim()
    {
        inFailTurn = true;
        playerAnimator.SetTrigger(wrongTurnTriggerName);
    }
    public void MissedTurnFallAnim()
    {
        if (GameState.GameActive)
        {
            inFailTurn = true;
            AudioManager.instance.PlaySound("car fall");
            switch (playerDirection.direction)
            {
                case WorldDirection.left:
                    playerAnimator.SetTrigger(fallTriggerLeftName);
                    break;
                case WorldDirection.right:
                    playerAnimator.SetTrigger(fallTriggerRightName);
                    break;
            }
        }
    }
    public void SwitchState()
    {
        if (GameState.GameActive)
        {
            if (!inFailTurn)
            {
                if (outOfTurn)
                {
                    AudioManager.instance.PlaySound("car turn");
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                    playerAnimator.SetTrigger(turnTrigger);
                }
            }
        }
    }
    public void TriggerAnim(string triggerName)
    {
        playerAnimator.SetTrigger(triggerName);
    }
    public void SetAnimator()
    {
        foreach(PlayerAnimator animator in allVehicleAnimators)
        {
            if (animator.vehicleType == equipedVehicle.Value)
            {
                activeVehicleAnimator = animator;
                playerAnimator.runtimeAnimatorController = activeVehicleAnimator.animator;
            }
        }
    }
    public void TurnEnd()
    {
        outOfTurn = true;
        OnAnimEnd.Raise();
    }
    public void GameOver()
    {
        AudioManager.instance.PlaySound("car crash");
        GameState.instance.GameEnd();
    }
}
[System.Serializable]
public struct PlayerAnimator
{
    public Vehicles vehicleType;
    public RuntimeAnimatorController animator;
    public Sprite idleSprite;
}