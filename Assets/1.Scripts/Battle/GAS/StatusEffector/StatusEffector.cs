using System;
using UnityEngine;

public class StatusEffector
{
    public string EffectorName;
    public StatusEffectorType EffectorType;
    public Sprite EffectorIcon;
    public int EffectorValue { get; set; } = 0;

    public virtual void Initialize(BattleCharacter owner)
    {
        // 초기화 로직을 여기에 작성합니다.
    }
}