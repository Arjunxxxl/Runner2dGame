using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyMovement enemyMovement { get; private set; }
    public EnemyAnimator enemyAnimator { get; private set; }
    public EnemyCollisionDetector enemyCollisionDetector { get; private set; }
    public EnemyHp enemyHp { get; private set; }

    internal void SetUp(List<GameObject> patrollingPoint, Vector2 idleDurationRange, Vector2 patrolDurationRange)
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyCollisionDetector = GetComponent<EnemyCollisionDetector>();
        enemyHp = GetComponent<EnemyHp>();
        
        enemyMovement.SetUp(this, patrollingPoint, idleDurationRange, patrolDurationRange);
        enemyCollisionDetector.SetUp(this);
        enemyHp.SetUp(this);
    }
}
