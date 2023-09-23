using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class BasicItem : ScriptableObject
{
    public TileBase tile;
    public Sprite image;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public bool stackable = true;
}

public enum ItemType
{
    Armor,
    Projectile    
}

public enum ActionType
{
    Helmet,
    Chest,
    Arrow
}