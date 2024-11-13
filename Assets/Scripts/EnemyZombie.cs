using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyZombie : EnemyController
{
    private NavMeshAgent agente;
    public Animator animaciones;
    public float daño = 5f;

    void Awake()
    {
        base.Awake();
        agente = GetComponent<NavMeshAgent>();
        
    }

    public override void EstadoIdle()
    {
        base.EstadoIdle();
        animaciones.SetFloat("WalkSpeedEnemy", 0);
        animaciones.SetBool("AttackEnemy", false);
        
        agente.SetDestination(transform.position);
    }

    public override void EstadoSeguir()
    {
        base.EstadoSeguir();
        animaciones.SetFloat("WalkSpeedEnemy", 1);
        animaciones.SetBool("AttackEnemy", false);
        
        agente.SetDestination(target.position);
    }

    public override void EstadoAtacar()
    {
        base.EstadoAtacar();
        animaciones.SetFloat("WalkSpeedEnemy", 0);
        animaciones.SetBool("AttackEnemy", true);
        
        agente.SetDestination(transform.position);
        transform.LookAt(target, Vector3.up);
    }

    public override void EstadoMuerto()
    {
        base.EstadoMuerto();
        animaciones.SetTrigger("Death");

        // Desactiva el agente para que no se mueva después de morir
        agente.enabled = false;
        Destroy(gameObject, 3f); //2f
    }

    public void Atacar()
    {
        if (distancia <= distanciaAtacar) 
        {
            if (PlayerController.PlayerSingleton != null && PlayerController.PlayerSingleton.barHealth != null)
            {
                PlayerController.PlayerSingleton.barHealth.TakesDamage(daño);
            }
            else
            {
                Debug.LogWarning("PlayerController o barHealth no están asignados.");
            }
        }
    }
}
