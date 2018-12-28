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
    [Header("SO's")]
    [SerializeField] private GameStateManager gameState;
    [Header("Events")]
    [SerializeField]private GameEvent OnAnimEnd;
    [SerializeField] private GameEvent OnGameOver;

    private bool inFailTurn;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ResetVarialbles()
    {
        
        inFailTurn = false;
        spriteRenderer.flipX = false;
    }
    public void ResetAnimator()
    {
        spriteRenderer.flipX = false;
        playerAnimator.SetTrigger(resetTriggerName);
    }
    public void WrongTurnAnim()
    {
        inFailTurn = true;
        playerAnimator.SetTrigger(wrongTurnTriggerName);
    }
    public void MissedTurnFallAnim()
    {
        inFailTurn = true;
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
    public void SwitchState()
    {
        if (!inFailTurn)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            playerAnimator.SetTrigger(turnTrigger);
        }
    }
    public void TurnEnd() => OnAnimEnd.Raise();
    public void GameOver() => OnGameOver.Raise();
}