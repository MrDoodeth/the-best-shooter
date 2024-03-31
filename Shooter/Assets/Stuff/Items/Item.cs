using System;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName { get; set; }
    public Sprite icon { get; set; }
    public bool stackable { get; set; }
    public int maxItemAmount { get; set; }
}

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var item = (Item)target;

        item.itemName = EditorGUILayout.TextField("Item name", item.itemName);
        item.icon = EditorGUILayout.ObjectField("Item icon", item.icon, typeof(Sprite), true) as Sprite;

        item.stackable = EditorGUILayout.Toggle("Stackable", item.stackable);

        if (item.stackable)
        {
            item.maxItemAmount = EditorGUILayout.IntSlider("Max item amount", item.maxItemAmount, 1, 64);
        }
    }
}