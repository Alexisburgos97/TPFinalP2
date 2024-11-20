using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float currentHealth = 100;
    private float maxHealth = 100;

    [Header("HUD Elements")]
    [SerializeField]public BarHealth barHealthHud;


    /*GETTERS*/
    public float get_currentHealth() { return currentHealth; }
    public float get_maxHealth() { return maxHealth; }

    /*METODOS*/
    public bool TakesDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        barHealthHud.UpdateInterfaz(currentHealth, maxHealth);
        Debug.Log($"Player health: {currentHealth}");
        
        return IsDead();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        barHealthHud.UpdateInterfaz(this.currentHealth, this.maxHealth);
        Debug.Log($"Player healed: {currentHealth}");
    }

    private bool IsDead()
    {
        return currentHealth <= 0;
    }
}
