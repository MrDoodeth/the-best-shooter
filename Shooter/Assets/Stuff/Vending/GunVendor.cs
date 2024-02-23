using UnityEngine;

public class GunVendor : MonoBehaviour, IBuyable
{
    [SerializeField] private GameObject item;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int Price;
    public int price
    {
        get => Price;
        set { Price = value <= 0 ? 0 : value; }
    }
    public void Buy(ref int balance)
    {
        if (balance < price)
        {
            Debug.Log("Недостаточно средств!");
            return;
        }

        balance -= price;
        Debug.Log($"Вы купили {item.GetComponent<Gun>().Name} за {price} ");
        SpawnItem();          
    }

    private void SpawnItem()
    {
        var _item = Instantiate(item, spawnPoint.position, Quaternion.identity);
        _item.GetComponent<Rigidbody>().isKinematic = false;
    }
}
