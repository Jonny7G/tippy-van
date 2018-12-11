using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateManager : MonoBehaviour
{
    [Header("Animation fields")]
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private string rightTurnTriggerName;
    [SerializeField] private string leftTurnTriggerName;

    [SerializeField] private string resetTriggerName;

    [SerializeField] private string wrongLeftTurnTriggerName;
    [SerializeField] private string wrongRightTurnTriggerName;

    [SerializeField] private string leftFallTriggerName;
    [SerializeField] private string rightFallTriggerName;

    [Header("SO's")]
    [SerializeField] private WorldDirectionReference playerDirection;
    [SerializeField] private GameStateManager gameState;

    [Header("Events")]
    [SerializeField]private GameEvent OnAnimEnd;

    private bool inFailTurn;

    #region old delegates(in case of reference)
    /*
    private void Start()
    {
        //playerDirectionData.OnTurn += SwitchState;
        //playerDirectionData.onDirectionReset += ResetAnimator;
        //playerDirectionData.OnMissedTurn += MissedTurnFallAnim;
        //playerDirectionData.OnWrongTurn += WrongTurnAnim;
        //gameState.onGameReload += () => inFailTurn = false;
    }

    private void OnDestroy()
    {
        //playerDirectionData.OnTurn -= SwitchState;
        //playerDirectionData.onDirectionReset -= ResetAnimator;
        //playerDirectionData.OnMissedTurn -= MissedTurnFallAnim;
        //playerDirectionData.OnWrongTurn -= WrongTurnAnim;
    }
    */
    #endregion

    public void ResetVarialbles() => inFailTurn = false;

    public void ResetAnimator() => playerAnimator.SetTrigger(resetTriggerName);

    public void WrongTurnAnim()
    {
        inFailTurn = true;
        string stateName=null;

        switch (playerDirection.direction)
        {
            case WorldDirection.left:
                stateName = wrongRightTurnTriggerName;
                break;
            case WorldDirection.right:
                stateName = wrongLeftTurnTriggerName;
                break;
            default:
                break;
        }

        playerAnimator.SetTrigger(stateName);
    }

    public void MissedTurnFallAnim()
    {
        inFailTurn = true;
        string stateName = null;
        switch (playerDirection.direction)
        {
            case WorldDirection.left:
                stateName = leftFallTriggerName;
                break;
            case WorldDirection.right:
                stateName = rightFallTriggerName;
                break;
            default:
                break;
        }
        playerAnimator.SetTrigger(stateName);
    }

    public void SwitchState()
    {
        if (!inFailTurn)
        {
            string stateName=null;

            switch (playerDirection.direction)
            {
                case WorldDirection.left:
                    stateName = rightTurnTriggerName;
                    break;
                case WorldDirection.right:
                    stateName = leftTurnTriggerName;
                    break;
                default:
                    break;
            }
            playerAnimator.SetTrigger(stateName);
        }
    }

    public void TurnEnd() => OnAnimEnd.Raise();

    //public void SwitchDirection() => SwitchDirection();

    public void GameOver() => gameState.GameOver();
}