using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int maxHealth = 100;
    public int baseHealth = 100;
    public int currentHealth;
    public HealthBarUI healthBarUI;
    public bool player;
    
    void Start()
    {
        // Initalize health bar with player health information
        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
        healthBarUI.SetHealth(maxHealth);

        if (player)
            SetMaxHealth(baseHealth + 15 * (int)GetComponent<PlayerStat>().vitality.Value);
    }
    void Update()
    {
        currentHealth = (int)healthBarUI.healthSlider.value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
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
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        healthBarUI.SetMaxHealth(maxHealth);
        float difference = Mathf.Abs(currentHealth - maxHealth * (currentHealth / maxHealth));
        Healing((int)difference);
    }
}
