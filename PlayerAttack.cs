using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    Animator animator;
    PlayerAttackArea playerAttackArea;
    PlayerStatus playerStatus;
    PlayerMove playerMove;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        playerAttackArea = GetComponentInChildren<PlayerAttackArea>();
        playerStatus = GetComponent<PlayerStatus>();
        playerMove = GetComponent<PlayerMove>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack(){
        if (!animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))//遷移途中でない
        {
            playerStatus.SetAttackState();
            int randam = Random.Range(1, 3);
            if (randam == 1) { animator.SetBool("attack1", true); }
            else{animator.SetBool("attack2", true);}
            playerMove.MoveStop();
        }
    }

    void StartAttackMotion(){
        animator.applyRootMotion = true;
        playerAttackArea.OnAttack();
        playerMove.MoveStop();
    }
    void EndAttackMotion(){
        animator.applyRootMotion = false;
        playerAttackArea.OnAttackTermination();
        animator.SetBool("attack1", false);
        playerStatus.SetWalkState();
        playerMove.MoveReStart();
    }
}
