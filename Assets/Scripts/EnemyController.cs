using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum Estado
{
    idle = 0,
    seguir = 1,
    atacando = 2,
    muerto = 4
}

public class EnemyController : MonoBehaviour
{
    
    public Estado estado;
    public float distanciaSeguir;
    public float distanciaAtacar;
    public float distanciaEscapar;
    public bool autoSeleccionarTarget = true;
    public Transform target;
    public float distancia;

    public float vida = 100f; 
    public bool estaVivo = true;

    public void Awake()
    {
        StartCoroutine(CalcularDistancia());
    }

    private void Start()
    {
        if (autoSeleccionarTarget)
        {
            // target = GameObject.FindGameObjectWithTag("Player").transform;
            target = PlayerController.singleton.transform;
        }
        
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        CheckEstado();
    }

    public void CheckEstado()
    {

        switch (estado)
        {
            case Estado.idle:
                EstadoIdle();
                break;
            case Estado.seguir:
                transform.LookAt(target, Vector3.up);
                EstadoSeguir();
                break;
            case Estado.atacando:
                EstadoAtacar();
                break;
            case Estado.muerto:
                EstadoMuerto();
                break;
        }
        
    }

    public void CambiarEstado(Estado e)
    {
        switch (e)
        {
            case Estado.idle:
                break;
            case Estado.seguir:
                break;
            case Estado.atacando:
                break;
            case Estado.muerto:
                estaVivo = false;
                break;
        }
        
        estado = e;
    }

    public virtual void EstadoIdle()
    {
        if (distancia < distanciaSeguir)
        {
            CambiarEstado(Estado.seguir);
        }
    }

    public virtual void EstadoSeguir()
    {
        if (distancia < distanciaAtacar)
        {
            CambiarEstado(Estado.atacando);
        }else if (distancia > distanciaEscapar)
        {
            CambiarEstado(Estado.idle);
        }
    }
    
    public virtual void EstadoAtacar()
    {
        if (distancia > distanciaAtacar + 0.4f)
        {
            CambiarEstado(Estado.seguir);
        }
    }
    
    public virtual void EstadoMuerto()
    {
        estaVivo = false;
        Debug.Log("El enemigo ha muerto");
    }

    IEnumerator CalcularDistancia()
    {
        while (estaVivo)
        {
            yield return new WaitForSeconds(0.2f);

            if (target != null)
            {
                distancia = Vector3.Distance(transform.position, target.position);
            }
        }
    }

    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up,  distanciaAtacar);
            
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, Vector3.up, distanciaSeguir);
            
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, distanciaEscapar);
        }
    #endif
    
    public void RecibirDaño(float daño)
    {
        vida -= daño;

        Debug.Log("vida: " + vida);
        
        if (vida <= 0 && estaVivo)
        {
            vida = 0;
            CambiarEstado(Estado.muerto);
            EstadoMuerto();
        }
    }
}