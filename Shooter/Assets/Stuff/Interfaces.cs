using UnityEngine;

public interface IDamagable
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
    public int price { get; set; }
    public int Buy(int balance);
}

public interface IOpenable
{
    public void Open();
}

public interface ITradable
{
    public Item neededItem { get; set; }
    public int neededItemsAmount { get; set; }
    public void Trade(PlayerInventory plInv);
}