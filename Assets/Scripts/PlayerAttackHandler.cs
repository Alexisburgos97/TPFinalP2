using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    //ANIMATIONS
    private Animator animator;

    //ATTACK SOUNDS
    private ISoundController _SoundControl;
    private AudioClip attkSound1;
    private AudioClip attkSound2;

    //ATTACK STATS
    private float attackDamage = 15f;
    private float attackRange = 1.6f;
    private float strongAttackRange = 2.1f;
    private float strongAttackDamage = 30f;

    //PLAYER POSITION
    private Transform playerTransform;

    // Start is called before the first frame update
    public PlayerAttackHandler(Animator animator, ISoundController soundController, AudioClip attkSound1, AudioClip attkSound2, 
                               float attackDamage, float attackRange, float strongAttackRange, float strongAttackDamage, Transform playerTransform)
    {
        this.animator = animator;
        this._SoundControl = soundController;
        this.attkSound1 = attkSound1;
        this.attkSound2 = attkSound2;
        this.attackRange = attackRange;
        this.attackDamage = attackDamage;
        this.strongAttackRange = strongAttackRange;
        this.strongAttackDamage = strongAttackDamage;
        this.playerTransform = playerTransform;
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

        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, range);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();

            if (enemy != null && enemy.estaVivo)
            {
                Debug.DrawRay(playerTransform.position, (enemy.transform.position - playerTransform.position).normalized * attackRange, Color.red);

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
