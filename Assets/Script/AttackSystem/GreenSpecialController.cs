using UnityEngine;
using Gameplay.Match3;  // make sure this matches your GridManager namespace

[RequireComponent(typeof(AttackComponent))]
public class GreenSpecialController : MonoBehaviour
{
    [Header("Match3 → Candy")]
    [SerializeField] private GridManager   gridManager;
    [SerializeField] private int           candyPerTile  = 1;

    [Header("Candy → UI")]
    [SerializeField] private SpecialBar    specialBar;

    [Header("Attack")]
    [SerializeField] private AttackComponent attackComponent;
    [SerializeField] private int              specialIndex = 1;

    private void Awake()
    {
        if (gridManager     == null) gridManager     = FindObjectOfType<GridManager>();
        if (specialBar      == null) specialBar      = FindObjectOfType<SpecialBar>();
        if (attackComponent == null) attackComponent = GetComponent<AttackComponent>();

        Debug.Assert(gridManager     != null, "Missing GridManager");
        Debug.Assert(specialBar      != null, "Missing SpecialBar");
        Debug.Assert(attackComponent != null, "Missing AttackComponent");
    }

    private void OnEnable()
    {
        gridManager.OnColorMatched += HandleColorMatch;
        gridManager.OnMatchMade    += HandleAnyMatch;
    }

    private void OnDisable()
    {
        gridManager.OnColorMatched -= HandleColorMatch;
        gridManager.OnMatchMade    -= HandleAnyMatch;
    }

    // Only fills on green
    private void HandleColorMatch(string color, int count)
    {
        if (color == "Green")
            specialBar.AddCandy(count * candyPerTile);
    }

    // Chooses special if bar is full *right now*, else normal
    private void HandleAnyMatch()
    {
        // find the current enemy once
        var enemy = FindObjectOfType<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning("[GSC] No Enemy found for attack!");
            return;
        }

        if (specialBar.IsFull)
        {
            specialBar.ConsumeFullBar();
            Debug.Log("[GSC] Bar full → special attack");
            // pass both index *and* target
            attackComponent.PerformAttack(specialIndex, enemy);
        }
        else
        {
            Debug.Log("[GSC] Bar not full → normal attack");
            // use null for index to pick random, but still pass target
            attackComponent.PerformAttack(null, enemy);
        }
    }

}
