using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;

namespace Shooter.Interactable.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
    [Serializable]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public bool stackable;
        public int maxItemAmount;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        Item item;
        SerializedObject serItem;
        private void OnEnable()
        {
            item = (Item)target;
            serItem = new SerializedObject(item);
        }
        public override void OnInspectorGUI()
        {
            serItem.Update();

            item.itemName = EditorGUILayout.TextField("Item name", item.itemName);
            item.icon = EditorGUILayout.ObjectField("Item icon", item.icon, typeof(Sprite), true) as Sprite;

            item.stackable = EditorGUILayout.Toggle("Stackable", item.stackable);

            if (item.stackable)
            {
                item.maxItemAmount = EditorGUILayout.IntSlider("Max item amount", item.maxItemAmount, 2, 64);
            }

            item = (Item)target;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(item);
                Undo.RecordObject(item, "Item Change");
                serItem.ApplyModifiedProperties();
            }
        }
    }
#endif
}