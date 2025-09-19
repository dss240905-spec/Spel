using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private int attackDamage = 1;

    private bool isAttacking;
    public bool IsAttacking => isAttacking;





    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Start spinning when button is held
        if ( Input.GetMouseButton(0))
        {
            if (!isAttacking)
            {
                StartAttack();
            }
        }
        else
        {
            if (isAttacking)
            {
                StopAttack();
            }
        }
    }
  


    private void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("Attack", true);
        attackHitbox.SetActive(true);

        attackHitbox.GetComponent<DamageEnemy>().SetDamage(attackDamage);
    }

    private void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
        attackHitbox.SetActive(false);
    }
}

