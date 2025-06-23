using System;
using UnityEngine;

/// <summary>
/// 상태효과의 기본 클래스입니다. 각 Effector는 이 클래스를 상속받고,
/// 필요한 타이밍 이벤트에 콜백을 등록합니다.
/// </summary>
[CreateAssetMenu(fileName = "StatusEffector", menuName = "GAS/StatusEffector")]
public class StatusEffectorAsset : ScriptableObject
{
    public string EffectorName;
    public Sprite EffectorIcon;
    
    public string Description;

    public StatusEffector CreateEffector(BattleCharacter character)
    {
        StatusEffector effector = new StatusEffector();
        effector.EffectorName = EffectorName.ToLower();
        effector.EffectorIcon = EffectorIcon;
        effector.BindCharacter(character);
        return effector;
    }
}