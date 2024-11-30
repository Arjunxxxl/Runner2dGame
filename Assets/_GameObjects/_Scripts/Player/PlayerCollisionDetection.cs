using System;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    private Player player;

    internal void SetUp(Player player)
    {
        this.player = player;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            GameObject obj = other.gameObject;
            Enemy enemy = obj.GetComponent<Enemy>();
            Spike spike = obj.GetComponent<Spike>();

            int hitObjLayer = obj.layer;

            if (hitObjLayer == Constants.LevelCompleteObjLayer)
            {
                GameManager.Instance.SetGameState(GameState.GameComplete);
                return;
            }
            
            if (enemy != null)
            {
                Vector3 enemyPos = enemy.transform.position;
                Vector3 playerPos = transform.position;

                float yOffset = playerPos.y - enemyPos.y;
                
                if (yOffset >= Constants.PlayerYOffsetForEnemyKill)
                {
                    player.playerMovement.JumpOnEnemyKill();
                    enemy.enemyHp.TakeDamage();
                    CameraController.ShakeCameraOnEnemyTaken?.Invoke();
                }
                else
                {
                    enemy.enemyMovement.SetIdleAfterHittingPlayer();
                    player.playerHp.TakeDamage();
                }
            }
            else if (spike != null)
            {
                player.playerHp.TakeDamage();
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other != null)
        {
            GameObject obj = other.gameObject;
            Enemy enemy = obj.GetComponent<Enemy>();
            Spike spike = obj.GetComponent<Spike>();

            if (enemy != null)
            {
                Vector3 enemyPos = enemy.transform.position;
                Vector3 playerPos = transform.position;

                float yOffset = playerPos.y - enemyPos.y;
                
                if (yOffset >= Constants.PlayerYOffsetForEnemyKill)
                {
                    //Do nothing
                }
                else
                {
                    enemy.enemyMovement.SetIdleAfterHittingPlayer();
                    player.playerHp.TakeDamage();
                }
            }
            else if (spike != null)
            {
                player.playerHp.TakeDamage();
            }
        }
    }
}