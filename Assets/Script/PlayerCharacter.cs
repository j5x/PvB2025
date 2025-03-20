using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    [Header("Player Stats")]
    [SerializeField] private int attackPower = 25;

    public override void PerformAction(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Attack:
                PerformAttack();
                break;
            case ActionType.Defend:
                PerformDefend();
                break;
            case ActionType.Mobility:
                PerformMobility();
                break;
        }
    }

    public override void PerformAttack()
    {
        // Custom attack logic for Player (e.g., Warrior, Mage)
        Debug.Log($"PLAYER performs an attack with {attackPower} power!");
    }

    public override void PerformDefend()
    {
        // Custom defense logic for Player
        Debug.Log($"PLAYER defends with {defense} defense power!");
    }

    public override void PerformMobility()
    {
        // Custom mobility logic for Player
        Debug.Log($"PLAYER moves with {agility} agility!");
    }
}