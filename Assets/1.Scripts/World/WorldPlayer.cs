using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class WorldPlayer : WorldCharacter
{
    public static WorldPlayer player;
    [SerializeField] private CinemachineCamera virtualCamera;
    
    CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 45f;

    [FormerlySerializedAs("characterSelectMenu")] [SerializeField] private PlayerSelectMenu playerSelectMenu;

    protected override void Awake()
    {
        base.Awake();
        characterController = GetComponent<CharacterController>();
        WorldPlayer.player = this;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        
        if(!characterController.isGrounded)
            move += Physics.gravity * Time.deltaTime;
        characterController.Move(move);
        animator.SetFloat("MoveSpeed", movementInput.magnitude);
        
    }
    
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerSelectMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            // 배틀씬으로 넘어가는 로직 구현
            Debug.Log("Battle Trigger Entered");
            var monster = other.GetComponent<WorldMonster>();
            GameUser.Instance.enemySamples = monster.BattleCharacters;
            List<BattleCharacter> battleCharacters = new List<BattleCharacter>();
            GameManager.Instance.StartBattle(battleCharacters);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            Debug.Log("Battle Trigger Entered");
            var monster = other.GetComponent<WorldMonster>();
            GameUser.Instance.enemySamples = monster.BattleCharacters;
            List<BattleCharacter> battleCharacters = new List<BattleCharacter>();
            GameManager.Instance.StartBossBattle(battleCharacters);
        }

    }
    

}
