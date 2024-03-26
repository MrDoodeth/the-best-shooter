using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Item[] inventory;
    [SerializeField][Range(2, 64)] private int maxItemAmount;
    [SerializeField] private int[] itemAmounts;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private TMP_Text[] counterLabels;

    void Start()
    {
        UpdateCounterLabels();
    }

    /// <summary>
    /// If return false => inventory is FULL
    /// </summary>
    public bool AddItem(Item item)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if (itemAmounts[i] == 0)
            {
                inventory[i] = item;               
                itemAmounts[i]++;

                var icon = slots[i].GetComponentInChildren<Image>();
                icon.sprite = item.icon;

                var color = icon.color;
                color.a = 1f;
                icon.color = color;

                item = null;
                break;
            }

            if (item.stackable && item.id == inventory[i].id && itemAmounts[i] < maxItemAmount)
            {
                itemAmounts[i]++;

                item = null;
                break;
            }
        }
        UpdateCounterLabels();
        
        if (item != null)
        {
            Debug.Log("Инвентарь полон");
            return false;
        }

        return true;
    }

    public void UpdateCounterLabels()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (itemAmounts[i] is 0 or 1)
            {
                counterLabels[i].enabled = false;
                continue;
            }

            counterLabels[i].text = itemAmounts[i].ToString();
            counterLabels[i].enabled = true;
        }
    }
}
