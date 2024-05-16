using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Shooter.Enemy;

namespace Shooter.Player
{
    public class PlayerHealth : MonoBehaviour, IDamagable
    {
        [SerializeField] private float maxHealth = 100;
        private float currentHealth;
        public float CurrentHealth
        {
            get => currentHealth;
            set
            {            
                currentHealth = Math.Clamp(value, 0, maxHealth);
                OnPlayerHealthChanged?.Invoke();
            }
        }
        public static Action OnPlayerHealthChanged;
        [SerializeField] private PlayerController controller;

        private void Start()
        {
            CurrentHealth = maxHealth;
        }
        public void TakeDamage(int damage)
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

        private static void OnGetDamage()
        {
            print("Auch");
        }
    }
}
