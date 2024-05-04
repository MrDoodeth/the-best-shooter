using System.Collections;
using Shooter.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shooter.Enemy
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyHealth : MonoBehaviour, IDamagable, IContusionable
    {
        private Enemy enemy;
        private Transform hipsBone;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            hipsBone = transform.GetChild(0);
        }

        public void TakeDamage(int damage)
        {
            if (!enemy.isAlive) return;

            enemy.currentHealth -= damage;

            if (enemy.currentHealth <= 0)
            {
                Die();
            }
            else
            {
                //OnGetDamage();
            }
        }
        public void Die()
        {
            StopAllCoroutines();
            StartCoroutine(Death());
            return;

            IEnumerator Death()
            {
                enemy.isAlive = false;
                EnemiesSpawner.OnEnemyDeath?.Invoke();
                PlayerScore.OnGetReward?.Invoke(enemy.rewardAmount);
                enemy.agent.enabled = false;
                enemy.allRigidbodys.SetIsKinematics(false);
                enemy.animator.enabled = false;
                yield return new WaitForSeconds(enemy.deathDelay);
                Destroy(gameObject);
            }
        }  

        private void OnGetDamage()
        {
            StopAllCoroutines();
            StartCoroutine(Damage());
            return;

            IEnumerator Damage()
            {
                print("Hit");
                enemy.agent.isStopped = true;
                enemy.animator.SetTrigger("isHit");
                enemy.animator.SetBool("isMirror", Random.value > 0.5f);
                yield return new WaitForSeconds(enemy.animator.GetCurrentAnimatorStateInfo(0).length + enemy.animator.GetAnimatorTransitionInfo(0).duration);
                enemy.agent.isStopped = false;
            }   
        }   
        public void GetContusion()   
        {   
            StopAllCoroutines();
            StartCoroutine(Contusion());
            return;

            IEnumerator Contusion()
            {
                enemy.isAlive = false;
                enemy.agent.isStopped = true;
                enemy.animator.enabled = false;
                enemy.allRigidbodys.SetIsKinematics(false);
            
                yield return new WaitForSeconds(enemy.contusionDelay);
                yield return new WaitUntil(() => enemy.agent.isOnNavMesh);

                //return rigs in world position
                transform.position = hipsBone.position;
                hipsBone.localPosition = Vector3.zero;

                enemy.allRigidbodys.SetIsKinematics(true);
                enemy.animator.enabled = true;
                enemy.animator.SetTrigger("isStandUp");
                yield return new WaitForSeconds(enemy.animator.GetCurrentAnimatorStateInfo(0).length + enemy.animator.GetAnimatorTransitionInfo(0).duration);
                enemy.isAlive = true;
                enemy.agent.isStopped = false;
                yield return null;
            }
        }
    }
}
