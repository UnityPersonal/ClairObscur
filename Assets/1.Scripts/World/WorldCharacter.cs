using UnityEngine;

public abstract class WorldCharacter : MonoBehaviour
{
    // 월드 캐릭터에는 배틀씬으로 넘어갈때 스폰할 캐릭터의 정보를 들고 있다.
    [SerializeField] private BattleCharacter[] battleCharacters;
    
    // 배틀씬으로 전환시 캐릭터 정보를 넘겨준다.
    protected abstract void Movement();
    
}
