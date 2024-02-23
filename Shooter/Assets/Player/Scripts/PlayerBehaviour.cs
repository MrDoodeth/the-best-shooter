using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100;
    private float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        private set
        {            
            currentHealth = Math.Clamp(value, 0, int.MaxValue);
            OnPlayerHealthChanged?.Invoke();
        }
    }
    public static Action OnPlayerHealthChanged;
    [SerializeField] private PlayerController controller;

    private void Start()
    {
        CurrentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnGetDamage();
        }
    }
    public void Die()
    {
        Debug.Log("GAME OVER");
        controller.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnGetDamage()
    {
        Debug.Log("Auch");
    }
}
