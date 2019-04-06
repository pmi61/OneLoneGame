using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayer : Entity
{
    #region FoW

    [SerializeField] protected float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    protected IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    protected void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask) && dstToTarget != 0)
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    #endregion
    [SerializeField] protected float offset;
    [SerializeField] protected float aggroDistance;
    protected bool isEnemySeen;
    protected float lastTimeMoveChange;
    protected Transform nearestEnemy;
    // Start is called before the first frame update
    void Start()
    {
        speed = startSpeed;
        lastTimeMoveChange = Time.time;
        isEnemySeen = false;
        SetMaxValues(maxHealth, maxStamina, staminaDelta);
        StartCoroutine("FindTargetsWithDelay", .2f);
        lastAttackTime = 0;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!GameManager.instance.isGameRunning || GameManager.instance.IsInMenu)
            return;
        now = Time.time;
        Live();
    }


    virtual protected Vector2 GetMovement()
    {
        int dir;
        Vector2 movement = this.movement;
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
    virtual protected void Live()
    {

    }

    protected Transform FindClosestVisibleEnemy()
    {
        Transform nearestCreature = null;
        if (visibleTargets.Count > 0)
        {
            nearestCreature = visibleTargets[0];
            float distance = Vector2.Distance(transform.position, nearestCreature.position);
            foreach (Transform creature in visibleTargets)
            {
                if (creature != null)
                {
                    if (creature != nearestCreature && distance > Vector2.Distance(transform.position, creature.position))
                    {
                        nearestCreature = creature;
                        distance = Vector2.Distance(transform.position, creature.position);
                    }
                }
            }
        }
        return nearestCreature;
    }

}
