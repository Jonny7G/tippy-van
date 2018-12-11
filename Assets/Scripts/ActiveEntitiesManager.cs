using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEntitiesManager : MonoBehaviour
{
    [Header("fields")]
    [SerializeField] private float worldMovementSpeed = 2f;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;
    [Header("References")]
    [SerializeField] private Vector3Reference playerDirection;
    [SerializeField] private BoolReference playerExecutingTurn;
    [SerializeField] private FloatReference WorldSpeed;
    [Header("SO's")]
    [SerializeField] private GameStateManager gameState;
    [SerializeField] private CameraMovement cameraTrans;

    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
    }

    private void Update()
    {
        if (gameState.GameActive)
        {
            if (!playerExecutingTurn.value)
            {
                List<IEntity> entitiesExpired = new List<IEntity>();

                if (objectPooler.ActiveEntities.Count > 0)
                    foreach (GameObject entity in objectPooler.ActiveEntities)
                    {
                        entity.transform.position += playerDirection.value * Time.deltaTime * WorldSpeed.value;

                        if (entity.transform.position.x > maxX || entity.transform.position.y > maxY)
                            entitiesExpired.Add(entity.GetComponent<IEntity>());
                    }
                if (entitiesExpired.Count > 0)
                    foreach (IEntity entity in entitiesExpired)
                    {
                        entity.EndReached();
                    }
                cameraTrans.MoveCam(worldMovementSpeed);
            }
        }
    }
}