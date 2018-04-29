using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatus : MonoBehaviour {
    public int HP;
    public int power;
    public string name;

    bool isDie = false;

    Transform life;
    Animator animator;
    NavMeshAgent agent;


	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage(int damage){
        HP -= damage;
        if(HP<=0){
            //死亡
            if (isDie == true) return;
            isDie = true;
            agent.SetDestination(transform.position);
            agent.Stop();
            animator.SetBool("die", true);
            StartCoroutine("Des");
        }
    }
    IEnumerator Des(){
        yield return new WaitForSeconds(8.0f);
        Destroy(gameObject);
    }
}
