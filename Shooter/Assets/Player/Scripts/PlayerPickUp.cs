using System;
using TMPro;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private KeyCode pickUpKey;
    [SerializeField] private TMP_Text itemNameLabel;
    [SerializeField] private PlayerInventory plInventory;
    public static event Action OnItemPickedUp;

   private void Awake()
    {
        itemNameLabel.enabled = false;
    }
    private void Update()
    {
        if (PlayerRayCaster.isHit && PlayerRayCaster.HIT.transform != null)
        {
            var _hit = PlayerRayCaster.HIT;
            if (_hit.transform.TryGetComponent(out PhysicalItem phItem))
            {
                itemNameLabel.enabled = true;
                itemNameLabel.text = phItem.item.itemName.ToString();

                if (Input.GetKeyDown(pickUpKey))
                {
                    if (plInventory.AddItem(phItem.item))
                    {
                        OnItemPickedUp?.Invoke();
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
        else
        {
            itemNameLabel.text = "";
            itemNameLabel.enabled = false;
        }
    }
}
