using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    public int HP;
    public int MaxHP;
    public string name;

    public enum State
    {
        walk,
        attack,
        swim, 
        die
    };
    public State state;

    UICtrl uICtrl;
    Animator animator;
    EnemyaAttackArea enemyaAttackArea;

    CharacterController characterController;

    public void SetWalkState() { state = State.walk; }
    public void SetAttackState() { state = State.attack; }
    public void SetSwimState() { state = State.swim; }

	// Use this for initialization
	void Start () {
        uICtrl = FindObjectOfType<UICtrl>();
        animator = GetComponent<Animator>();
        enemyaAttackArea = FindObjectOfType<EnemyaAttackArea>();
        characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Damage(int damage, Vector3 atDamage)
    {
        GetComponent<PlayerMove>().MoveStop();
        StartCoroutine("MoveReStart");
        HP -= damage;
        uICtrl.HPChange(HP, MaxHP);
        characterController.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForceAtPosition((transform.position - atDamage).normalized * 7, atDamage, ForceMode.VelocityChange);
        if (HP <= 0)
        {
            //死亡
            animator.SetBool("die", true);
        }
        else{
            animator.SetBool("damage", true);
        }
    }

    IEnumerator MoveReStart(){
        yield return new WaitForSeconds(0.5f);
        characterController.enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(2.0f);
        GetComponent<PlayerMove>().MoveReStart();
    }
}
