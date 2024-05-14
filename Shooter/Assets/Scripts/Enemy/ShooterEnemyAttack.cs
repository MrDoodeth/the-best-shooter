using UnityEngine;

namespace Shooter.Enemy
{
    [RequireComponent(typeof(Enemy))]
    public class ShooterEnemyAttack : EnemyAttack
    {
        [SerializeField] private FireBall fireObject;
        [SerializeField] private float throwForce;
        private FireBall currentFireObject;
        private Rigidbody fireBallRigidbody;

        private void Start()
        {
            currentFireObject = Instantiate(fireObject, attackPoint.position, Quaternion.identity);
            currentFireObject.transform.SetParent(this.transform);
            fireBallRigidbody = currentFireObject.GetComponent<Rigidbody>();
            currentFireObject.gameObject.SetActive(false);
            fireBallRigidbody.isKinematic = true;
            currentFireObject.damage = damage;
        }

        private void Attack()
        {
            fireBallRigidbody.isKinematic = false;
            currentFireObject.transform.position = attackPoint.position;
            currentFireObject.gameObject.SetActive(true);
            fireBallRigidbody.AddForce(attackPoint.forward * throwForce, ForceMode.Impulse);
            
            Debug.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * throwForce, Color.red, 1f);
        }
    }
}
