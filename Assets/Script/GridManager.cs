using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Match3;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width;
    public int height;
    public float tileSize = 1f;
    public GameObject[] tilePrefabs;

    private GameObject[,] _grid;
    private Tile _selectedTile;
    private Dictionary<string, int> matchCount = new();

    public event Action OnMatchMade;

    #region Unity Methods

    private void Start()
    {
        _grid = new GameObject[width, height];
        InitializeGrid();
    }

    #endregion

    #region Grid Initialization

    private void InitializeGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnTile(x, y);
            }
        }

        while (CheckMatches())
        {
            RemoveAllMatches();
            FillEmptyTiles();
        }
    }

    private void SpawnTile(int x, int y)
    {
        Vector3 position = new Vector3(x * tileSize, y * tileSize);
        GameObject prefab = tilePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length)];
        GameObject tileObj = Instantiate(prefab, position, Quaternion.identity, transform);

        Tile tile = tileObj.GetComponent<Tile>();
        if (tile != null)
            tile.SetGridPosition(new Vector2Int(x, y));

        _grid[x, y] = tileObj;
    }

    private void RemoveAllMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid[x, y] == null) continue;

                string tag = _grid[x, y].tag;

                if (x < width - 2 &&
                    _grid[x + 1, y]?.tag == tag &&
                    _grid[x + 2, y]?.tag == tag)
                {
                    Destroy(_grid[x, y]);
                    Destroy(_grid[x + 1, y]);
                    Destroy(_grid[x + 2, y]);
                    _grid[x, y] = null;
                    _grid[x + 1, y] = null;
                    _grid[x + 2, y] = null;
                }

                if (y < height - 2 &&
                    _grid[x, y + 1]?.tag == tag &&
                    _grid[x, y + 2]?.tag == tag)
                {
                    Destroy(_grid[x, y]);
                    Destroy(_grid[x, y + 1]);
                    Destroy(_grid[x, y + 2]);
                    _grid[x, y] = null;
                    _grid[x, y + 1] = null;
                    _grid[x, y + 2] = null;
                }
            }
        }
    }

    private void FillEmptyTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid[x, y] == null)
                    SpawnTile(x, y);
            }
        }
    }

    #endregion

    #region Tile Selection & Swapping

    public bool SelectTile(Tile tile)
    {
        if (_selectedTile == null)
        {
            _selectedTile = tile;
        }
        else
        {
            if (AreTilesAdjacent(_selectedTile, tile))
                StartCoroutine(SwapAndCheckMatches(_selectedTile, tile));

            _selectedTile = null;
        }

        return false;
    }

    private bool AreTilesAdjacent(Tile tile1, Tile tile2)
    {
        Vector2Int pos1 = tile1.GetGridPosition();
        Vector2Int pos2 = tile2.GetGridPosition();

        return (Mathf.Abs(pos1.x - pos2.x) == 1 && pos1.y == pos2.y) ||
               (Mathf.Abs(pos1.y - pos2.y) == 1 && pos1.x == pos2.x);
    }

    private IEnumerator SwapAndCheckMatches(Tile tile1, Tile tile2)
    {
        SwapTiles(tile1, tile2);
        yield return new WaitForSeconds(0.25f);

        if (!CheckMatches())
        {
            SwapTiles(tile1, tile2); // Swap back
        }
        else
        {
            Invoke(nameof(RefillGrid), 0.2f);
        }
    }

    private void SwapTiles(Tile tile1, Tile tile2)
    {
        Vector2Int pos1 = tile1.GetGridPosition();
        Vector2Int pos2 = tile2.GetGridPosition();

        _grid[pos1.x, pos1.y] = tile2.gameObject;
        _grid[pos2.x, pos2.y] = tile1.gameObject;

        tile1.SetGridPosition(pos2);
        tile2.SetGridPosition(pos1);

        StartCoroutine(MoveTile(tile1, tile2.transform.position));
        StartCoroutine(MoveTile(tile2, tile1.transform.position));
    }

    private IEnumerator MoveTile(Tile tile, Vector3 targetPos)
    {
        if (tile == null || tile.gameObject == null) yield break;

        float duration = 0.2f;
        float elapsedTime = 0f;
        Vector3 startPos = tile.transform.position;

        while (elapsedTime < duration)
        {
            if (tile == null || tile.gameObject == null) yield break;

            tile.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (tile != null && tile.gameObject != null)
        {
            tile.transform.position = targetPos;
        }
    }


    #endregion

    #region Match Checking & Clearing

    private bool CheckMatches()
    {
        List<Vector2Int> matchedTiles = new();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid[x, y] == null) continue;

                string tag = _grid[x, y].tag;

                if (x < width - 2 &&
                    _grid[x + 1, y]?.tag == tag &&
                    _grid[x + 2, y]?.tag == tag)
                {
                    matchedTiles.Add(new Vector2Int(x, y));
                    matchedTiles.Add(new Vector2Int(x + 1, y));
                    matchedTiles.Add(new Vector2Int(x + 2, y));
                }

                if (y < height - 2 &&
                    _grid[x, y + 1]?.tag == tag &&
                    _grid[x, y + 2]?.tag == tag)
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
            OnMatchMade?.Invoke();
            return true;
        }

        return false;
    }

    private void ClearMatches(List<Vector2Int> matchedTiles)
    {
        Dictionary<string, int> matchCounter = new();

        foreach (var pos in matchedTiles)
        {
            if (_grid[pos.x, pos.y] == null) continue;

            string color = _grid[pos.x, pos.y].tag;

            if (!matchCounter.ContainsKey(color)) matchCounter[color] = 0;
            matchCounter[color]++;

            Destroy(_grid[pos.x, pos.y]);
            _grid[pos.x, pos.y] = null;
        }

        foreach (var match in matchCounter)
            TrackMatch(match.Key, match.Value);

        StartCoroutine(ApplyGravity());
    }

    #endregion

    #region Gravity & Refill

    private IEnumerator ApplyGravity()
    {
        bool tilesMoved = false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                if (_grid[x, y] != null && _grid[x, y - 1] == null)
                {
                    int fallY = y;
                    while (fallY > 0 && _grid[x, fallY - 1] == null)
                        fallY--;

                    GameObject tile = _grid[x, y];
                    _grid[x, fallY] = tile;
                    _grid[x, y] = null;

                    Tile tileComponent = tile.GetComponent<Tile>();
                    if (tileComponent != null)
                    {
                        tileComponent.SetGridPosition(new Vector2Int(x, fallY));
                        StartCoroutine(MoveTile(tileComponent, new Vector2(x * tileSize, fallY * tileSize)));
                        tilesMoved = true;
                    }
                }
            }
        }

        if (tilesMoved) yield return new WaitForSeconds(0.3f);
        RefillGrid();
    }

    private void RefillGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid[x, y] == null)
                    SpawnTile(x, y);
            }
        }

        if (CheckMatches())
            Invoke(nameof(RefillGrid), 0.2f);
    }

    #endregion

    #region Match Tracking (Debug)

    public void TrackMatch(string color, int count)
    {
        if (!matchCount.ContainsKey(color)) matchCount[color] = 0;
        matchCount[color] += count;

        Debug.Log($"Updated match count: {color} = {matchCount[color]}");
    }

    public void PrintMatchCounts()
    {
        foreach (var match in matchCount)
            Debug.Log($"{match.Key} matches: {match.Value}");
    }

    #endregion
}
