using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {
    PlayerCtrlSetting playerCtrlSetting;
    PlayerMove playerMove;
    FollowCamera followCamera;
    bool attack = false;

	// Use this for initialization
	void Start () {
        playerCtrlSetting = GetComponent<PlayerCtrlSetting>();
        playerMove = GetComponent<PlayerMove>();
        followCamera = FindObjectOfType<FollowCamera>();
	}
	
	// Update is called once per frame
	void Update () {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f) { followCamera.Zoom(scroll); }

        if(Input.GetKey(playerCtrlSetting.forward)){
            playerMove.ForwardMove();
        }
        if (Input.GetKeyUp(playerCtrlSetting.forward))
        {
            playerMove.ForwardMoveStop();
        }
        if (Input.GetKey(playerCtrlSetting.back))
        {
            playerMove.BackMove();
        }
        if (Input.GetKeyUp(playerCtrlSetting.back))
        {
            playerMove.BackMoveStop();
        }
        if (Input.GetKey(playerCtrlSetting.left))
        {
            playerMove.LeftMove();
        }
        if (Input.GetKeyUp(playerCtrlSetting.left))
        {
            playerMove.LeftMoveStop();
        }
        if (Input.GetKey(playerCtrlSetting.right))
        {
            playerMove.RightMove();
        }
        if (Input.GetKeyUp(playerCtrlSetting.right))
        {
            playerMove.RightMoveStop();
        }
        if (Input.GetKey(playerCtrlSetting.dush))
        {
            playerMove.DushMove();
        }
        if (Input.GetKeyUp(playerCtrlSetting.dush))
        {
            playerMove.DushMoveStop();
        }

        if(Input.GetKeyDown(playerCtrlSetting.jump)){
            playerMove.Jump();
        }
        if (Input.GetKeyUp(playerCtrlSetting.jump))
        {
            playerMove.JumpStop();
        }
	}
}
