using System.Collections;
using UnityEngine;

public class EnemyDeathEfx : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator anim;
    
    [Header("Disable Delay")]
    [SerializeField] private float disableDelay;

    private static readonly int PlayEfx = Animator.StringToHash("PlayEfx");

    internal void SetUp()
    {
        anim.SetTrigger(PlayEfx);
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }
}
