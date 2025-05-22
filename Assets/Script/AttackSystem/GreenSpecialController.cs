// GreenSpecialController.cs
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
    [SerializeField] private int specialIndex = 1;

    // Tracks if the bar just filled on the last OnColorMatched event
    private bool justFilled;

    private void Awake()
    {
        gridManager = gridManager ?? FindObjectOfType<GridManager>();
        specialBar = specialBar ?? FindObjectOfType<SpecialBar>();
        attackComponent = attackComponent ?? GetComponent<AttackComponent>();

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

    // Only tracks green candy and flags when it first hits full
    private void HandleColorMatch(string color, int count)
    {
        if (color != "Green") return;

        bool wasFull = specialBar.CandyAmount >= specialBar.MaxCandy;
        specialBar.AddCandy(count * candyPerTile);
        bool isFull = specialBar.CandyAmount >= specialBar.MaxCandy;

        justFilled = !wasFull && isFull;
    }

    // On any match: either fire normal or special (but never both at once)
    private void HandleAnyMatch()
    {
        var enemy = FindObjectOfType<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning("[GSC] No Enemy found for attack!");
            return;
        }

        if (justFilled)
        {
            Debug.Log("[GSC] Bar just filled → normal attack (special delayed)");
            attackComponent.PerformAttackByIndex(null, enemy);
            justFilled = false;
            return;
        }

        if (specialBar.CandyAmount >= specialBar.MaxCandy)
        {
            specialBar.ConsumeFullBar();
            Debug.Log("[GSC] Bar full → SPECIAL attack");
            attackComponent.PerformAttackByIndex(specialIndex, enemy);
        }
        else
        {
            Debug.Log("[GSC] Bar not full → normal attack");
            attackComponent.PerformAttackByIndex(null, enemy);
        }
    }

}
