using UnityEngine;

namespace Gameplay.Match3
{
    public class Tile : MonoBehaviour
    {
        private Vector2Int gridPosition;
        private GridManager gridManager;
        private Vector3 originalScale;
        private bool isSelected = false;

        private void Start()
        {
            gridManager = FindObjectOfType<GridManager>();
            originalScale = transform.localScale;
        }

        private void OnMouseDown()
        {
            isSelected = gridManager.SelectTile(this);
            UpdateScale();
        }

        private void OnMouseEnter()
        {
            if (!isSelected)
                transform.localScale = originalScale * 1.1f; // Slightly bigger when hovering
        }

        private void OnMouseExit()
        {
            if (!isSelected)
                transform.localScale = originalScale; // Reset when not hovering
        }

        public void SetGridPosition(Vector2Int newPosition)
        {
            gridPosition = newPosition;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public void Deselect()
        {
            isSelected = false;
            UpdateScale();
        }

        private void UpdateScale()
        {
            transform.localScale = isSelected ? originalScale * 1.2f : originalScale; // Bigger when selected
        }
    }
}
