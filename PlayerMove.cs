using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : MonoBehaviour {

    // 重力値.
    public  float GravityPower = 9.8f; 
    //　目的地についたとみなす停止距離.
    const float StoppingDistance = 0.6f;
    
    // 現在の移動速度.
    Vector3 velocity = Vector3.zero; 
    // キャラクターコントローラーのキャッシュ.
    CharacterController characterController; 

    // 向きを強制的に指示するか.
    bool forceRotate = false;
    
    // 強制的に向かせたい方向.
    Vector3 forceRoateDirection;

    Vector3 direction = Vector3.zero;


    bool joyStick = false;
    bool forward = false;
    bool back = false;
    bool left = false;
    bool right = false;
    bool dush = false;
    bool swim = false;
    bool jump = false;
    bool isOnGround = false;

    Animator anim;
    PlayerStatus playerStatus;
    EnemyaAttackArea enemyaAttackArea;
    PlayerAttackArea playerAttackArea;

    
    // 移動速度.
    public float walkSpeed = 7.0f;
    public float walkSpeedDiff;
    float walkSpeedCash;
    
    // 回転速度.
    public float rotationSpeed = 360.0f;
    public float jumpPower = 6.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        walkSpeedDiff = walkSpeed;
        walkSpeedCash = walkSpeedDiff;
        anim = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
        enemyaAttackArea = FindObjectOfType<EnemyaAttackArea>();
        playerAttackArea = GetComponentInChildren<PlayerAttackArea>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnGround) { playerStatus.SetWalkState(); }
        if (swim) { playerStatus.SetWalkState(); }
        // 移動速度velocityを更新する
        if (characterController.isGrounded)
        {
            direction = Vector3.zero;
            Transform m_Cam = Camera.main.transform;
            float yAngle = m_Cam.transform.rotation.eulerAngles.y;
            m_Cam.rotation = Quaternion.Euler(0, yAngle, 0);

            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float v = CrossPlatformInputManager.GetAxisRaw("Vertical");
            if (h != 0.0f || v != 0.0f)
            {
                direction = m_Cam.forward.normalized * v + m_Cam.right.normalized * h;
                anim.SetFloat("Forward", 0.5f);
                joyStick = true;
            }
            else { 
                joyStick = false;
                anim.SetFloat("Forward", 0);
            }

            if(forward){
                direction += m_Cam.forward.normalized;
                anim.SetFloat("Forward", 0.5f);
            }
            if(back){
                direction -= m_Cam.forward.normalized;
                anim.SetFloat("Forward", 0.5f);
            }
            if (left)
            {
                direction -= m_Cam.right.normalized;
                anim.SetFloat("Forward", 0.5f);
            }
            if (right)
            {
                direction += m_Cam.right.normalized;
                anim.SetFloat("Forward", 0.5f);
            }
            if (dush)
            {
                walkSpeed = walkSpeedDiff*2;
                if (forward || back || right || left || joyStick) { anim.SetFloat("Forward", 1.0f); }
            }
            else { walkSpeed = walkSpeedDiff; }

            // 現在の速度を退避.
            Vector3 currentVelocity = velocity;

            velocity = direction * walkSpeed;
            // スムーズに補間.
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            velocity.y = 0;


            if (!forceRotate)
            {
                // 向きを行きたい方向に向ける.
                if (velocity.magnitude > 0.1f  && direction!=Vector3.zero)
                { // 移動してなかったら向きは更新しない.
                    Quaternion characterTargetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                // 強制向き指定.
                forceRoateDirection = direction;
                Quaternion characterTargetRotation = Quaternion.LookRotation(forceRoateDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // 接地していたら思いっきり地面に押し付ける.
        // (UnityのCharactorControllerの特性のため）
        Vector3 snapGround = Vector3.zero;
        if (characterController.isGrounded && !jump)
            snapGround = Vector3.down;

        // 重力.
        velocity += Vector3.down * GravityPower * Time.deltaTime;
        if (characterController.isGrounded || swim)anim.SetBool("jump", false);
        if (jump) { velocity += Vector3.up * jumpPower; jump = false; anim.SetBool("jump", true); }

        // CharacterControllerを使って動かす.
        characterController.Move(velocity * Time.deltaTime + snapGround);

        // 強制的に向きを変えるを解除.
        if (forceRotate && Vector3.Dot(transform.forward, forceRoateDirection) > 0.99f)
            forceRotate = false;
    }

    public void ForwardMove(){
        if (!isOnGround && !swim) { return; }
        forward = true;
    }

    public void BackMove()
    {
        if (!isOnGround && !swim) { return; }
        back = true;
    }

    public void LeftMove()
    {
        if (!isOnGround && !swim) { return; }
        left = true;
    }

    public void RightMove()
    {
        if (!isOnGround && !swim) { return; }
        right = true;
    }

    public void DushMove()
    {
        if (!isOnGround && !swim) { return; }
        dush = true;
    }

    public void ForwardMoveStop(){
        forward = false;
        anim.SetFloat("Forward", 0);
    }
    public void BackMoveStop()
    {
        back = false;
        anim.SetFloat("Forward", 0);
    }
    public void LeftMoveStop()
    {
        left = false;
        anim.SetFloat("Forward", 0);
    }
    public void RightMoveStop()
    {
        right = false;
        anim.SetFloat("Forward", 0);
    }
    public void DushMoveStop()
    {
        dush = false;
    }

    public void Jump(){
        Debug.Log("jump");
        if(characterController.isGrounded && !swim)jump = true;
    }
    public void JumpStop()
    {
        Debug.Log("jump");
    }

    public void MoveStop(){
        walkSpeedDiff = 0;
    }
    public void MoveReStart(){
        walkSpeedDiff = walkSpeedCash;
        enemyaAttackArea.hitEffect.SetActive(false);
        playerAttackArea.hitParticle.SetActive(false);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isOnGround = true;
        }

        GameArea gameArea = hit.gameObject.GetComponent<GameArea>();
        if(gameArea != null && !(FindObjectOfType<UICtrl>().isAlert)){
            StartCoroutine(gameArea.AlertGameAreaLimit());
        }

        if(hit.gameObject.layer == LayerMask.NameToLayer("Water")){
            swim = true;
            anim.SetBool("swim", swim);
        }
        else if(swim){
            swim = false;
            anim.SetBool("swim", swim);
        }
    }
}
