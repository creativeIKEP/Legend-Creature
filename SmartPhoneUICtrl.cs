using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartPhoneUICtrl : MonoBehaviour {
    PlayerMove playerMove;
    PlayerAttack playerAttack;
    InputAttack.InputAttackCheacck inputAttackCheacck;
    Vector2 prevPosition;
    Vector2 delta = Vector2.zero;
    bool moved = false;

	// Use this for initialization
	void Start () {
        playerMove = FindObjectOfType<PlayerMove>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        inputAttackCheacck = FindObjectOfType<InputAttack.InputAttackCheacck>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!moved) { delta = Vector2.zero; }
	}

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
    }

    public Vector2 GetCursorPosition()
    {
        return Input.mousePosition;
    }


    //public void ForwardButton(){
    //    playerMove.ForwardMove();
    //}
    //public void BackButton()
    //{
    //    playerMove.BackMove();
    //}
    //public void LeftButton()
    //{
    //    playerMove.LeftMove();
    //}
    //public void RightButton()
    //{
    //    playerMove.RightMove();
    //}

    //public void ForwardButtonUp()
    //{
    //    playerMove.ForwardMoveStop();
    //}
    //public void BackButtonUp()
    //{
    //    playerMove.BackMoveStop();
    //}
    //public void LeftButtonUp()
    //{
    //    playerMove.LeftMoveStop();
    //}
    //public void RightButtonUp()
    //{
    //    playerMove.RightMoveStop();
    //}
    public void DashButton(){
        playerMove.DushMove();
    }
    public void DashMoveStop(){
        playerMove.DushMoveStop();
    }

    public void AttackButton(){
        playerAttack.Attack();
        StartCoroutine(inputAttackCheacck.Attack());
    }

    public void ScrollButtonDown(){
        prevPosition = GetCursorPosition();
    }
    public void ScrollButton()
    {
        delta = GetCursorPosition() - prevPosition;
        prevPosition = GetCursorPosition();
        moved = true;
    }
    public void ScrollButtonUp()
    {
        prevPosition = GetCursorPosition();
        moved = false;
    }

    //public void JumpButton(){
    //    playerMove.Jump();
    //}
}
