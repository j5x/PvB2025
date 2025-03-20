using UnityEngine;

public class Player : Character
{
    private float attackPower;

    protected override void Move()
    {
        transform.position = new Vector2(-5, 0);
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void Defend()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        Move();
    }
}