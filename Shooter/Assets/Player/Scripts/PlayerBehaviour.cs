using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;


    [SerializeField] private PlayerController controller;

    [SerializeField] private int score;
    public int Score 
    { 
        get { return score; }
        set 
        {
            Mathf.Clamp(score, 0, int.MaxValue);
        }
    }

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
