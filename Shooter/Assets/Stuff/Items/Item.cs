using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public int id { get; set; }
    [field: SerializeField] public string itemName { get; set; }
    [field: SerializeField] public bool stackable { get; set; }
    [field: SerializeField] public Sprite icon { get; set; }

    public void Take()
    {

    }
}