using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyZombieBoss : EnemyController
{
    private NavMeshAgent agente;
    public Animator animaciones;
    private bool OneTimeExec = false;

    void Awake()
    {
        base.Awake();
        agente = GetComponent<NavMeshAgent>();
    }

    public override void EstadoIdle()
    {
        base.EstadoIdle();
        animaciones.SetFloat("WalkSpeedEnemyBoss", 0);
        animaciones.SetBool("AttackEnemyBoss", false);
        
        agente.SetDestination(transform.position);
    }

    public override void EstadoSeguir()
    {
        base.EstadoSeguir();
        animaciones.SetFloat("WalkSpeedEnemyBoss", 1);
        animaciones.SetBool("AttackEnemyBoss", false);
        
        agente.SetDestination(target.position);
    }

    public override void EstadoAtacar()
    {
        base.EstadoAtacar();
        animaciones.SetFloat("WalkSpeedEnemyBoss", 0);
        animaciones.SetBool("AttackEnemyBoss", true);
        
        agente.SetDestination(transform.position);
        transform.LookAt(target, Vector3.up);
    }

    public override void EstadoMuerto()
    {
        base.EstadoMuerto();

        animaciones.SetTrigger("DeathEnemyBoss");
        agente.enabled = false;
        
        if (OneTimeExec == false)
        {
            OneTimeExec = true;
            Invoke(nameof(moriteBicho), 3f);
            Destroy(gameObject, 3.5f);
        }
    }

    private void moriteBicho()
    {
        GameManager.Instance.LoadLevel("VictoryScreen");
    }
}