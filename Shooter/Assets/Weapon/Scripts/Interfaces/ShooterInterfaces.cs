namespace ShooterInterfaces
{
    public interface IDamageable
    {
        public void TakeDamage(float damage);
        public void Die();
        public void OnGetDamage();
    }

    public interface IAttackable
    {
        public void Attack(float damage);
    }
}


