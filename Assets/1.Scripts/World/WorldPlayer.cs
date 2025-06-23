using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.XR;

public class WorldPlayer : WorldCharacter
{
    public static WorldPlayer player;
    [SerializeField] private CinemachineCamera virtualCamera;
    
    CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 45f;

    [SerializeField] private CharacterSelectMenu characterSelectMenu;
    [SerializeField] private SkillDatabase warriorSkillTable;
    //[SerializeField] private SkillDatabase archerSkillTable;
    public SkillDatabase GetSkillDatabase(string characterName)
    {
        switch (characterName)
        {
            case "Warrior":
                return warriorSkillTable;
            // case "Archer":
            //     return archerSkillTable;
            default:
                throw new ArgumentException($"Unknown character name: {characterName}");
        }
    }

    protected override void Awake()
    {
        base.Awake();
        characterController = GetComponent<CharacterController>();
        WorldPlayer.player = this;
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
    
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            characterSelectMenu.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.layer & LayerMask.NameToLayer("Monster")) != 0)
        {
            // 배틀씬으로 넘어가는 로직 구현
            Debug.Log("Battle Trigger Entered");
            var monster = other.GetComponent<WorldMonster>();
            GameUser.Instance.enemySamples = monster.BattleCharacters;
            List<BattleCharacter> battleCharacters = new List<BattleCharacter>();
            GameManager.Instance.StartBattle(battleCharacters);
        }
    }
    

}
