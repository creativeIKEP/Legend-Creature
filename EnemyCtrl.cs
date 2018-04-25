using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour {
    NavMeshAgent agent;
    Transform moveTarget;
    Animator animator;
    public Transform bossPoint;
    public GameObject player;

    EnemyaAttackArea enemyaAttackArea;
    bool isIdle;
    bool isChase = false;
    bool key = false;

    enum State{
        idle,
        walk,
        chase,
        attack,
        die
    }

    State state;
    State nextState;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        moveTarget = bossPoint;
        agent.SetDestination(moveTarget.position);
        //agent.destination = moveTarget.position;
        animator = GetComponent<Animator>();
        state = State.walk;
        nextState = State.walk;
        enemyaAttackArea = GetComponentInChildren<EnemyaAttackArea>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.walk) { Walk(); }
        else if (state == State.idle) { Idle(); }
        else if (state == State.chase) { Chase(); }
        else if (state == State.attack) { Attack(); }
        else { Die(); }

        if (nextState != state)
        {
            if (nextState == State.walk) { state = nextState; }
            else if (nextState == State.idle) { state = nextState; }
            else if (nextState == State.chase) { state = nextState; }
            else if (nextState == State.attack) { state = nextState; }
            else { Die(); }
        }
	}

    void Walk(){
        Debug.Log("walk\n");
        Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        //Vector3 moveTargetPos = new Vector3(moveTarget.position.x, 0, moveTarget.position.z);
        //Debug.Log(Vector3.Distance(moveTarget.position, enemyPos));
        if(Vector3.Distance(moveTarget.position, enemyPos)<=0.6f){
            moveTarget.position = new Vector3(Random.Range(transform.position.x-10.0f, transform.position.x+10.0f), 0, Random.Range(transform.position.z-10.0f, transform.position.z+10.0f));
            nextState = State.idle;
            animator.SetBool("Idle", true);
        }
        else{
            agent.SetDestination(new Vector3(moveTarget.position.x, transform.position.y, moveTarget.position.z));
            transform.LookAt(new Vector3(moveTarget.position.x, transform.position.y, moveTarget.position.z));
        }
    }

    void Idle(){
        return;
    }

    void Chase()
    {
        Debug.Log("chase");
        animator.SetBool("Idle", false);
        if (!enemyaAttackArea.isHit)
        {
            enemyaAttackArea.OnAttack();
            agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
        else if (!key){
            //enemyaAttackArea.OnAttackTermination();
            agent.SetDestination(transform.position+transform.forward*30+transform.right*5);
            transform.LookAt(transform.position + transform.forward * 30+ transform.right * 5);
            key = !key;
        }
        else{
            if(Vector3.Distance(transform.position, agent.destination)<=0.6f){
                key = false;
                enemyaAttackArea.isHit = false;
            }
        }
    }
    void Attack(){
        
    }

    void Die(){
        
    }

    public void StartIdleAnim()
    {
        //Debug.Log("startIdleAnim");
        isIdle = true;
        animator.SetBool("Idle", false);
    }

    public void EndIdleAnim(){
        //Debug.Log("endIdleAnim");
        isIdle = false;
        //nextState = State.walk;
    }

    public void StartRun(){
        agent.speed = 7;

    }
    public void EndRun()
    {
        enemyaAttackArea.OnAttackTermination();
        enemyaAttackArea.hitEffect.SetActive(false);
    }

	private void OnTriggerEnter(Collider other)  //Playerを探す
	{
        if(other.gameObject.layer==LayerMask.NameToLayer("Player") && !isChase){
            Debug.Log("Found Player!");
            isChase = true;
            state = State.chase;
            nextState = State.chase;
            animator.SetBool("chase", true);
            animator.SetBool("Attacking", true);
        }
	}
}
