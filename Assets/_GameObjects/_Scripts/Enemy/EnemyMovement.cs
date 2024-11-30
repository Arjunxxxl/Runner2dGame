using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyMovementType enemyMovementType = EnemyMovementType.Unknown;

    [Header("Patrolling Points")]
    [SerializeField] private List<GameObject> patrollingPoint;

    [Header("Movement Data")]
    [SerializeField] private float enemyBaseMoveSpeed;
    private float enemyMoveSpeed;
    private int targetPatrolPointIdx;
    [SerializeField] private Vector2 patrolDurationRange;
    [SerializeField] private Vector2 idleDurationRange;
    private float currentMovementDuration;
    private float currentTimeElapsed;

    [Header("Override Data")] 
    [SerializeField] private bool overridePatrolDuration;
    [SerializeField] private bool overrideIdleDuration;
    [SerializeField] private Vector2 overridePatrolDurationRange;
    [SerializeField] private Vector2 overrideIdleDurationRange;
    [SerializeField] private bool removeWaitingToStartChase;
    [SerializeField] private bool removeChaseIdle;

    [Header("Chasing Player")]
    private Player TargetedPlayer;

    [Header("Flip")] 
    private Vector3 localScale;

    private Enemy enemy;
    
    private void Update()
    {
        UpdateMoveDuration();
    }
    
    internal void SetUp(Enemy enemy, List<GameObject> patrollingPoint, Vector2 idleDurationRange, Vector2 patrolDurationRange)
    {
        this.enemy = enemy;
        this.patrollingPoint = patrollingPoint;
        this.idleDurationRange = idleDurationRange;
        this.patrolDurationRange = patrolDurationRange;

        if (overrideIdleDuration)
        {
            this.idleDurationRange = overrideIdleDurationRange;
        }
        
        if (overridePatrolDuration)
        {
            this.patrolDurationRange = overridePatrolDurationRange;
        }
        
        targetPatrolPointIdx = -1;
        localScale = Vector3.one;

        enemyMoveSpeed = enemyBaseMoveSpeed +
                         Random.Range(Constants.EnemyMoveSpeedVariation.x, Constants.EnemyMoveSpeedVariation.y);
        
        EnemyMovementType enemyMovementType = Random.value >= 0.5f ? EnemyMovementType.Idle : EnemyMovementType.Patrolling;
        SetEnemyMovementType(enemyMovementType);
    }

    #region Movement Type

    private void SetEnemyMovementType(EnemyMovementType enemyMovementType)
    {
        if (this.enemyMovementType == enemyMovementType)
        {
            return;
        }

        if (enemyMovementType == EnemyMovementType.Idle)
        {
            currentMovementDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
        }
        else if (enemyMovementType == EnemyMovementType.Patrolling)
        {
            if (targetPatrolPointIdx == -1)
            {
                targetPatrolPointIdx = Random.Range(0, patrollingPoint.Count);
            }
            else
            {
                targetPatrolPointIdx++;
                if (targetPatrolPointIdx >= patrollingPoint.Count)
                {
                    targetPatrolPointIdx = 0;
                }
            }

            currentMovementDuration = Random.Range(patrolDurationRange.x, patrolDurationRange.y);
            
            UpdateLookDir();
        }
        else if (enemyMovementType == EnemyMovementType.WaitingToStartChase)
        {
            currentMovementDuration = removeWaitingToStartChase ? 
                0 : 
                Random.Range(Constants.EnemyChaseWaitDurationRange.x, Constants.EnemyChaseWaitDurationRange.y);
        }
        
        currentTimeElapsed = 0;
        this.enemyMovementType = enemyMovementType;

        UpdateAnimation(enemyMovementType);
    }

    private void UpdateMoveDuration()
    {
        if (enemyMovementType == EnemyMovementType.Idle)
        {
            currentTimeElapsed += Time.deltaTime;

            if (currentTimeElapsed >= currentMovementDuration)
            {
                SetEnemyMovementType(EnemyMovementType.Patrolling);
            }
        }
        else if (enemyMovementType == EnemyMovementType.Patrolling)
        {
            currentTimeElapsed += Time.deltaTime;

            transform.position =
                Vector3.MoveTowards(transform.position, patrollingPoint[targetPatrolPointIdx].transform.position, Time.deltaTime * enemyBaseMoveSpeed);

            if (Vector3.Distance(transform.position, patrollingPoint[targetPatrolPointIdx].transform.position) < 0.01f)
            {
                SetEnemyMovementType(EnemyMovementType.Idle);
            }

            if (currentTimeElapsed >= currentMovementDuration)
            {
                SetEnemyMovementType(EnemyMovementType.Idle);
            }
        }
        else if (enemyMovementType == EnemyMovementType.WaitingToStartChase)
        {
            currentTimeElapsed += Time.deltaTime;

            if (currentTimeElapsed >= currentMovementDuration)
            {
                if (TargetedPlayer != null)
                {
                    SetEnemyMovementType(EnemyMovementType.Chasing);
                }
            }
            
            UpdateLookDir();
        }
        else if (enemyMovementType == EnemyMovementType.Chasing)
        {
            Vector3 destination = TargetedPlayer.transform.position;
            destination.y = patrollingPoint[0].transform.position.y;
            
            float minX = Mathf.Min(patrollingPoint[0].transform.position.x, patrollingPoint[1].transform.position.x);
            float maxX = Mathf.Max(patrollingPoint[0].transform.position.x, patrollingPoint[1].transform.position.x);

            destination.x = Mathf.Clamp(destination.x, minX, maxX);
            
            transform.position =
                Vector3.MoveTowards(transform.position, destination, Time.deltaTime * enemyBaseMoveSpeed);

            if (!removeChaseIdle && 
                (Mathf.Approximately(transform.position.x, minX) || Mathf.Approximately(transform.position.x, maxX)))
            {
                SetEnemyMovementType(EnemyMovementType.ChasingIdle);
            }
            
            UpdateLookDir();
        }
        else if (enemyMovementType == EnemyMovementType.ChasingIdle)
        {
            Vector3 destination = TargetedPlayer.transform.position;
            destination.y = patrollingPoint[0].transform.position.y;
            
            float minX = Mathf.Min(patrollingPoint[0].transform.position.x, patrollingPoint[1].transform.position.x);
            float maxX = Mathf.Max(patrollingPoint[0].transform.position.x, patrollingPoint[1].transform.position.x);

            destination.x = Mathf.Clamp(destination.x, minX, maxX);
            
            if (!Mathf.Approximately(destination.x, minX) && !Mathf.Approximately(destination.x, maxX))
            {
                SetEnemyMovementType(EnemyMovementType.Chasing);
            }
            
            UpdateLookDir();
        }
    }

    #endregion

    #region LookDir

    private void UpdateLookDir()
    {
        if (enemyMovementType == EnemyMovementType.WaitingToStartChase || 
            enemyMovementType == EnemyMovementType.Chasing || 
            enemyMovementType == EnemyMovementType.ChasingIdle)
        {
            localScale.x = TargetedPlayer.transform.position.x > transform.position.x ? -1 : 1;
            transform.localScale = localScale;
        }
        else
        {
            localScale.x = patrollingPoint[targetPatrolPointIdx].transform.position.x > transform.position.x ? -1 : 1;
            transform.localScale = localScale;
        }
    }

    #endregion

    #region Chasing

    internal void SetPlayerToChase(Player player)
    {
        if (player == null)
        {
            if (TargetedPlayer != null)
            {
                TargetedPlayer = null;
                SetEnemyMovementType(EnemyMovementType.Idle);
            }
        }
        else
        {
            if (TargetedPlayer == null)
            {
                TargetedPlayer = player;
                SetEnemyMovementType(EnemyMovementType.WaitingToStartChase);
            }
        }
    }

    #endregion

    #region Player

    internal void SetIdleAfterHittingPlayer()
    {
        SetEnemyMovementType(EnemyMovementType.Idle);
    }

    #endregion
    
    #region Animation

    private void UpdateAnimation(EnemyMovementType enemyMovementType)
    {
        bool isIdle = enemyMovementType == EnemyMovementType.Idle || 
                      enemyMovementType == EnemyMovementType.WaitingToStartChase ||
                      enemyMovementType == EnemyMovementType.ChasingIdle;
        
        bool isRunning = enemyMovementType == EnemyMovementType.Patrolling || 
                         enemyMovementType == EnemyMovementType.Chasing;
        
        enemy.enemyAnimator.PlayAnim(EnemyAnimationType.Idle, isIdle);
        enemy.enemyAnimator.PlayAnim(EnemyAnimationType.Running, isRunning);
    }

    #endregion
}
