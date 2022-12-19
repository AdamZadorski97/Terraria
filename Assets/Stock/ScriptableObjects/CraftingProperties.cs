using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

[CreateAssetMenu(fileName = "CraftingProperties", menuName = "ScriptableObjects/CraftingProperties", order = 1)]
public class CraftingProperties : ScriptableObject
{
    public List<CraftingRecipie> recipies;
}

[Serializable]
public class CraftingRecipie
{
    [ListDrawerSettings(NumberOfItemsPerPage = 3)]
    public List<int> itemID;
    public int recipieID;
    public ItemType itemType;
    public Sprite sprite;
}
