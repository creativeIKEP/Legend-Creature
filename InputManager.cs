using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    //public bool isBuildSmartPhone = false;
	Vector2 slideStartPosition;
	Vector2 prevPosition;
	Vector2 delta = Vector2.zero;
	bool moved = false;
    //SmartPhoneUICtrl smartPhoneUICtrl;
    PlayerAttack playerAttack;
    InputAttack.InputAttackCheacck inputAttackCheacck;

	private void Start()
	{
        //smartPhoneUICtrl = FindObjectOfType<SmartPhoneUICtrl>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        inputAttackCheacck = FindObjectOfType<InputAttack.InputAttackCheacck>();
	}

	void Update()
	{
        
        // スライド開始地点.
        if (Input.GetMouseButtonDown(0))
			slideStartPosition = GetCursorPosition();
        
		
		// 画面の１割以上移動させたらスライド開始と判断する.
        if (Input.GetMouseButton(0)) {
			if (Vector2.Distance(slideStartPosition,GetCursorPosition()) >= (Screen.width * 0.1f))
				moved = true;
		}
		
		// スライド操作が終了したか.
        if (!Input.GetMouseButtonUp(0) && !Input.GetMouseButton(0))
			moved = false; // スライドは終わった.
		
		// 移動量を求める.
		if (moved)
			delta = GetCursorPosition() - prevPosition;
		else
			delta = Vector2.zero;
		
		// カーソル位置を更新.
		prevPosition = GetCursorPosition();

        if (Input.GetMouseButtonDown(1))
        {
            playerAttack.Attack();
            StartCoroutine(inputAttackCheacck.Attack());
        }
	}
	
	// クリックされたか.
	public bool Clicked()
	{
        if (!moved && Input.GetMouseButtonUp(0))
			return true;
		else
			return false;
	}	
	
	// スライド時のカーソルの移動量.
	public Vector2 GetDeltaPosition()
	{
		return delta;
	}
	
	// スライド中か.
	public bool Moved()
	{
        return moved;
        //if (!isBuildSmartPhone) { return moved; }
        //else{
        //    if (smartPhoneUICtrl.isControl && Input.touchCount >= 2) { return moved; }
        //    else if (Input.touchCount < 2 && !smartPhoneUICtrl.isControl) { return moved; }
        //    else { return false; }
        //}
	}
	
	public Vector2 GetCursorPosition()
	{
		return Input.mousePosition;
	}
}
