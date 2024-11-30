using UnityEngine;

public class EnemyCollisionDetector : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private float playerDetectionRange;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Player detectedPlayer;
    
    private Enemy enemy;

    internal void SetUp(Enemy enemy)
    {
        this.enemy = enemy;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            return;
        }
        
        DoSphereCastToDetectPlayer();
    }

    #region Player Detection

    private void DoSphereCastToDetectPlayer()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, playerDetectionRange, playerLayerMask);

        if (detectedColliders != null)
        {
            detectedPlayer = null;

            for (int i = 0; i < detectedColliders.Length; i++)
            {
                if (detectedColliders[i].gameObject.GetComponent<Player>() != null)
                {
                    detectedPlayer = detectedColliders[i].gameObject.GetComponent<Player>();
                }
            }

            enemy.enemyMovement.SetPlayerToChase(detectedPlayer);
        }
    }

    #endregion
    
    #region Gizmo

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, playerDetectionRange);
    }

    #endregion
}
