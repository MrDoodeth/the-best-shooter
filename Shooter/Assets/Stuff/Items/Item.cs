using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    [field: SerializeField] public int id { get; set; }
    [field: SerializeField] public string itemName { get; private set; }
    [field: SerializeField] public bool stackable { get; private set; }
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public GameObject physicalObject { get; private set; }

}