using Shooter.Player;
using Shooter.Weapon;
using UnityEngine;

namespace Shooter.Interactable.Vendor
{
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
        public int Buy(int balance)
        {
            if (balance < price)
            {
                Debug.Log("Недостаточно средств!");
                return 0;
            }

            //Usefull action
            SpawnItem();     
        
            return price;
        }

        private void SpawnItem()
        {
            var _item = Instantiate(item);
            _item.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            _item.GetComponent<Rigidbody>().isKinematic = false;
            
            PlayerScore.OnChangePointsAmount?.Invoke();
            Debug.Log($"Вы купили {item.GetComponent<Gun>().Name} за {price} ");
        }
    }
}
