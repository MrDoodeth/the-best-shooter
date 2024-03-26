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

            if (_hit.transform.CompareTag("Item"))
            {
                Item _item = _hit.transform.GetComponent<Item>();
                itemNameLabel.enabled = true;
                itemNameLabel.text = _item?.itemName.ToString();

                if (Input.GetKeyDown(pickUpKey))
                {
                    plInventory.AddItem(_item);
                    //OnItemPickedUp?.Invoke();
                    itemNameLabel.enabled = false;
                    _hit.transform.gameObject?.SetActive(false);
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
