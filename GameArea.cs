using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//private void OnCollisionEnter(Collision collision)
	//{
 //       Debug.Log("AlertGameAreaLimit");
 //       StartCoroutine(AlertGameAreaLimit());
	//}

	public IEnumerator AlertGameAreaLimit(){
        UICtrl uiCtrl = FindObjectOfType<UICtrl>();
        uiCtrl.AlertTextSet("この先はゲームエリア外です");
        yield return new WaitForSeconds(3.0f);
        uiCtrl.AlertTextOff();
    }
}
