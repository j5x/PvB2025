using UnityEngine;

namespace Gameplay.Match3
{
    public class Tile : MonoBehaviour
    {
        private Vector2Int gridPosition;
        private Vector2 touchStart;

        private float swipeThreshold = 50f; // Adjust as needed
        private GridManager gridManager;

        private void Awake()
        {
            gridManager = FindObjectOfType<GridManager>();
        }

        public void SetGridPosition(Vector2Int pos) => gridPosition = pos;
        public Vector2Int GetGridPosition() => gridPosition;

        private void OnMouseDown()
        {
            touchStart = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            Vector2 touchEnd = Input.mousePosition;
            Vector2 delta = touchEnd - touchStart;

            if (delta.magnitude < swipeThreshold) return;

            Vector2Int direction = Vector2Int.zero;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                direction = delta.x > 0 ? Vector2Int.right : Vector2Int.left;
            else
                direction = delta.y > 0 ? Vector2Int.up : Vector2Int.down;

            gridManager.TrySwapWithNeighbor(this, direction);
        }
    }
}
