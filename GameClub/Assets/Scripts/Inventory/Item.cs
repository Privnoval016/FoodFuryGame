using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Create new item")]
[System.Serializable]
public class Item : ScriptableObject {
    public int id;
    public string itemName;
    [TextArea(3, 3)] public string description;
    public enum Types {
        Throwable,
        Currency
    }
    public enum Rarity {
        Common,
        Rare,
        Unreal
    }
    
    public GameObject prefab;
    public Texture icon;
    public Types type;
    public Rarity rarity;
    public int damage;
    public Color color;
}
