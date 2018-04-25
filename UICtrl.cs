using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICtrl : MonoBehaviour {
    public bool isAlert = false;

    Text alertText;
    Text name;
    PlayerStatus playerStatus;

	// Use this for initialization
	void Start () {
        
        alertText = GameObject.Find("AlertText").GetComponent<Text>();
        name = GameObject.Find("Name").GetComponent<Text>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        name.text = playerStatus.name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AlertTextSet(string tex)
    {
        alertText.text = tex;
        isAlert = true;
    }
    public void AlertTextOff()
    {
        alertText.text = "";
        isAlert = false;
    }

    public void HPChange(int hp, int maxHP){
        RawImage maxHPBar = GameObject.Find("BackLifeBar").GetComponent<RawImage>();
        int maxHPLength = (int)maxHPBar.rectTransform.rect.width;
        RawImage HPBar = GameObject.Find("FrontLifeBar").GetComponent<RawImage>();
        HPBar.rectTransform.sizeDelta = new Vector2(maxHPLength * hp / maxHP, 30);
    }
}
