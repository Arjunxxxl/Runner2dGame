using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("Animator")] 
    [SerializeField] private Animator anim;

    internal void PlayAnim(EnemyAnimationType enemyAnimationType, bool play)
    {
        anim.SetBool(enemyAnimationType.ToString(), play);
    }
}
