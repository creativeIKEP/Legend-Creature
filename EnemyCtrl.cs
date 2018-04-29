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
    public GameObject bossNameUI;
    public GameObject bossLifeUI_b;
    public GameObject bossLifeUI_f;

    EnemyaAttackArea enemyaAttackArea;
    bool isIdle;
    bool isAttack = false;
    bool AttackPosition = false;
    bool chaseKey = false;
    bool attackKey = false;

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

        //攻撃が当たってないならplayerを追い続ける
        if (!enemyaAttackArea.isHit)
        {
            //突進開始
            enemyaAttackArea.OnAttack();
            agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
        else if (!chaseKey){
            //攻撃が当たったらそのまま走る
            //enemyaAttackArea.OnAttackTermination();
            agent.SetDestination(transform.position+transform.forward*30+transform.right*5);
            transform.LookAt(transform.position + transform.forward * 30+ transform.right * 5);
            chaseKey = !chaseKey;
        }
        else{
            if(Vector3.Distance(transform.position, agent.destination)<=0.6f){
                chaseKey = false;
                enemyaAttackArea.isHit = false;
                int j = Random.Range(0, 2);
                if(j==0){nextState = State.chase;}
                else{ nextState = State.attack; }
            }
        }
    }

    void Attack(){
        Debug.Log("Attack");
        if(!AttackPosition){    //playerと距離があるなら近づく
            agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            if (Vector3.Distance(transform.position, agent.destination) <= 10.0f)   //playerの近くにきたら
            {
                AttackPosition = true;
            }
        }
        else if(!attackKey){    //攻撃中
            agent.isStopped = true;
            animator.SetBool("attack", true);
            moveTarget.position = transform.position + transform.forward * 30 + transform.right * 5;
        }
        else{
            if (Vector3.Distance(transform.position, agent.destination) <= 0.6f)
            {
                AttackPosition = false;
                attackKey = false;
                int j = Random.Range(0, 2);
                if (j == 0) { nextState = State.chase; }
                else { nextState = State.attack; }
            }
            else{
                agent.isStopped = false;
                agent.SetDestination(moveTarget.position);
                transform.LookAt(moveTarget.position);
            }
        }
    }

    void Die(){
        
    }


    //以下、イベント関数
    public void StartIdleAnim()
    {
        //Debug.Log("startIdleAnim");
        isIdle = true;
    }

    public void EndIdleAnim(){
        //Debug.Log("endIdleAnim");
        isIdle = false;
        animator.SetBool("Idle", false);
        nextState = State.walk;
    }

    public void StartRun(){
        agent.speed = 7;
    }
    public void EndRun()
    {
        enemyaAttackArea.OnAttackTermination();
        enemyaAttackArea.hitEffect.SetActive(false);
    }

    public void StartAttack(){
        enemyaAttackArea.OnAttack();
    }
    public void EndAttack()
    {
        enemyaAttackArea.OnAttackTermination();
        enemyaAttackArea.hitEffect.SetActive(false);
    }
    public void EndAttack2(){
        attackKey = true;
        agent.SetDestination(transform.position + transform.forward * 30 + transform.right * 5);
        transform.LookAt(transform.position + transform.forward * 30 + transform.right * 5);
        animator.SetBool("attack", false);
    }

	private void OnTriggerEnter(Collider other)  //Playerを探す
	{
        if(other.gameObject.layer==LayerMask.NameToLayer("Player") && !isAttack){
            Debug.Log("Found Player!");
            isAttack = true;
            nextState = State.chase;
            animator.SetBool("chase", true);
            animator.SetBool("Attacking", true);

            if(gameObject.tag=="Boss"){
                bossNameUI.SetActive(true);
                bossLifeUI_b.SetActive(true);
                bossLifeUI_f.SetActive(true);
            }
        }
	}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && isAttack)
        {
            Debug.Log("Lost Player!");
            isAttack = false;
            nextState = State.walk;
            animator.SetBool("chase", false);
            animator.SetBool("Attacking", false);
            moveTarget = bossPoint;

            if (gameObject.tag == "Boss")
            {
                bossNameUI.SetActive(false);
                bossLifeUI_b.SetActive(false);
                bossLifeUI_f.SetActive(false);
            }
        }
    }
}
