using UnityEngine;

public class Tile : MonoBehaviour
{
    public int type; // Type of the tile (e.g., color or shape)
    public Vector2Int gridPosition; // Position of the tile in the grid

    private GridManager gridManager;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    // Called when the tile is clicked
    void OnMouseDown()
    {
        gridManager.SelectTile(this);
    }
}