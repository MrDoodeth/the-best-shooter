using UnityEngine;

namespace Shooter.Enemy
{
    [RequireComponent(typeof(Enemy))]
    public class ShooterEnemyAttack : EnemyAttack
    {
        [SerializeField] private GameObject fireObject;
        [SerializeField] private float throwForce;
        private GameObject currentFireObject;
        private Rigidbody fireBallRigidbody;

        private void Start()
        {
            currentFireObject = Instantiate(fireObject, attackPoint.position, Quaternion.identity);
            fireBallRigidbody = currentFireObject.GetComponent<Rigidbody>();
            currentFireObject.SetActive(false);
            fireBallRigidbody.isKinematic = true;
            currentFireObject.GetComponent<FireBall>().damage = damage;
        }

        private void Attack()
        {
            fireBallRigidbody.isKinematic = false;
            currentFireObject.transform.position = attackPoint.position;
            currentFireObject.SetActive(true);
            fireBallRigidbody.AddForce(attackPoint.forward * throwForce, ForceMode.Impulse);
            
            Debug.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * throwForce, Color.red, 1f);
        }
    }
}
