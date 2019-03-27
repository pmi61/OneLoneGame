using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    
    protected override void Attack(Transform creature)
    {
           FireArrow(transform.position, creature.position);
    }

    protected override Vector2 GetMovement(Transform creature)
    {
        int dir;
        Vector2 movement = new Vector2(0, 0);
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
            return (transform.position - nearestEnemy.position).normalized;
        }

    }
}
