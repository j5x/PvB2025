using Gameplay.Match3;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float padding = 1.5f; // Extra space around the grid

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }

        if (gridManager != null)
        {
            AdjustCamera();
        }
        else
        {
            Debug.LogError("GridManager not found. CameraSetup requires a GridManager reference.");
        }
    }

    private void AdjustCamera()
    {
        int gridWidth = gridManager.width;
        int gridHeight = gridManager.height;
        float tileSize = gridManager.tileSize;

        // Calculate grid center position
        Vector3 gridCenter = new Vector3((gridWidth - 1) * tileSize / 2f, (gridHeight - 1) * tileSize / 2f, -10);
        cam.transform.position = gridCenter;

        // Adjust orthographic size based on grid height
        float verticalSize = (gridHeight / 2f) * tileSize + padding;
        float horizontalSize = ((gridWidth / 2f) * tileSize + padding) / cam.aspect;

        // Choose the larger size to ensure the whole grid fits
        cam.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }
}
