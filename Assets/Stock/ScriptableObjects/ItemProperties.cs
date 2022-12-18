using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemProperties", menuName = "ScriptableObjects/ItemProperties", order = 1)]
public class ItemProperties : ScriptableObject
{
    public List<Item> item;
}

[Serializable]
public class Item
{
    public int itemID;
    public ItemType item;
    public string itemName;
    public GameObject itemPrefab;
    public Sprite sprite;
    public float miningTime;
}
