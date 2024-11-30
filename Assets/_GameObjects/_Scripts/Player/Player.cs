using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement { get; private set; }
    public PlayerAnimator playerAnimator { get; private set; }
    public PlayerCollisionDetection playerCollisionDetection { get; private set; }
    public PlayerHp playerHp { get; private set; }
    
    internal void SetUp()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerCollisionDetection = GetComponent<PlayerCollisionDetection>();
        playerHp = GetComponent<PlayerHp>();

        playerMovement.SetUp(this);
        playerCollisionDetection.SetUp(this);
        playerHp.SetUp(this);
    }
}
