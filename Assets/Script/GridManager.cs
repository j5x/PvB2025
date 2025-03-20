using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Gameplay.Match3
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int width = 8;
        [SerializeField] private int height = 8;
        [SerializeField] private float tileSize = 1.0f;

        [SerializeField] private GameObject redTilePrefab;
        [SerializeField] private GameObject blueTilePrefab;
        [SerializeField] private GameObject greenTilePrefab;

        private GameObject[,] _grid;
        private Tile _selectedTile;

        private void Start()
        {
            _grid = new GameObject[width, height];
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SpawnTile(x, y);
                }
            }
        }

        private void SpawnTile(int x, int y)
        {
            GameObject tilePrefab;
            do
            {
                tilePrefab = GetRandomTilePrefab();
            } while (WouldCreateMatch(x, y, tilePrefab));

            GameObject tile = Instantiate(tilePrefab, new Vector2(x * tileSize, y * tileSize), Quaternion.identity);
            tile.transform.parent = transform;

            Tile tileComponent = tile.GetComponent<Tile>();
            tileComponent.SetGridPosition(new Vector2Int(x, y));

            _grid[x, y] = tile;
        }

        private GameObject GetRandomTilePrefab()
        {
            int rand = Random.Range(0, 3);
            if (rand == 0) return redTilePrefab;
            if (rand == 1) return blueTilePrefab;
            return greenTilePrefab;
        }

        private bool WouldCreateMatch(int x, int y, GameObject tilePrefab)
        {
            string tileTag = tilePrefab.tag;

            if (x >= 2 &&
                _grid[x - 1, y] != null && _grid[x - 1, y].tag == tileTag &&
                _grid[x - 2, y] != null && _grid[x - 2, y].tag == tileTag)
            {
                return true;
            }

            if (y >= 2 &&
                _grid[x, y - 1] != null && _grid[x, y - 1].tag == tileTag &&
                _grid[x, y - 2] != null && _grid[x, y - 2].tag == tileTag)
            {
                return true;
            }

            return false;
        }

        public bool SelectTile(Tile tile)
        {
            if (_selectedTile == null)
            {
                _selectedTile = tile;
            }
            else
            {
                if (AreTilesAdjacent(_selectedTile, tile))
                {
                    StartCoroutine(SwapAndCheckMatches(_selectedTile, tile));
                }

                _selectedTile = null;
            }

            return false;
        }

        private IEnumerator SwapAndCheckMatches(Tile tile1, Tile tile2)
        {
            SwapTiles(tile1, tile2);

            yield return new WaitForSeconds(0.25f); // Wait for swap animation

            if (!CheckMatches())
            {
                // No match → Swap them back
                SwapTiles(tile1, tile2);
            }
            else
            {
                Invoke("RefillGrid", 0.2f);
            }
        }


        private bool AreTilesAdjacent(Tile tile1, Tile tile2)
        {
            Vector2Int pos1 = tile1.GetGridPosition();
            Vector2Int pos2 = tile2.GetGridPosition();

            return (Mathf.Abs(pos1.x - pos2.x) == 1 && pos1.y == pos2.y) || 
                   (Mathf.Abs(pos1.y - pos2.y) == 1 && pos1.x == pos2.x);
        }

        private void SwapTiles(Tile tile1, Tile tile2)
        {
            Vector2Int pos1 = tile1.GetGridPosition();
            Vector2Int pos2 = tile2.GetGridPosition();

            // Swap references in the grid
            _grid[pos1.x, pos1.y] = tile2.gameObject;
            _grid[pos2.x, pos2.y] = tile1.gameObject;

            // Swap their logical positions
            tile1.SetGridPosition(pos2);
            tile2.SetGridPosition(pos1);

            // Animate the movement
            StartCoroutine(MoveTile(tile1, tile2.transform.position));
            StartCoroutine(MoveTile(tile2, tile1.transform.position));
        }

        /// <summary>
        /// Moves a tile smoothly to a target position.
        /// </summary>
        private IEnumerator MoveTile(Tile tile, Vector3 targetPos)
        {
            float duration = 0.2f; // Swap animation duration
            float elapsedTime = 0f;
            Vector3 startPos = tile.transform.position;

            while (elapsedTime < duration)
            {
                tile.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            tile.transform.position = targetPos; // Ensure exact position at the end
        }


        private bool CheckMatches()
        {
            List<Vector2Int> matchedTiles = new List<Vector2Int>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_grid[x, y] == null) continue;

                    if (x < width - 2 &&
                        _grid[x, y].tag == _grid[x + 1, y].tag &&
                        _grid[x, y].tag == _grid[x + 2, y].tag)
                    {
                        matchedTiles.Add(new Vector2Int(x, y));
                        matchedTiles.Add(new Vector2Int(x + 1, y));
                        matchedTiles.Add(new Vector2Int(x + 2, y));
                    }

                    if (y < height - 2 &&
                        _grid[x, y].tag == _grid[x, y + 1].tag &&
                        _grid[x, y].tag == _grid[x, y + 2].tag)
                    {
                        matchedTiles.Add(new Vector2Int(x, y));
                        matchedTiles.Add(new Vector2Int(x, y + 1));
                        matchedTiles.Add(new Vector2Int(x, y + 2));
                    }
                }
            }

            if (matchedTiles.Count > 0)
            {
                ClearMatches(matchedTiles);
                return true; //Return true if a match was found
            }
            return false; // Return false if no matches
        }


        private void ClearMatches(List<Vector2Int> matchedTiles)
        {
            foreach (var pos in matchedTiles)
            {
                if (_grid[pos.x, pos.y] != null)
                {
                    Destroy(_grid[pos.x, pos.y]);
                    _grid[pos.x, pos.y] = null;
                }
            }
        }

        private void RefillGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_grid[x, y] == null)
                    {
                        SpawnTile(x, y);
                    }
                }
            }

            if (CheckMatches()) Invoke("RefillGrid", 0.2f);
        }
    }
}
