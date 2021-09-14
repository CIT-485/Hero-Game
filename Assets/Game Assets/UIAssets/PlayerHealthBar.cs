using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public PlayerHealthBarUI playerHealthBarUI;
    
    void Start()
    {
        // This will find the player health bar by finding the object with the tag
        playerHealthBarUI = GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<PlayerHealthBarUI>();

        // Initalize health bar with player health information
        currentHealth = maxHealth;
        playerHealthBarUI.SetMaxHealth(maxHealth);
        playerHealthBarUI.SetHealth(maxHealth);
    }
    void Update()
    {
        currentHealth = (int) playerHealthBarUI.healthSlider.value;
    }

    // Function to simulate take damage
    public void TakeDamage(int amount)
    {
        playerHealthBarUI.DecreaseHealth(amount);
    }

    // Function to simulate healing
    public void Healing(int amount)
    {
        playerHealthBarUI.IncreaseHealth(amount);
    }
}
