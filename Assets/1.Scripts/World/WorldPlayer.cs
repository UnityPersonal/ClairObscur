using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class WorldPlayer : WorldCharacter
{
    [SerializeField] private CinemachineFreeLookModifier freeLookCamera;
    
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
        characterController.Move(new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime);
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
        if (other.CompareTag("BattleTrigger"))
        {
            // 배틀씬으로 넘어가는 로직 구현
            Debug.Log("Battle Trigger Entered");
            List<BattleCharacter> battleCharacters = new List<BattleCharacter>();
            GameManager.Instance.StartBattle(battleCharacters);
        }
    }
    

}
