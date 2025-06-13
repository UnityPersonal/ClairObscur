using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class WorldPlayer : WorldCharacter
{
    [SerializeField] private CinemachineCamera virtualCamera;
    
    CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 45f;

    protected override void Awake()
    {
        base.Awake();
        characterController = GetComponent<CharacterController>();
        
    }

    protected override void UpdateMovement()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 forward = virtualCamera.transform.forward;
        forward = new Vector3(forward.x , 0 , forward.z); // 수평면에서만 이동
        Vector3 right = Vector3.Cross(Vector3.up, forward ).normalized;
        
        if(movementInput.magnitude < 0.1f)
        {
            animator.SetFloat("MoveSpeed", 0f);
            return; // 이동 입력이 없으면 애니메이션을 멈추고 리턴
        }
        
        Vector3 moveDirection = forward * movementInput.y + right * movementInput.x;
        moveDirection = moveDirection.normalized;
        
        transform.forward = Vector3.Slerp(transform.forward , moveDirection , Time.deltaTime * rotateSpeed);
        Vector3 move = transform.forward * (moveSpeed * Time.deltaTime);
        characterController.Move(move);
        
        animator.SetFloat("MoveSpeed", movementInput.magnitude);
        
    }
    
    private void UpdateLookMovement()
    {
        Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    protected override void Update()
    {
        base.Update();
        UpdateLookMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.layer & LayerMask.NameToLayer("Monster")) != 0)
        {
            // 배틀씬으로 넘어가는 로직 구현
            Debug.Log("Battle Trigger Entered");
            List<BattleCharacter> battleCharacters = new List<BattleCharacter>();
            GameManager.Instance.StartBattle(battleCharacters);
        }
    }
    

}
