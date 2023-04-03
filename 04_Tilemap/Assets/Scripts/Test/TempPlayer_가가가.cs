using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempPlayer_가가가 : MonoBehaviour
{
    public float health = 50;    
    public float maxHealth = 100;

    TextMeshProUGUI healthText = null;
    Slider healthSlider = null;
    Button healthUp = null;
    Button healthDown = null;

    public float Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);

            healthText.text = $"{health:F0}/{(int)maxHealth}";
            healthSlider.value = health/maxHealth;
        }
    }

    private void Awake()
    {

    }

    private void Start()
    {
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        healthUp = GameObject.Find("HealthUp").GetComponent<Button>();
        healthDown = GameObject.Find("HealthDown").GetComponent<Button>();

        if (healthUp != null)
        {
            healthUp.onClick.AddListener(UpClick);
            Text text = healthUp.GetComponentInChildren<Text>();
            text.text = "UP";
        }
        if (healthDown != null)
        {
            healthDown.onClick.AddListener(DownClick);
            Text text = healthDown.GetComponentInChildren<Text>();
            text.text = "Down";
        }

        Health = health;
    }

    private void DownClick()
    {
        Health -= maxHealth * 0.1f;
    }

    private void UpClick()
    {
        Health += maxHealth * 0.1f;
    }
}
