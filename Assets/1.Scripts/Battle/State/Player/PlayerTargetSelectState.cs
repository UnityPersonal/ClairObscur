using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetSelectState : PlayerState
{
    List<BattleCharacter> alivedEnemyList = new List<BattleCharacter>();
    
    int currentIndex = 0;
    public override void Execute()
    {
        var player = character as BattlePlayer;
        
        alivedEnemyList[currentIndex].OnFocusIn();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            alivedEnemyList[currentIndex].OnFocusOut();
                
            currentIndex--;
            currentIndex = Math.Clamp(currentIndex,0 , alivedEnemyList.Count-1);
                
            alivedEnemyList[currentIndex].OnFocusIn();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            alivedEnemyList[currentIndex].OnFocusOut();
                
            currentIndex++;
            currentIndex = Math.Clamp(currentIndex,0 , alivedEnemyList.Count-1);
                
            alivedEnemyList[currentIndex].OnFocusIn();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) // target selected
        {
            player.PlayerTargetCharacter = alivedEnemyList[currentIndex];
            switch (player.CurrentAttackType)
            {
                case BattleAttackType.Normal:
                    character.SwapState("attack");
                    break;
                case BattleAttackType.Skill:
                    character.SwapState("skillactive");
                    break;
                case BattleAttackType.Jump:
                case BattleAttackType.Gradient:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            alivedEnemyList[currentIndex].OnFocusOut();
            character.SwapState("mainmenu");
        }
    }

    public override void Enter()
    {
        var enemyList = BattleManager.Instance.CharacterGroup[BattleCharacterType.Enemy];
        alivedEnemyList.Clear();        
        foreach (var character in enemyList)
        {
            if (character.IsDead == false)
            {
                alivedEnemyList.Add(character);
            }
        }
        currentIndex = 0;
        
    }

    public override void Exit()
    {
        foreach (var enemy in alivedEnemyList)
        {
            enemy.OnFocusOut();
        }
    }
}
