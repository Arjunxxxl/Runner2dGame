using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animator")] 
    [SerializeField] private Animator anim;

    internal void PlayAnim(PlayerAnimationType playerAnimationType, bool play)
    {
        anim.SetBool(playerAnimationType.ToString(), play);
    }
    
    internal void TriggerAnim(PlayerAnimationType playerAnimationType)
    {
        anim.SetTrigger(playerAnimationType.ToString());
    }
}
