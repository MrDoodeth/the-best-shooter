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
        public virtual void Die()
        {
            StartCoroutine(Death());

            IEnumerator Death()
            {
                if (Random.value <= enemy.buffsSpawnProbability)
                {
                    int rundomInt = Random.Range(0, enemy.buffs.Length);
                    var buff = enemy.buffs[rundomInt];

                    Instantiate(buff, transform.position, Quaternion.identity);
                }
                
                enemy.isAlive = false;
                enemy.animator.enabled = false;
                enemy.agent.enabled = false;
                EnemiesSpawner.OnEnemyDeath?.Invoke(enemy);
                PlayerScore.OnGetReward?.Invoke(enemy.rewardAmount);
                enemy.allRigidbodys.SetIsKinematics(false);
                yield return new WaitForSeconds(enemy.deathDelay);
                Destroy(gameObject);
            }
        }  

        private void OnGetDamage()
        {
            StartCoroutine(Damage());

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
            //StartCoroutine(Contusion());

            IEnumerator Contusion()
            {
                enemy.isAlive = false;
                enemy.agent.enabled = false;
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
                enemy.agent.enabled = true;
            }
        }
    }
}
