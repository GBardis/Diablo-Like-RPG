using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    [Header("Stats")]
    public float attackDinstance;
    public float attackRate;
    private float nextAttack;

    private NavMeshAgent navMeshAgent;
    private Animator anim;

    private Transform targetEnemy;
    private bool enemyClicked;
    private bool walking;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        // storing the current position of the mouse click and this position is where the player goes
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // in right click
        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.tag == "Enemy")
                {
                    // take info of the current position (x,y,x,rotation e.t.c)
                    targetEnemy = hit.transform;
                    enemyClicked = true;
                    print("ENEMY HITTED");
                }
                else
                {
                    walking = true;
                    enemyClicked = false;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = hit.point;
                }
            }
        }
        if (enemyClicked)
        {
            MoveAndAttack();
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            walking = false;
        }
        else
        {
            walking = true;
        }
       // anim.SetBool("isWalking",walking);
    }

    void MoveAndAttack()
    {
        if (targetEnemy == null)
        {
            return;
        }
        navMeshAgent.destination = targetEnemy.position;

        if (navMeshAgent.remainingDistance > attackDinstance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else
        {
            transform.LookAt(targetEnemy);
            // direction we want to attack
            Vector3 ditToAttack = targetEnemy.transform.position - transform.position;

            if (Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
            }
            navMeshAgent.isStopped = true;
            walking = false;
        }

    }
}
