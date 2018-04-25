using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeUI : MonoBehaviour {
    Camera camera;
    EnemyStatus enemyStatus;
    public GameObject life;
    int maxHP;

	// Use this for initialization
	void Start () {
        camera = Camera.main;
        enemyStatus = gameObject.transform.root.GetComponent<EnemyStatus>();
        maxHP = enemyStatus.HP;
	}
	
	// Update is called once per frame
	void Update () {
        float xpos = camera.transform.position.x;
        float zpos = camera.transform.position.z;
        transform.LookAt(new Vector3(xpos, transform.position.y, zpos));
        if(enemyStatus.HP>0)life.transform.localScale = new Vector3(15.5f * enemyStatus.HP / maxHP, 1, 1);
        else life.transform.localScale = new Vector3(0, 1, 1);
	}
}
