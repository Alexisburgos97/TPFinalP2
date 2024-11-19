using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    //ANIMATIONS
    private Animator animator;

    //ATTACK SOUNDS
    private ISoundController _SoundControl;

    [Header("Player Attack Stats")]
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float strongAttackRange = 2.1f;
    [SerializeField] private float strongAttackDamage = 30f;

    [Header("Sounds")]
    [SerializeField] private AudioClip attkSound1;
    [SerializeField] private AudioClip attkSound2;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        _SoundControl = GetComponent<ISoundController>();

        Application.targetFrameRate = 60;
    }
    public void HandleAttack()
    {

        if (IsAttacking()) return;

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            _SoundControl.PlaySound(attkSound1);
            AttemptAttack(attackDamage, attackRange);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Attack2");
            _SoundControl.PlaySound(attkSound2);
            AttemptAttack(strongAttackDamage, strongAttackRange);
        }
    }

    public void AttemptAttack(float damage, float range)
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();

            if (enemy != null && enemy.estaVivo)
            {
                Debug.DrawRay(transform.position, (enemy.transform.position - transform.position).normalized * attackRange, Color.red);

                enemy.TakesDamage(damage);
            }
        }
    }

    private bool IsAttacking()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        return currentState.IsName("Attack") || currentState.IsName("Attack2");
    }
}
