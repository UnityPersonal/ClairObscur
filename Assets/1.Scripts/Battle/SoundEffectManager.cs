using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] private AudioClip hitSound; // 사운드 이펙트 클립 배열
    [SerializeField] private AudioClip dodgeSound; // 사운드 이펙트 클립 배열
    [SerializeField] private AudioClip parryCount; // 사운드 이펙트 클립 배열
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
