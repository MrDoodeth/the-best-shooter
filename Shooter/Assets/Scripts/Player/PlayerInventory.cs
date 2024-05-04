using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shooter.Interactable;
using Shooter.Interactable.Item;


namespace Shooter.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Item[] inventory;
        [SerializeField] private int[] itemAmounts;
        [SerializeField] private Image[] icons;
        [SerializeField] private TMP_Text[] counterLabels;
        [SerializeField] private TMP_Text tradeLabel;
        [SerializeField] private KeyCode tradeKey;

        private void Start()
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

                if (item == inventory[i] && item.stackable && itemAmounts[i] < item.maxItemAmount)
                {
                    itemAmounts[i]++;

                    item = null;
                    break;
                }
            }
            UpdateInventoryUI();
            if (item is not null)
            {
                print("Инвентарь полон");
                return false;
            }
            return true;
        }
        public bool TryDeleteItems(Item exampleItem, int numberDeletedItems)
        {
            int currentNeededAmount = 0; //Счётчик текущего количества предметов в инвентаре
            Stack<int> currentNeededIndexes = new(); //Стек индексов необходимых слотов

            //Подсчёт текущего количества предметов в инвентаре
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == exampleItem)
                {
                    currentNeededAmount += itemAmounts[i];
                    currentNeededIndexes.Push(i);
                }
            }

            //Хватает или нет???
            if (currentNeededAmount >= numberDeletedItems)
            {
                foreach (int index in currentNeededIndexes)
                {
                    numberDeletedItems -= itemAmounts[index];
                    if (numberDeletedItems >= 0)
                    {
                        itemAmounts[index] = 0;
                        inventory[index] = null;
                    }
                    else
                    {
                        itemAmounts[index] = -(numberDeletedItems);
                        break;
                    }
                }
                UpdateInventoryUI();
                tradeLabel.enabled = false;

                return true;            
            }
            else
            {
                print($"Нехватает {numberDeletedItems - currentNeededAmount} {exampleItem.name}");
                return false;
            }
        }

        private void UpdateInventoryUI()
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
            return;

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
            if (PlayerRayCaster.isHit && PlayerRayCaster.HIT.transform is not null)
            {
                var hit = PlayerRayCaster.HIT;
                if (hit.transform.TryGetComponent(out ITradable iTradableEntity))
                {
                    tradeLabel.enabled = true;
                    tradeLabel.text = $"Needed {iTradableEntity?.neededItemsAmount} {iTradableEntity?.neededItem.itemName}";

                    if (Input.GetKeyDown(tradeKey))
                    {
                        iTradableEntity.Trade(this);
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
            else
            {
                tradeLabel.text = "";
                tradeLabel.enabled = false;
            }
        }
    }
}
