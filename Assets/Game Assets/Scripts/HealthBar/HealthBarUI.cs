using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public bool playerHealth;
    public Slider damageSlider;
    public Slider healthSlider;

    //private variables
    private float shakeAmount = 1000f;
    private float waitTime = 0f;
    private float _decreaseRate = 50f;
    private float _decreaseMultipler = 1f;
    private float _increaseRate = 350f;
    private List<float> _healAmounts = new List<float>();

    private bool _shaking = false;
    private bool _damaged = false;
    private bool _healing = false;
    
    private RectTransform _rt;
    private Vector2 _originalPos;

    private DateTime _before;
    private DateTime _after;
    
    void Start()
    {
        // We will reference the RectTransform of the health bar later
        _rt = GetComponent<RectTransform>();

        // We will also record the original position of the health bar so it can return to it's original position after being shaken
        _originalPos = _rt.anchoredPosition;
    }

    private void Update()
    {
        // This will create the shaking motion the health bar will perform when damaged
        if (_shaking)
        {
            Vector2 newPos = _originalPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
            _rt.anchoredPosition = newPos;
        }
        else
        {
            // When the health bar is done shaking, then return to original position
            _rt.anchoredPosition = _originalPos;
        }

        // This will have the damage slider (the yellow one) slowly decrease to the point to where our current health is
        if (_damaged)
        {
            damageSlider.value -= (_decreaseRate * Time.deltaTime) * _decreaseMultipler;

            // The damage slider will decrease at an increasing rate, but only to the limit of 100x the original speed
            _decreaseMultipler += 10f * Time.deltaTime;
            if (_decreaseMultipler > 100)
                _decreaseMultipler = 100;

            // This will reset values for the next time we take damage when the damage slider's value is less than/equals the health slider's
            if (damageSlider.value <= healthSlider.value)
            {
                damageSlider.value = healthSlider.value;
                _decreaseMultipler = 1;
                _damaged = false;
            }
        }

        // This will heal the player by the given amount. Healing items can stack on one another!
        if (_healing)
        {
            if (_healAmounts.Count > 0)
            {
                // We iterate through the healing list to increase the player health a little bit for each healing used
                for (int i = 0; i < _healAmounts.Count; i++)
                {
                    if (_healAmounts[i] > 0)
                    {
                        healthSlider.value += _increaseRate * Time.deltaTime;
                        damageSlider.value += _increaseRate * Time.deltaTime;
                        _healAmounts[i] -= _increaseRate * Time.deltaTime;
                    }
                }

                // Here we remove the healing amounts that are 0 from the list
                List<float> newList = new List<float>();
                foreach (float a in _healAmounts)
                {
                    if (a > 0)
                        newList.Add(a);
                }
                _healAmounts = new List<float>(newList);
            }
            else
            {
                // If there is no more healing, then we will clear the healing list just in case, and flip the healing boolean
                _healAmounts.Clear();
                _healing = false;
            }
        }
    }

    // Initalize a maximum health for the health bar
    public void SetMaxHealth(int health)
    {
        if (playerHealth)
            GetComponent<RectTransform>().sizeDelta = new Vector2(health, GetComponent<RectTransform>().sizeDelta.y);
        healthSlider.maxValue = health;
        damageSlider.maxValue = health;
    }

    // Set the current health
    public void SetHealth(int health)
    {
        healthSlider.value = health;
        damageSlider.value = health;
    }

    // Decrease the current health by a given amount
    public void DecreaseHealth(int amount)
    {
        shakeAmount = Mathf.Log((float)amount) * 2.5f;
        waitTime = Mathf.Log((float)amount) / 20f;

        StartCoroutine(DamageAction());

        healthSlider.value -= amount;
    }

    // Increase the current health by a given amount
    public void IncreaseHealth(int amount)
    {
        _healing = true;

        // Here we add the amount given to a list, we have a list so that we can potentially use multiple healing items at the same time
        _healAmounts.Add(amount);
    }

    // This will set the shaking and damaged boolean to true then wait a defined amount of time before setting the shaking boolean back to false
    IEnumerator DamageAction()
    {
        if (_shaking == false)
        {
            _shaking = true;
            _damaged = true;
            yield return new WaitForSeconds(waitTime);
            _shaking = false;
        }
    }


}
