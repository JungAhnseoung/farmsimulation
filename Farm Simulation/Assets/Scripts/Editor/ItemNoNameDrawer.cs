using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;

[CustomPropertyDrawer(typeof(ItemNoName))]

public class ItemNoNameDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property); 

        if(property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.BeginChangeCheck();

            var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height/2),label, property.intValue);

            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Description", GetItemInfo(property.intValue));
            if(EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }
        }
        EditorGUI.EndProperty();
    }

    private string GetItemInfo(int itemNo)
    {
        ItemList itemList = LoadItemList();
       
        List<ItemInfo> itemInfoList = itemList.itemInfo;

        ItemInfo itemInfo = itemInfoList.Find(x => x.itemNo == itemNo);

        return itemInfo == null ? "" : itemInfo.itemName;
    }

    private ItemList LoadItemList()
    {
        string path = "Assets/Items/ItemList.asset";
        return AssetDatabase.LoadAssetAtPath(path, typeof(ItemList)) as ItemList;
    }
}
