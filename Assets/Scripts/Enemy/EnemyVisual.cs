using System.Collections;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayWalk()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
    }

    public void PlayRun()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
        /*StartCoroutine(ResetAttackTrigger());*/
    }
    /*private IEnumerator ResetAttackTrigger()
    {
        yield return new WaitForSeconds(0.3f); // Adjust based on animation length
        animator.ResetTrigger("Attack");
    }*/
    public void Flip(float direction)
    {
        if (direction != 0)
        {
            transform.rotation = Quaternion.Euler(0, direction > 0 ? 0 : 180, 0);
        }
    }
}
