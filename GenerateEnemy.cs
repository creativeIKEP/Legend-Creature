using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateEnemy : MonoBehaviour {
    public GameObject[] enemys;
    GameObject[] enemysContrl;
    bool isAppear = false;

	// Use this for initialization
	void Start () {
        enemysContrl = new GameObject[enemys.Length];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(!isAppear){
                for (int i = 0; i < enemys.Length; i++)
                {
                    createEnemy(i);
                }
            }
            else{
                for (int i = 0; i < enemys.Length; i++){
                    if (enemysContrl[i] != null) enemysContrl[i].GetComponent<EnemyCtrl>().RestartEnemy();
                    else createEnemy(i);
                }
            }
            isAppear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < enemys.Length; i++)
            {
                if (enemysContrl[i] != null)enemysContrl[i].GetComponent<EnemyCtrl>().StopEnemy();
            }
        }
    }

    void createEnemy(int i){
        GameObject firstPos = new GameObject();
        float x = Random.Range(transform.position.x - 10.0f, transform.position.x + 10.0f);
        float z = Random.Range(transform.position.z - 10.0f, transform.position.z + 10.0f);
        firstPos.transform.position = new Vector3(x, 0, z);
        enemys[i].GetComponent<EnemyCtrl>().firstPoint = firstPos.transform;
        float y = Terrain.activeTerrain.terrainData.GetInterpolatedHeight(x / Terrain.activeTerrain.terrainData.size.x, z / Terrain.activeTerrain.terrainData.size.z);  //get terrain's height with x and z pos

        enemysContrl[i] = Instantiate(enemys[i], new Vector3(x, y, z), Quaternion.identity);
    }
}
