using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Gameplay.Match3
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] public int width = 8;
        [SerializeField] public int height = 8;
        [SerializeField] public float tileSize = 1.0f;

        [SerializeField] private GameObject redTilePrefab;
        [SerializeField] private GameObject blueTilePrefab;
        [SerializeField] private GameObject greenTilePrefab;

        [SerializeField] private GameObject matchVFXPrefab;
        [SerializeField] private GameObject swipeHintPrefab;
        [SerializeField] private Vector3 swipeHintOffset = new Vector3(0, -1f, 0);

        private GameObject[,] _grid;
        private Tile _selectedTile;

        private Dictionary<string, int> matchCount = new Dictionary<string, int>();

        public event System.Action OnMatchMade;
        public event System.Action<string, int> OnColorMatched;
        public event System.Action<int> OnGreenMatched;

        [SerializeField] private Vector3 gridOffset;

        private float idleTimer = 0f;
        private bool hintShown = false;

        private void Start()
        {
            _grid = new GameObject[width, height];

            float gridWidth = width * tileSize;
            float gridHeight = height * tileSize;

            GenerateGrid();

            matchCount["Red"] = 0;
            matchCount["Blue"] = 0;
            matchCount["Green"] = 0;
            matchCount["Yellow"] = 0;
        }

        private void Update()
        {
            idleTimer += Time.deltaTime;
            if (!hintShown && idleTimer >= 3f)
            {
                ShowSwipeHint();
            }
        }

        private void ResetIdleTimer()
        {
            idleTimer = 0f;
            hintShown = false;
        }

        private void ShowSwipeHint()
        {
            if (swipeHintPrefab == null) return;
            hintShown = true;
            Vector3 center = new Vector3((width - 1) * tileSize / 2f, (height - 1) * tileSize / 2f, 0) + gridOffset + swipeHintOffset;
            GameObject hint = Instantiate(swipeHintPrefab, center, Quaternion.identity, transform);
            Destroy(hint, 2f);
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

            Vector3 spawnPosition = new Vector3(x * tileSize, y * tileSize, 0) + gridOffset;

            GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);

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
            ResetIdleTimer();

            // 👇 NEW: Notify GreenSpecialController about the swap
            GreenSpecialController gsc = FindObjectOfType<GreenSpecialController>();
            if (gsc != null)
                gsc.BeginSwap();

            SwapTiles(tile1, tile2);
            yield return new WaitForSeconds(0.25f);

            if (!CheckMatches())
            {
                SwapTiles(tile1, tile2);
            }
            else
            {
                Invoke(nameof(RefillGrid), 0.2f);
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

            _grid[pos1.x, pos1.y] = tile2.gameObject;
            _grid[pos2.x, pos2.y] = tile1.gameObject;

            tile1.SetGridPosition(pos2);
            tile2.SetGridPosition(pos1);

            StartCoroutine(MoveTile(tile1, tile2.transform.position));
            StartCoroutine(MoveTile(tile2, tile1.transform.position));
        }

        private IEnumerator MoveTile(Tile tile, Vector3 unusedTargetPos)
        {
            if (tile == null || tile.gameObject == null) yield break;

            float duration = 0.2f;
            float elapsedTime = 0f;

            Vector2Int gridPos = tile.GetGridPosition();
            Vector3 targetPos = new Vector3(gridPos.x * tileSize, gridPos.y * tileSize, 0) + gridOffset;
            Vector3 startPos = tile.transform.position;

            while (elapsedTime < duration)
            {
                if (tile == null || tile.gameObject == null) yield break;

                tile.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (tile != null && tile.gameObject != null)
                tile.transform.position = targetPos;
        }

        private bool CheckMatches()
        {
            ResetIdleTimer();
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
                OnMatchMade?.Invoke();
                return true;
            }
            return false;
        }

        public void TrackMatch(string color, int count)
        {
            if (matchCount.ContainsKey(color))
                matchCount[color] += count;
            else
                matchCount[color] = count;

            Debug.Log($"Updated match count: {color} = {matchCount[color]}");
        }

        public void PrintMatchCounts()
        {
            foreach (var match in matchCount)
                Debug.Log($"{match.Key} matches: {match.Value}");
        }

        private void ClearMatches(List<Vector2Int> matchedTiles)
        {
            var matchCounter = new Dictionary<string, int>();

            foreach (var pos in matchedTiles)
            {
                if (_grid[pos.x, pos.y] != null)
                {
                    string color = _grid[pos.x, pos.y].tag;

                    if (matchCounter.ContainsKey(color))
                        matchCounter[color]++;
                    else
                        matchCounter[color] = 1;

                    Vector3 vfxPosition = _grid[pos.x, pos.y].transform.position;
                    Instantiate(matchVFXPrefab, vfxPosition, Quaternion.identity);

                    Destroy(_grid[pos.x, pos.y]);
                    _grid[pos.x, pos.y] = null;
                }
            }

            foreach (var kv in matchCounter)
            {
                string color = kv.Key;
                int count = kv.Value;

                TrackMatch(color, count);
                Debug.Log($"[GridManager] OnColorMatched on {name} (ID {GetInstanceID()})");
                OnColorMatched?.Invoke(color, count);

                if (color == "Green")
                {
                    Debug.Log($"[GridManager] 🔔 OnGreenMatched({count}) about to fire");
                    OnGreenMatched?.Invoke(count);
                }
            }

            StartCoroutine(ApplyGravity());
        }

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

                        GameObject tileToMove = _grid[x, y];
                        _grid[x, fallY] = tileToMove;
                        _grid[x, y] = null;

                        if (tileToMove != null)
                        {
                            Tile tileComponent = tileToMove.GetComponent<Tile>();
                            tileComponent.SetGridPosition(new Vector2Int(x, fallY));
                            StartCoroutine(MoveTile(tileComponent, new Vector3(x * tileSize, fallY * tileSize, 0) + gridOffset));
                            tilesMoved = true;
                        }
                    }
                }
            }

            if (tilesMoved)
                yield return new WaitForSeconds(0.3f);

            RefillGrid();
        }

        private void RefillGrid()
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (_grid[x, y] == null)
                        SpawnTile(x, y);

            if (CheckMatches())
                Invoke(nameof(RefillGrid), 0.2f);
        }

        public void TrySwapWithNeighbor(Tile tile, Vector2Int direction)
        {
            Vector2Int pos = tile.GetGridPosition();
            Vector2Int neighborPos = pos + direction;

            if (neighborPos.x < 0 || neighborPos.x >= width || neighborPos.y < 0 || neighborPos.y >= height)
                return;

            GameObject neighborObj = _grid[neighborPos.x, neighborPos.y];
            if (neighborObj == null) return;

            Tile neighborTile = neighborObj.GetComponent<Tile>();
            if (neighborTile == null) return;

            StartCoroutine(SwapAndCheckMatches(tile, neighborTile));
        }
    }
}
