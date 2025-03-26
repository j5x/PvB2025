using UnityEngine;
using Gameplay.Match3; // Make sure this namespace matches the GridManager's namespace

public class Player : Character
{
    private GridManager gridManager; // Reference to the GridManager

    protected override void Attack()
    {
        // Log a message when the player attacks
        Debug.Log("I am Attacking");
    }

    protected override void Defend()
    {
        // Add defend logic if you want later
        Debug.Log("Player is defending.");
    }

    private void Start()
    {
        // Find the GridManager in the scene
        gridManager = FindObjectOfType<GridManager>();
        
        // Subscribe to the OnMatchMade event
        if (gridManager != null)
        {
            gridManager.OnMatchMade += Attack;
        }
        else
        {
            Debug.LogError("GridManager not found in the scene!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the player is destroyed
        if (gridManager != null)
        {
            gridManager.OnMatchMade -= Attack;
        }
    }
}