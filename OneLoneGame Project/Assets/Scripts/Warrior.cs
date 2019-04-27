using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Enemy
{
    protected override void Attack(Transform creature)
    {
        CmdAttemptToAttack(transform.position, creature.position - transform.position);
    }


    protected override Vector2 GetMovement(Transform creature)
    {
        float distance = Vector2.Distance(creature.position, transform.position);
        int dir;
        Vector2 movement = new Vector2(0, 0);
        if (distance > offset)
        {
            if (!isEnemySeen)
            {
                float now = Time.time;
                if (now - lastTimeMoveChange > 1f)
                {
                    lastTimeMoveChange = Time.time;
                    dir = Random.Range(0, 9);
                    switch (dir)
                    {
                        case 0:
                            movement = new Vector3(1, 0, 0);
                            break;
                        case 1:
                            movement = new Vector3(-1, 0, 0);
                            break;
                        case 2:
                            movement = new Vector3(0, 1, 0);
                            break;
                        case 3:
                            movement = new Vector3(0, -1, 0);
                            break;
                        case 4:
                            movement = new Vector3(1, 1, 0);
                            break;
                        case 5:
                            movement = new Vector3(1, -1, 0);
                            break;
                        case 6:
                            movement = new Vector3(-1, -1, 0);
                            break;
                        case 7:
                            movement = new Vector3(-1, 1, 0);
                            break;
                        case 8:
                            movement = new Vector3(0, 0, 0);
                            break;
                    }
                    movement.Normalize();
                }
                return movement;
            }
            else
            {
                return (creature.position - transform.position).normalized;
            }
        }
        return movement;

    }
}
