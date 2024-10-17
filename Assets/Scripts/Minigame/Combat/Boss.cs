using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Boss : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private int maxHP;
    private int health; 
    

    private void Start()
    {
        health = maxHP;
        UpdateHealthBar();
    }

    
    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float healthPercentage = (float) health / maxHP;
            healthBarFill.fillAmount = healthPercentage;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public virtual void TakeUltimateDamage()
    {
        int damageAmount = 1;
        health = Mathf.Max(health - damageAmount, 0);
        UpdateHealthBar();
    }
}