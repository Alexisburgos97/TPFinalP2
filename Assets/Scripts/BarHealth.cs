using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarHealth : MonoBehaviour
{
    public float Health = 100;
    public float HealthMax = 100;

    [Header("Interfaz")] 
    public Image ImageBarHealth;
    public Text TextHealth;

    public void TakesDamage(float damage)
    {
        Health -= damage;
        
        if (Health <= 0)
        {
            Health = 0;
            //Die();
        }

        UpdateInterfaz();
    }

    public bool recibeCure(float cure)
    {
        if (Health == HealthMax)
        {
            return false;
        }
        
        Health += cure;

        if (Health > HealthMax)
        {
            Health = HealthMax;
        }

        UpdateInterfaz();
        return true;
    }

    public void UpdateInterfaz()
    {
        ImageBarHealth.fillAmount = Health / HealthMax;
        TextHealth.text = "+ " + Health.ToString("f0");
    }
    
    private void Die()
    {
        SceneManager.LoadScene("GameOver");
    }
}