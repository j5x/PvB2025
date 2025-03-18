using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8; // Grid width
    public int height = 8; // Grid height
    public float tileSize = 1.0f; // Size of each tile
    public GameObject tilePrefab; // Prefab for the tile

    private GameObject[,] grid; // 2D array to store the grid
    private Tile selectedTile; // Currently selected tile

    void Start()
    {
        grid = new GameObject[width, height];
        GenerateGrid();
    }

    // Generates the grid and places tiles
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Instantiate a tile at the correct position
                GameObject tile = Instantiate(tilePrefab, new Vector2(x * tileSize, y * tileSize), Quaternion.identity);
                tile.transform.parent = this.transform;

                // Set the tile's grid position and type
                Tile tileComponent = tile.GetComponent<Tile>();
                tileComponent.gridPosition = new Vector2Int(x, y);
                tileComponent.type = Random.Range(0, 5); // Random type (0-4 for example)

                grid[x, y] = tile;
            }
        }
    }

    // Called when a tile is clicked
    public void SelectTile(Tile tile)
    {
        if (selectedTile == null)
        {
            // Select the first tile
            selectedTile = tile;
        }
        else
        {
            // Swap the selected tile with the clicked tile
            SwapTiles(selectedTile, tile);
            selectedTile = null;

            // Check for matches after swapping
            CheckMatches();
        }
    }

    // Swaps two tiles in the grid
    void SwapTiles(Tile tile1, Tile tile2)
    {
        // Swap their positions in the grid array
        grid[tile1.gridPosition.x, tile1.gridPosition.y] = tile2.gameObject;
        grid[tile2.gridPosition.x, tile2.gridPosition.y] = tile1.gameObject;

        // Swap their world positions
        Vector2 tempPosition = tile1.transform.position;
        tile1.transform.position = tile2.transform.position;
        tile2.transform.position = tempPosition;

        // Update their grid positions
        Vector2Int tempGridPosition = tile1.gridPosition;
        tile1.gridPosition = tile2.gridPosition;
        tile2.gridPosition = tempGridPosition;
    }

    // Checks for matches in the grid
    void CheckMatches()
    {
        bool matchFound = false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Check horizontal matches
                if (x < width - 2 &&
                    grid[x, y].GetComponent<Tile>().type == grid[x + 1, y].GetComponent<Tile>().type &&
                    grid[x, y].GetComponent<Tile>().type == grid[x + 2, y].GetComponent<Tile>().type)
                {
                    ClearMatch(x, y, true);
                    matchFound = true;
                }

                // Check vertical matches
                if (y < height - 2 &&
                    grid[x, y].GetComponent<Tile>().type == grid[x, y + 1].GetComponent<Tile>().type &&
                    grid[x, y].GetComponent<Tile>().type == grid[x, y + 2].GetComponent<Tile>().type)
                {
                    ClearMatch(x, y, false);
                    matchFound = true;
                }
            }
        }

        // Refill the grid if matches were found
        if (matchFound)
        {
            RefillGrid();
        }
    }

    // Clears a match of 3 or more tiles
    void ClearMatch(int x, int y, bool isHorizontal)
    {
        for (int i = 0; i < 3; i++)
        {
            if (isHorizontal)
            {
                Destroy(grid[x + i, y]);
                grid[x + i, y] = null;
            }
            else
            {
                Destroy(grid[x, y + i]);
                grid[x, y + i] = null;
            }
        }
    }

    // Refills the grid after clearing matches
    void RefillGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    // Create a new tile at the empty position
                    GameObject tile = Instantiate(tilePrefab, new Vector2(x * tileSize, y * tileSize), Quaternion.identity);
                    tile.transform.parent = this.transform;

                    // Set the tile's grid position and type
                    Tile tileComponent = tile.GetComponent<Tile>();
                    tileComponent.gridPosition = new Vector2Int(x, y);
                    tileComponent.type = Random.Range(0, 5); // Random type (0-4 for example)

                    grid[x, y] = tile;
                }
            }
        }

        // Check for new matches after refilling
        CheckMatches();
    }
}