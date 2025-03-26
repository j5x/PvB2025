using Script;
using UnityEngine;

public class Enemy : Character
{
    
    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void Defend()
    {
        throw new System.NotImplementedException();
    }

    public void Start()
    {
        Attack();
    }
}