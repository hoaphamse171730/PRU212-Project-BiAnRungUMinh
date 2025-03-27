using UnityEngine;

public class BossVisual : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayWalk()
    {
        animator.SetBool("isWalking", true);
    }
    public void StopWalk()
    {
        animator.SetBool("isWalking", false);
    }
    public void PlayAttack()
    {
        StopWalk();
        animator.SetTrigger("Attack");
    }
    public void Flip(float direction)
    {
        if (direction != 0)
        {
            transform.rotation = Quaternion.Euler(0, direction > 0 ? 0 : 180, 0);
        }
    }
}
