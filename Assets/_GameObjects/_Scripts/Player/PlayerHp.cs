using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [Header("Hp Data")] 
    [SerializeField] private int hpLeft;

    [Header("Inverenuable Data")] 
    [SerializeField] private float inverenuableDuration;
    private float inverenublityTimeElapsed;
    private bool isInvenruable;

    [Header("Testin")] 
    [SerializeField] private bool stopDmgTaking;

    private Player player;
    
    // Update is called once per frame
    void Update()
    {
        UpdateInvenrublityTimer();
        CheckIfPlayerFallThroughLevel();
    }

    #region SetUp

    internal void SetUp(Player player)
    {
        this.player = player;
        
        hpLeft = Constants.MaxPlayerHp;

        GameplayMenu.UpdateHpUi(hpLeft);
    }

    #endregion
    
    #region Damage
    
    internal void TakeDamage(bool isFallThoughLevel = false)
    {
        if (isInvenruable)
        {
            return;
        }

        if (!stopDmgTaking)
        {
            hpLeft--;
        }

        if (isFallThoughLevel)
        {
            hpLeft = 0;
        }

        GameplayMenu.UpdateHpUi(hpLeft);
        
        if (hpLeft <= 0)
        {
            GameObject enemyKillEfxObj =
                ObjectPooler.Instance.SpawnFormPool("EnemyKillEfx", transform.position, Quaternion.identity);
            EnemyDeathEfx enemyDeathEfx = enemyKillEfxObj.GetComponent<EnemyDeathEfx>();
            enemyDeathEfx.SetUp();
         
            GameManager.Instance.SetGameState(GameState.WaitingForGameOver);
            
            SoundManager.Instance.PlayAudio("player death");
            
            gameObject.SetActive(false);
            
            return;
        }
        
        isInvenruable = true;
        inverenublityTimeElapsed = 0;
        
        player.playerMovement.JumpOnDmgTaken();
        
        CameraController.ShakeCameraOnDamageTaken?.Invoke();

        SoundManager.Instance.PlayAudio("hit");
    }

    private void UpdateInvenrublityTimer()
    {
        if (!isInvenruable)
        {
            return;
        }
        
        inverenublityTimeElapsed += Time.deltaTime;

        if (inverenublityTimeElapsed >= inverenuableDuration)
        {
            isInvenruable = false;
            inverenublityTimeElapsed = 0;
            
            player.playerMovement.SetHurtAnimation(false);
        }
    }

    private void CheckIfPlayerFallThroughLevel()
    {
        if (transform.position.y < Constants.PlayerYPosForFallDeath)
        {
            TakeDamage(true);
        }
    }
    
    #endregion

    #region Getter/Setter

    internal bool IsPlayerInventuable()
    {
        return isInvenruable;
    }

    #endregion
}
