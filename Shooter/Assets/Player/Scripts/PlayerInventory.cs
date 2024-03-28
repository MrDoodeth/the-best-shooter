using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Item[] inventory;
    [SerializeField][Range(2, 64)] private int maxItemAmount;
    [SerializeField] private int[] itemAmounts;
    [SerializeField] private Image[] icons;
    [SerializeField] private TMP_Text[] counterLabels;
    [SerializeField] private TMP_Text tradeLabel;
    [SerializeField] private KeyCode tradeKey;

    void Start()
    {
        tradeLabel.enabled = false;
        UpdateInventoryUI();
    }
    public bool AddItem(Item item)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if (itemAmounts[i] == 0)
            {
                inventory[i] = item;
                icons[i].sprite = item.icon;
                itemAmounts[i]++;

                item = null;
                break;
            }

            if (item.stackable && item == inventory[i] && itemAmounts[i] < maxItemAmount)
            {
                itemAmounts[i]++;

                item = null;
                break;
            }
        }
        UpdateInventoryUI();
        if (item != null)
        {
            Debug.Log("Инвентарь полон");
            return false;
        }
        return true;
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (itemAmounts[i] < 1)
            {
                counterLabels[i].enabled = false;
                SetAlpha(0f, i);
                continue;
            }
            else if (itemAmounts[i] == 1)
            {
                counterLabels[i].enabled = false;
            }
            else if (itemAmounts[i] >= 2)
            {
                counterLabels[i].text = itemAmounts[i].ToString();
                counterLabels[i].enabled = true;
            }
            SetAlpha(1f, i);
        }

        void SetAlpha(float value, int index)
        {
            Image icon = icons[index].GetComponent<Image>();

            Color color = icon.color;
            color.a = value;
            icon.color = color;
        }
    }

    private void Update()
    {
        if (PlayerRayCaster.isHit)
        {
            var _hit = PlayerRayCaster.HIT;

            ITradable iTradableEntity = _hit.transform.GetComponent<ITradable>();
            tradeLabel.enabled = true;
            if (iTradableEntity is not null)
                tradeLabel.text = $"Needed {iTradableEntity?.neededItemsAmount} {iTradableEntity?.neededItem.itemName}";

            if (Input.GetKeyDown(tradeKey))
            {
                iTradableEntity?.Trade(this, inventory, ref itemAmounts);
                //OnChangePointsAmount?.Invoke();
                tradeLabel.enabled = false;
            }
        }
        else
        {
            tradeLabel.text = "";
            tradeLabel.enabled = false;
        }
    }
}
