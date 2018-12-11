using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTile : MonoBehaviour
{
    public Transform LockPosition { get { return _lockPosition; } }
    [Header("Fields")]
    [SerializeField] private Transform _lockPosition;
    [Header("SO's")]
    [SerializeField] private GameStateManager gameStateManager;
    [Header("Variables")]
    [SerializeField] private TurnTileVariable activeTurnTile;
    [SerializeField] private TransformVariable WorldObject;
    [Header("Events")]
    [SerializeField] private GameEvent TurnTriggered;
    [SerializeField] private GameEvent TurnExited;

    #region statics
    private static bool loaded = false;
    #endregion
    
    private bool wasTriggered;

    private void Start()
    {
        if(!loaded)
        {
            //gameStateManager.onGameReload += playerDirectionData.ResetTriggeredCount;
            WorldObject.Value = transform.parent;
            loaded = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!wasTriggered)
        {
            if (collision.CompareTag("Player"))
            {
                TurnTriggered.Raise();
                activeTurnTile.Value = this;
                wasTriggered = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (wasTriggered)
        {
            TurnExited.Raise();
            if (activeTurnTile.Value == this)
                activeTurnTile.Value = null;
        }
    }

    private void OnEnable()
    {
        wasTriggered = false;
    }
}