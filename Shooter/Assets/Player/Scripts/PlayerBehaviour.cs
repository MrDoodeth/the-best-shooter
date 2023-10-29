using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShooterInterfaces;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;


    [SerializeField] private PlayerController controller;

    //[SerializeField] private int score;
    //public int Score 
    //{ 
    //    get { return score; }
    //    set 
    //    {
    //        if (value >= 0)
    //            score = value; 
    //        else
    //            score = 0;
    //    }
    //}

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();

            void Die()
            {
                Debug.Log("GAME OVER");
                controller.enabled = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //Destroy(gameObject);
            }
        }
        else
        {
            OnTargetDamage();

            void OnTargetDamage()
            {
                Debug.Log("Auch");
            }
        }
    }
}
