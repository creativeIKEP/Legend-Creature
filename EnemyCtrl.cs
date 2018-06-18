using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour {
    
    NavMeshAgent agent;
    Transform moveTarget;
    Animator animator;
    public Transform firstPoint;

    public GameObject bossNameUI;
    public GameObject bossLifeUI_b;
    public GameObject bossLifeUI_f;
    public float attackDistance;
    public bool isChaseAttack;
    public float dis = 30;
    public float runSpeed = 10;

    EnemyaAttackArea enemyaAttackArea;
    GameObject player;
    Vector3 preEnemyPos;
    float time = 0;
    bool isIdle;
    bool isAttack = false;
    bool AttackPosition = false;
    bool chaseKey = false;
    bool attackKey = false;
    float stopJudgeDis = 0.01f;
    bool isMove = true;

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
        moveTarget = firstPoint;
        agent.SetDestination(moveTarget.position);
        //agent.destination = moveTarget.position;
        animator = GetComponent<Animator>();
        state = State.walk;
        nextState = State.walk;
        enemyaAttackArea = GetComponentInChildren<EnemyaAttackArea>();
        player = GameObject.FindWithTag("Player");
        preEnemyPos = new Vector3(transform.position.x-5.0f, 0, transform.position.z);
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

        if (Input.GetKeyDown(KeyCode.P)) { StopEnemy(); }
        if (Input.GetKeyDown(KeyCode.L)) { RestartEnemy(); }
	}

    void Walk(){
        Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        print(Vector3.Distance(enemyPos, preEnemyPos));
        //Vector3 moveTargetPos = new Vector3(moveTarget.position.x, 0, moveTarget.position.z);
        //Debug.Log(Vector3.Distance(moveTarget.position, enemyPos));
        if(Vector3.Distance(moveTarget.position, enemyPos)<=1.0f || (((Vector3.Distance(enemyPos, preEnemyPos))<stopJudgeDis)&&time>5.0f)){
            //Debug.Log("walk1\n");
            time = 0;
            moveTarget.position = new Vector3(Random.Range(transform.position.x-10.0f, transform.position.x+10.0f), 0, Random.Range(transform.position.z-10.0f, transform.position.z+10.0f));
            nextState = State.idle;
            animator.SetBool("Idle", true);
        }
        else{
            //Debug.Log("walk2\n");
            agent.SetDestination(new Vector3(moveTarget.position.x, transform.position.y, moveTarget.position.z));
            transform.LookAt(new Vector3(moveTarget.position.x, transform.position.y, moveTarget.position.z));
        }
        preEnemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        time += Time.deltaTime;
    }

    void Idle(){
        return;
    }

    void Chase()
    {
        if (!isChaseAttack) { Attack(); return; }

        animator.SetBool("Idle", false);

        //攻撃が当たってないならplayerを追い続ける
        if (!chaseKey && Vector3.Distance(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z)) > 10.0f)
        {
            //突進開始
            enemyaAttackArea.OnAttack();
            //moveTarget.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
        else if (!chaseKey){
            //攻撃が当たったらそのまま走る
            //enemyaAttackArea.OnAttackTermination();
            moveTarget.position = transform.position + transform.forward * dis ;
            agent.SetDestination(moveTarget.position);
            transform.LookAt(moveTarget.position);
            chaseKey = !chaseKey;
        }
        else{
            Vector3 destination = new Vector3(agent.destination.x, transform.position.y, agent.destination.z);
            if(Vector3.Distance(transform.position, destination)<=1.0f){
                chaseKey = false;
                enemyaAttackArea.isHit = false;
                int j = Random.Range(0, 3);
                if(j==0){nextState = State.chase;}
                else{ nextState = State.attack; }
            }
            else
            {
                agent.SetDestination(moveTarget.position);
                transform.LookAt(moveTarget.position);
                if (!enemyaAttackArea.isHit) enemyaAttackArea.OnAttack();
                else { enemyaAttackArea.OnAttackTermination(); }
            }
        }
    }

    void Attack(){
        animator.SetBool("Idle", false);
        if(!AttackPosition){    //playerと距離があるなら近づく
            agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            Vector3 destination = new Vector3(agent.destination.x, transform.position.y, agent.destination.z);
            if (Vector3.Distance(transform.position, destination) <= attackDistance)   //playerの近くにきたら
            {
                AttackPosition = true;
            }
        }
        else if(!attackKey){    //攻撃中
            agent.isStopped = true;
            animator.SetBool("attack", true);
            moveTarget.position = transform.position + transform.forward * dis + transform.right * 5;
        }
        else{
            Vector3 destination = new Vector3(agent.destination.x, transform.position.y, agent.destination.z);
            if (Vector3.Distance(transform.position, destination) <= 1.0f)
            {
                AttackPosition = false;
                attackKey = false;
                enemyaAttackArea.isHit = false;
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
        isIdle = true;
    }

    public void EndIdleAnim(){
        isIdle = false;
        animator.SetBool("Idle", false);
        nextState = State.walk;
    }

    public void StartRun(){
        agent.speed = runSpeed;
    }
    public void EndRun()
    {
        enemyaAttackArea.OnAttackTermination();
        enemyaAttackArea.hitEffect.SetActive(false);
    }

    public void StartAttack(){
        Debug.Log("startAttack");
        enemyaAttackArea.OnAttack();
    }
    public void EndAttack()
    {
        Debug.Log("EndAttack");
        enemyaAttackArea.OnAttackTermination();
        enemyaAttackArea.hitEffect.SetActive(false);
    }
    public void EndAttack2(){
        Debug.Log("EndAttack2");
        attackKey = true;
        agent.SetDestination(transform.position + transform.forward * dis + transform.right * 5);
        transform.LookAt(transform.position + transform.forward * dis + transform.right * 5);
        animator.SetBool("attack", false);
    }

	private void OnTriggerEnter(Collider other)  //Playerを探す
	{
        if(other.gameObject.layer==LayerMask.NameToLayer("Player") && !isAttack){
            //Debug.Log("Found Player!");
            isAttack = true;
            nextState = State.attack;
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
            //Debug.Log("Lost Player!");
            isAttack = false;
            chaseKey = false;
            AttackPosition = false;
            attackKey = false;
            enemyaAttackArea.isHit = false;
            nextState = State.walk;
            animator.SetBool("chase", false);
            animator.SetBool("Attacking", false);
            moveTarget.position = new Vector3(firstPoint.transform.position.x, 0, firstPoint.transform.position.z);

            if (gameObject.tag == "Boss")
            {
                bossNameUI.SetActive(false);
                bossLifeUI_b.SetActive(false);
                bossLifeUI_f.SetActive(false);
            }
        }
    }

    public void StopEnemy(){
        if (isMove)
        {
            animator.StartPlayback();
            agent.Stop();
            isMove = false;
        }
    }

    public void RestartEnemy()
    {
        if (!isMove)
        {
            animator.StopPlayback();
            agent.Resume();
            isMove = true;
        }
    }
}
