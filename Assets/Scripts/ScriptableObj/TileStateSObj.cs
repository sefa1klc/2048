
using UnityEngine;

[CreateAssetMenu(menuName = "Tile State")]
public class TileStateSObj : ScriptableObject
{
    [Header("Color")]
    [Tooltip("Keeps the background color")] public Color BackgroundColor;
    [Tooltip("Keeps the text color")]public Color TextColor;
}
