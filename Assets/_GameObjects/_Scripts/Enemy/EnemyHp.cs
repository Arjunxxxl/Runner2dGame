using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int hpLeft;

    private Enemy enemy;
    
    internal void SetUp(Enemy enemy)
    {
        this.enemy = enemy;
        hpLeft = maxHp;
    }

    internal void TakeDamage()
    {
        hpLeft--;

        if (hpLeft <= 0)
        {
            GameObject enemyKillEfxObj =
                ObjectPooler.Instance.SpawnFormPool("EnemyKillEfx", transform.position, Quaternion.identity);
            EnemyDeathEfx enemyDeathEfx = enemyKillEfxObj.GetComponent<EnemyDeathEfx>();
            enemyDeathEfx.SetUp();
            
            SoundManager.Instance.PlayAudio("enemy death");
            
            gameObject.SetActive(false);
        }
    }
}
