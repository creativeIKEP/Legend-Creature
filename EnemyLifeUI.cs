using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeUI : MonoBehaviour {
    Camera camera;
    EnemyStatus enemyStatus;
    public GameObject life;

    public GameObject bossLife;
    public Text bossName;
    int maxHP;
    bool boss = false;

	// Use this for initialization
	void Start () {
        camera = Camera.main;
        enemyStatus = gameObject.transform.root.GetComponent<EnemyStatus>();
        maxHP = enemyStatus.HP;

        if (gameObject.transform.root.tag == "Boss") { 
            boss = true;
            bossName.text = enemyStatus.name;
            int childCount = gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++){
                Transform g=gameObject.transform.GetChild(i);
                g.gameObject.SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!boss)
        {
            float xpos = camera.transform.position.x;
            float zpos = camera.transform.position.z;
            transform.LookAt(new Vector3(xpos, transform.position.y, zpos));
            if (enemyStatus.HP > 0) life.transform.localScale = new Vector3(15.5f * enemyStatus.HP / maxHP, 1, 1);
            else life.transform.localScale = new Vector3(0, 1, 1);
        }
        else{
            if (enemyStatus.HP > 0) bossLife.transform.localScale = new Vector3((float)enemyStatus.HP / maxHP, 1.0f, 1.0f);
            else{
                bossLife.transform.localScale = new Vector3(0, 1, 1);  
            } 
        }
	}
}
