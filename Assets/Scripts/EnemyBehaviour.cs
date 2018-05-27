using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR 
using UnityEditor;
#endif 
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : Interactable {
    //NAV MESH AGENT
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool walking;

    //RADIUS
    public float lookRadius;
    public float attackRadius;

    //ATTACK
    private float nextAttack;
    public float attackRate = 1f; /*1f = 1second long*/


    //TARGET
    Transform targetPlayer;


	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetPlayer = PlayerManager.instance.ourPlayer.transform;

	}
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("walking",walking);

        if (!walking)
        {
            animator.SetBool("idling", true);
        }
        else
        {
            animator.SetBool("idling", false);
        }

        float distance = Vector3.Distance(transform.position, targetPlayer.position);


        if(distance <= lookRadius)
        {
            //move towards the player
            MoveAndAttack();
            //attack
        }
        //update walking!
        else
        {
            walking = false;
        }

        //walking = false;
       
    }

    void MoveAndAttack()
    {
        navMeshAgent.destination = targetPlayer.position;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackRadius)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackRadius) /*we're close enough*/
        {
            //attack
            animator.SetBool("fighting", false);
            transform.LookAt(targetPlayer);

            if(Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;

                animator.SetBool("fighting", true);
            }

            navMeshAgent.isStopped = true;
            walking = false;
        }
    }

    private void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.position+new Vector3(0,0.02f,0),transform.up,transform.right,360,lookRadius);

        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position + new Vector3(0, 0.02f, 0), transform.up, transform.right, 360, attackRadius);
    #endif
    }

    public override void Interact()
    {
        Debug.Log("Enemy Got Damage");

        base.Interact();
    }
}
