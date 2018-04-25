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

    public void SetWalkState() { state = State.walk; }
    public void SetAttackState() { state = State.attack; }
    public void SetSwimState() { state = State.swim; }

	// Use this for initialization
	void Start () {
        uICtrl = FindObjectOfType<UICtrl>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage(int damage)
    {
        GetComponent<PlayerMove>().MoveStop();
        StartCoroutine("MoveReStart");
        HP -= damage;
        uICtrl.HPChange(HP, MaxHP);
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
        yield return new WaitForSeconds(2.5f);
        GetComponent<PlayerMove>().MoveReStart();
    }
}
