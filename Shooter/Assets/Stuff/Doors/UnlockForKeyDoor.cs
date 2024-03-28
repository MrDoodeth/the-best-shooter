using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockForKeyDoor : MonoBehaviour, ITradable
{
    [field: SerializeField] public Item neededItem { get; set; }
    [field: SerializeField] public int neededItemsAmount { get; set; }
    [Space]
    [SerializeField] private float openDuration = 2f;
    [SerializeField][Range(-180f, 180f)] private float openingAngle = 90f;
    [SerializeField] private RotateMode rotMode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trade(PlayerInventory plInv, Item[] items, ref int[] itemsAmounts)
    {
        int currentNeededAmount = 0;
        List<int> currentNeededIconIndexes = new List<int>();
        int neededAmount = neededItemsAmount;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == neededItem)
            {
                currentNeededAmount += itemsAmounts[i];
                currentNeededIconIndexes.Add(i);
            }
        }

        if (currentNeededAmount >= neededItemsAmount)
        {
            foreach (int index in currentNeededIconIndexes)
            {
                neededAmount -= itemsAmounts[index];
                if (neededAmount >= 0)
                {
                    itemsAmounts[index] = 0;
                }
                else
                {
                    itemsAmounts[index] = -(neededAmount);
                    break;
                }                   
            }
            plInv.UpdateInventoryUI();

            //Usefull actions
            Debug.Log("Дверь открыта");
            transform.DOLocalRotate(new Vector3(0f, openingAngle, 0f), openDuration, rotMode);
            Destroy(this);
        }
        else
        {
            Debug.Log($"Нехватает {neededItemsAmount - currentNeededAmount} {neededItem.name}");
            return;
        }
    }
}
