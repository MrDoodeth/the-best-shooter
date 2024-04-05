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
    /// <summary>
    /// Trade to swap items
    /// </summary>
    /// <param name="items">Player inventory</param>
    /// <param name="itemsAmounts">Amounts of items</param>
    public void Trade(PlayerInventory plInv);
}