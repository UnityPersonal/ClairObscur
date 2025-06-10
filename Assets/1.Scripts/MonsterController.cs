using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] string monsterName = "Unknown Monster";
    [SerializeField] int attackDamageMin = 10;
    [SerializeField] int attackDamageMax = 20;
    
    [SerializeField] int hp = 100;
    
    [SerializeField] Animator animator;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnEvade(EvadeEventArgs args)
    {
        Debug.Log("MonsterController ::: OnEvade");
        animator.SetTrigger("Evade");
    }

    public void OnTakeDamage(TakeDamageEventArgs args)
    {
        Debug.Log("MonsterController ::: OnTakeDamage");
        animator.SetTrigger("Damage");
    }
}
