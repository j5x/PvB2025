using UnityEngine;
using Gameplay.Match3;  // make sure this matches your GridManager namespace

[RequireComponent(typeof(AttackComponent))]
public class GreenSpecialController : MonoBehaviour
{
    [Header("Match3 → Candy")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int candyPerTile = 1;

    [Header("Candy → UI")]
    [SerializeField] private SpecialBar specialBar;

    [Header("Attack")]
    [SerializeField] private AttackComponent attackComponent;
    [SerializeField] private int specialIndex = 1;

    private void Awake()
    {
        if (gridManager == null) gridManager = FindObjectOfType<GridManager>();
        if (specialBar == null) specialBar = FindObjectOfType<SpecialBar>();
        if (attackComponent == null) attackComponent = GetComponent<AttackComponent>();

        // sanity
        Debug.Assert(gridManager != null, "Missing GridManager");
        Debug.Assert(specialBar != null, "Missing SpecialBar");
        Debug.Assert(attackComponent != null, "Missing AttackComponent");
    }

    private void OnEnable()
    {
        gridManager.OnColorMatched += HandleColorMatch;
        gridManager.OnMatchMade += HandleAnyMatch;
    }

    private void OnDisable()
    {
        gridManager.OnColorMatched -= HandleColorMatch;
        gridManager.OnMatchMade -= HandleAnyMatch;
    }

    // Only used to fill the bar on green
    private void HandleColorMatch(string color, int count)
    {
        if (color == "Green")
            specialBar.AddCandy(count * candyPerTile);
    }

    // Dispatches whichever attack is appropriate
    private void HandleAnyMatch()
    {
        // If bar is full, consume & fire special
        if (specialBar.ConsumeFullBar())
        {
            Debug.Log("[GSC] Bar full → special attack");
            attackComponent.PerformAttack(specialIndex);
        }
        else
        {
            // Otherwise do your normal attack (random/basic)
            attackComponent.PerformAttack();
        }
    }
}
