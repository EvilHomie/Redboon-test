using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class BasicItem : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    public bool stackable = true;
    
    public int startPrice;
}

public enum ItemType
{
    Helmet,
    Chest,
    Legs,
    Boots,
    LeftArm,
    RightArm,
    Projectile
}
