using UnityEngine;

public class Warrior : Character
{
    protected override void Attack()
    {
        Debug.Log("Warrior slashes with sword!");
    }

    protected override void Defend()
    {
        Debug.Log("Warrior raises shield!");
    }
}
