using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "ItemProperties", menuName = "ScriptableObjects/ItemProperties", order = 1)]
public class ItemProperties : ScriptableObject
{
    public int itemID;
    public ItemType itemType;
    public string itemName;
    public GameObject itemPrefab;
    public Sprite sprite;
    public float miningTime;
    public Tile tile;
    public List<LootOnMined> loot;
}


[System.Serializable]
public class LootOnMined
{
    public ItemProperties item;
    public int amount;
}

public enum ItemType
{
    none,
    block,
    ore,
    interactiveItem,
    weapon,
}