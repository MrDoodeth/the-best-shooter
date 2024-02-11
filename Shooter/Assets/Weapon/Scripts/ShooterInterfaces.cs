public interface IDamageable
{
    public void TakeDamage(float damage);
    public void Die();
}

public interface IAttackable
{
    public void Attack();
}

public interface IContusionable
{
    public void GetContusion();
}