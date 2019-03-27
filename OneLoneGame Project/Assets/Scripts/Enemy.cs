using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NonPlayer
{
    protected float lastAttackTime;
    // Start is called before the first frame update

    virtual protected void Attack(Transform creature)
    {

    }

    // Update is called once per frame
    protected override void Live()
    {
        if (health <= 0)
            Die();
        Transform nearestEnemy = FindClosestVisibleEnemy();
        float distance = aggroDistance + 1;
        if (nearestEnemy != null)
        {
            isEnemySeen = true;
            float now = Time.time;
            if (now - lastAttackTime > 2.0f)
            {
                Attack(nearestEnemy);
                lastAttackTime = now;
            }

            distance = Vector2.Distance(transform.position, nearestEnemy.position);
            if (distance > offset)
                movement = GetMovement(nearestEnemy);
            else
                movement = new Vector3(0, 0, 0);
        }
        else
        {
            movement = GetMovement();
            isEnemySeen = false;
        }        
        rb.velocity = movement * speed;
    }

   virtual protected Vector2 GetMovement(Transform creature)
    {
        int dir;
        Vector2 movement = new Vector2(0,0);
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
}
