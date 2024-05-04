namespace Shooter.Enemy
{
    public interface IDamagable
    {
        public void TakeDamage(int damage);
        public void Die();
    }
    
    public interface IContusionable
    {
        public void GetContusion();
    }
}
