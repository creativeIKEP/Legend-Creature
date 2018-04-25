﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyaAttackArea : MonoBehaviour {
    EnemyStatus enemyStatus;
    public GameObject hitEffect;
    public bool isHit = false;

	// Use this for initialization
	void Start () {
        enemyStatus = transform.root.GetComponent<EnemyStatus>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus playerStatus = other.gameObject.transform.root.GetComponent<PlayerStatus>();
        playerStatus.Damage(enemyStatus.power);
        hitEffect.SetActive(true);
        OnAttackTermination();
        isHit=true;
        //other.gameObject.transform.root.GetComponent<PlayerMove>().MoveStop();
    }

    // 攻撃判定を有効にする.
    public void OnAttack()
    {
        GetComponent<Collider>().enabled = true;
    }


    // 攻撃判定を無効にする.
    public void OnAttackTermination()
    {
        GetComponent<Collider>().enabled = false;
    }
}