using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage);
    public void Die();
}

public interface IContusionable
{
    public void GetContusion();
}

public interface IBuyable
{
    public int price { get; }
    public void Buy(ref int balance);
}