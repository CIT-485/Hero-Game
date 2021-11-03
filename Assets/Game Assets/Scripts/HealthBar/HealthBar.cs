using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBarUI healthBarUI;
    
    void Start()
    {
        // Initalize health bar with player health information
        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
        healthBarUI.SetHealth(maxHealth);
    }
    void Update()
    {
        currentHealth = (int)healthBarUI.healthSlider.value;
    }

    // Function to simulate take damage
    public void TakeDamage(int amount)
    {
        healthBarUI.DecreaseHealth(amount);
    }

    // Function to simulate healing
    public void Healing(int amount)
    {
        healthBarUI.IncreaseHealth(amount);
    }
}
