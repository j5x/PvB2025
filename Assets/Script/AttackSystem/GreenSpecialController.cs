using UnityEngine;
using Gameplay.Match3;

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

    private int _pendingCandy = 0;
    private bool isSwapInProgress = false;

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

    public void BeginSwap()
    {
        isSwapInProgress = true;
        _pendingCandy = 0;
    }

    private void HandleColorMatched(string color, int count)
    {
        if (!isSwapInProgress) return;

        if (color == "Green")
        {
            _pendingCandy += count * candyPerTile;
        }
    }

    private void HandleMatchMade()
    {
        if (!isSwapInProgress) return;

        // Add collected green candy to the bar
        if (_pendingCandy > 0)
        {
            specialBar.AddCandy(_pendingCandy);
            Debug.Log($"[GSC] Added {_pendingCandy} green candy");
        }

        // Fire SPECIAL attack only when bar is full
        if (specialBar.IsBarFull())
        {
            var enemy = FindObjectOfType<Enemy>();
            if (enemy != null)
            {
                specialBar.ConsumeFullBar();
                Debug.Log("[GSC] SPECIAL attack triggered!");
                attackComponent.PerformAttackByIndex(specialIndex, enemy);
            }
        }


        _pendingCandy = 0;
        isSwapInProgress = false;
    }
}
