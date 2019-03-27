using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Enemy
{
    protected override void Attack(Transform creature)
    {
        AttemptToAttack(transform.position, creature.position - transform.position);
    }
}
