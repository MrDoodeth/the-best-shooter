using Shooter.Player;

namespace Shooter.Interactable
{
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
        public Item.Item neededItem { get; set; }
        public int neededItemsAmount { get; set; }
        public void Trade(PlayerInventory plInv);
    }
}