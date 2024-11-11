using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyZombieBoss : EnemyController
{
    private NavMeshAgent agente;
    public Animator animaciones;
    
    public float daño = 10f;

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
        
        //animaciones.SetBool("DeathEnemyBoss", true);
        
        // Desactiva el agente para que no se mueva después de morir
        agente.enabled = false;
        
        Destroy(gameObject, 2f); 
    }

    public void Atacar()
    {
        if (distancia <= distanciaAtacar) 
        {
            if (PlayerController.singleton != null && PlayerController.singleton.barHealth != null)
            {
                PlayerController.singleton.barHealth.TakesDamage(daño);
            }
            else
            {
                Debug.LogWarning("PlayerController o barHealth no están asignados.");
            }
        }
    }
}