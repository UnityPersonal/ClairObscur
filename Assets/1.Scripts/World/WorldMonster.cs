using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMonster : WorldCharacter
{
    
    public string battleScene;
    
    //[SerializeField] private string monsterKey;
    // 월드 캐릭터에는 배틀씬으로 넘어갈때 스폰할 캐릭터의 정보를 들고 있다.
    [SerializeField] private BattleMonster[] battleCharacters;
    public List<BattleMonster> BattleCharacters => battleCharacters.ToList();
    // navigation 기반으로 이동 동작 구현
    protected override void UpdateMovement()
    {
    }
    
    
}
