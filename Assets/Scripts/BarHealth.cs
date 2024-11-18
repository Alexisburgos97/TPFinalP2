using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarHealth : MonoBehaviour
{

    [Header("Interfaz")] 
    public Image ImageBarHealth;
    public Text TextHealth;
    
    public void UpdateInterfaz(float currentHealth, float MaxHealth)
    {
        ImageBarHealth.fillAmount = currentHealth / MaxHealth;
        TextHealth.text = "+ " + currentHealth.ToString("f0");
    }
}