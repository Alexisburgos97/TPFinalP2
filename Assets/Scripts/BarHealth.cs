using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CodHealth : MonoBehaviour
{

    public float Health = 100;
    public float HealthMax = 100;

    [Header("Interfaz")] 
    public Image BarHealth;
    public Text TextHealth;

    void Update()
    {
        UpdateInterfaz();
    }

    public void TakesDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }

    public bool recibeCure(float cure)
    {

        if (Health == 100f)
        {
            return false;
        }
        
        Health += cure;

        if (Health > HealthMax)
        {
            Health = HealthMax;
        }

        return true;
    }

    public void UpdateInterfaz()
    {
        BarHealth.fillAmount = Health / HealthMax;
        TextHealth.text = "+ " + Health.ToString("f0");
    }
    
    private void Die()
    {
        SceneManager.LoadScene("GameOver");
    }
    
}