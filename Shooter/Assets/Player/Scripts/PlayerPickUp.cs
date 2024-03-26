using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameLabel;
    [SerializeField] private KeyCode pickUpKey;
    [SerializeField] private PlayerInventory plInventory;

   private void Awake()
    {
        itemNameLabel.enabled = false;
    }
    private void Update()
    {
        if (PlayerRayCaster.isHit)
        {
            var _hit = PlayerRayCaster.HIT;

            PhysicalItem phItem = _hit.transform.GetComponent<PhysicalItem>();
            itemNameLabel.enabled = true;
            itemNameLabel.text = phItem?.item.itemName.ToString();

            if (Input.GetKeyDown(pickUpKey) && phItem is not null)
            {
                if (plInventory.AddItem(phItem.item))
                {
                    //OnItemPickedUp?.Invoke();
                    itemNameLabel.enabled = false;
                    phItem.gameObject.SetActive(false);
                    Destroy(phItem.gameObject);
                }
            }
        }
        else
        {
            itemNameLabel.text = "";
            itemNameLabel.enabled = false;
        }
    }
}
