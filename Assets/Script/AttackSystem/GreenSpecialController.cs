using UnityEngine;
using Gameplay.Match3;  // your GridManager namespace

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
    [Tooltip("Which slot in attackConfigs is your green special?")]
    [SerializeField] private int specialIndex = 1;

    // — internal state to block cascades —
    private bool _hasProcessedThisSwap = false;
    private int _pendingCandy = 0;

    private void Awake()
    {
        gridManager = gridManager ? gridManager : FindObjectOfType<GridManager>();
        specialBar = specialBar ? specialBar : FindObjectOfType<SpecialBar>();
        attackComponent = attackComponent ? attackComponent : GetComponent<AttackComponent>();

        Debug.Assert(gridManager != null, "Missing GridManager");
        Debug.Assert(specialBar != null, "Missing SpecialBar");
        Debug.Assert(attackComponent != null, "Missing AttackComponent");
    }

    private void OnEnable()
    {
        gridManager.OnColorMatched += HandleColorMatched;
        gridManager.OnMatchMade += HandleMatchMade;
    }

    private void OnDisable()
    {
        gridManager.OnColorMatched -= HandleColorMatched;
        gridManager.OnMatchMade -= HandleMatchMade;
    }

    private void HandleColorMatched(string color, int count)
    {
        if (color != "Green")
            return;

        // first green of a new swap? reset the block flag & tally:
        if (_hasProcessedThisSwap && _pendingCandy == 0)
        {
            _hasProcessedThisSwap = false;
            _pendingCandy = count * candyPerTile;
        }
        else if (!_hasProcessedThisSwap)
        {
            // still first-match in this swap → accumulate
            _pendingCandy += count * candyPerTile;
        }
        // else: we’ve already fired for this swap, ignore cascades
    }

    private void HandleMatchMade()
    {
        if (_hasProcessedThisSwap) return;  // one attack per swap only

        // 1) apply all green candy accrued
        specialBar.AddCandy(_pendingCandy);

        // 2) pick target
        var enemy = FindObjectOfType<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning("[GSC] No Enemy found — skipping attack");
        }
        else
        {
            // 3) fire special or normal
            if (specialBar.ConsumeFullBar())
            {
                Debug.Log("[GSC] Bar full → SPECIAL attack");
                attackComponent.PerformAttackByIndex(specialIndex, enemy);
            }
            else
            {
                Debug.Log("[GSC] Bar not full → normal attack");
                attackComponent.PerformAttackByIndex(null, enemy);
            }
        }

        // 4) block further cascades until next swap
        _hasProcessedThisSwap = true;
        _pendingCandy = 0;
    }
}
