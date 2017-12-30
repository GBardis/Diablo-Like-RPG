using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour {
	[Header("Stats")]
	public float attackDistance;
	public float attackRate;
	private float nextAttack;

	//NAV MESH
	private NavMeshAgent navMeshAgent;
	private Animator anim;

	//ENEMY
	private Transform targetedEnemy;
	private bool enemyClicked;
	private bool walking;

	//OBJECTS
	private Transform clickedObject;
	private bool objectClicked;

	//DOUBLE CLICK
	private bool oneClick;
	private bool doubleClick;
	private float timerForDoubleClick;
	private float delay = 0.25f;

	void Awake () 
	{
		anim = GetComponent<Animator>();
		//anim = GetComponentInChildren<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		CheckDoubleClick();

		if (Input.GetButtonDown("Fire1"))
		{
			navMeshAgent.ResetPath();
			if (Physics.Raycast(ray, out hit, 1000))
			{
				if (hit.collider.tag == "Enemy")
				{
					targetedEnemy = hit.transform;
					enemyClicked = true;

					//print("ENEMY HITTED");
				}
				else if(hit.collider.tag == "Chest")
				{
					objectClicked = true;
					clickedObject = hit.transform;
				}
				else if(hit.collider.tag == "Info")
				{
					objectClicked = true;
					clickedObject = hit.transform;
				}
				else
				{
					walking = true;
					enemyClicked = false;
					navMeshAgent.destination = hit.point;
					navMeshAgent.isStopped = false;
				}
			}
		}

		if (enemyClicked && doubleClick)
		{
			MoveAndAttack();
		}
		else if(enemyClicked)
		{
			//select enemy
		}
		else if (objectClicked && clickedObject.gameObject.tag == "Info")
		{
			ReadInfos(clickedObject);
		}
		else if (objectClicked && clickedObject.gameObject.tag == "Chest")
		{
			OpenChest(clickedObject);
		}
		else
		{
			if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance )
			{
				walking = false;
			}
			else if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance )
			{
				walking = true;
			}
		}

		//anim.SetBool("isWalking", walking);

        //handle shift press
        if(Input.GetKey(KeyCode.LeftShift))
        {
            //toggle running
            navMeshAgent.speed = 7f;
            anim.SetBool("isRunning", walking);
            anim.SetBool("isWalking", false);
        }
        else
        {
            //toggle running
            navMeshAgent.speed = 3.5f;
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", walking);
        }


        if (!walking)
        {
            anim.SetBool("isIdling", true);
        }
        else
        {
            anim.SetBool("isIdling", false);
        }
	}

	void MoveAndAttack()
	{
		if (targetedEnemy == null)
		{
			return;
		}
		navMeshAgent.destination = targetedEnemy.position;

		if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
		{
			navMeshAgent.isStopped = false;
			walking = true;
		}
		else if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance)
		{
			anim.SetBool("isAttacking",false);
			transform.LookAt(targetedEnemy);
			Vector3 dirToAttack = targetedEnemy.transform.position - transform.position;

			if (Time.time > nextAttack)
			{
                targetedEnemy.GetComponent<Interactable>().Interact();

				nextAttack = Time.time + attackRate;
				//CALL THE ATTACK WITH THE DIRTOATTACK
				anim.SetBool("isAttacking",true);
			}
			navMeshAgent.isStopped = true;
			walking = false;
		}
	}
		
	void ReadInfos(Transform target)
	{
		//set target
		navMeshAgent.destination = target.position;
		//go close
		if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
		{
			navMeshAgent.isStopped = false;
			walking = true;
		}
		//then read
		else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance)
		{
			navMeshAgent.isStopped = true;
			transform.LookAt(target);
			walking = false;

			//print an info
			print(target.GetComponent<Infos>().info);

			objectClicked = false;
			navMeshAgent.ResetPath();
		}

	}

	void OpenChest(Transform target)
	{
		//set target
		navMeshAgent.destination = target.position;
		//go close
		if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
		{
			navMeshAgent.isStopped = false;
			walking = true;
		}
		//then read
		else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance)
		{
			navMeshAgent.isStopped = true;
			transform.LookAt(target);
			walking = false;

			//play animatiion
			target.gameObject.GetComponentInChildren<Animator>().SetTrigger("Play");
			//print(target.GetComponent<Infos>().info);

			objectClicked = false;
			navMeshAgent.ResetPath();
		}

	}

	void CheckDoubleClick()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (!oneClick)
			{
				oneClick = true;
				timerForDoubleClick = Time.time;
			}
			else
			{
				oneClick = false;
				doubleClick = true;
			}
		}

		if (oneClick)
		{
			if ((Time.time - timerForDoubleClick) > delay)
			{
				oneClick = false;
				doubleClick = false;
			}
		}
	}
}
