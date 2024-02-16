using UnityEngine;
[RequireComponent(typeof(ExplosiveComponent), typeof(Enemy))]
public class ExplosiveEnemyAttack : MonoBehaviour
{
    private Enemy enemy;
    private ExplosiveEnemyHealth exHealth;
    private bool isRun;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        exHealth = GetComponent<ExplosiveEnemyHealth>();
    }
    public void Attack()
    {
        if (enemy.isAlive)
            exHealth.Die();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
            Attack();
    }
    private void ChasePlayer()
    {
        if (enemy.agent.isOnNavMesh && !enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
            enemy.agent.SetDestination(enemy.playerPosition.position);
        isRun = true;
    }
    void Update()
    {
        ChasePlayer();

        UpdateAnimations();
    }
    private void UpdateAnimations()
    {
        enemy.animator.SetBool("isRunning", isRun);
    }
}
