using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : MonoBehaviour {
    public int power;
    public string weaponName;
    public GameObject hitParticle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
        EnemyStatus enemyStatus = other.gameObject.transform.root.GetComponent<EnemyStatus>();
        enemyStatus.Damage(power);
        hitParticle.SetActive(true);
        GetComponent<Collider>().enabled = false;
	}

    public void OnAttack()
    {
        GetComponent<Collider>().enabled = true;
    }


    // 攻撃判定を無効にする.
    public void OnAttackTermination()
    {
        GetComponent<Collider>().enabled = false;
        hitParticle.SetActive(false);
    }
}
