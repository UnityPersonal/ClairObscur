using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetSelectState : PlayerState
{
    public override IEnumerator Execute(BattlePlayer player)
    {
        var enemyList = BattleManager.Instance.CharacterGroup[BattleCharacterType.Enemy];
        List<BattleCharacter> alivedEnemyList = new List<BattleCharacter>();        
        foreach (var character in enemyList)
        {
            if (character.IsDead == false)
            {
                alivedEnemyList.Add(character);
            }
        }

        bool isTargetSelected = false;
        int currentIndex = 0;
        alivedEnemyList[currentIndex].OnFocusIn();
        while (isTargetSelected == false)
        {
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
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                alivedEnemyList[currentIndex].OnFocusOut();
                isTargetSelected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                alivedEnemyList[currentIndex].OnFocusOut();
                break;
            }
            yield return null;
        }
        
        //targetCharacter = alivedEnemyList[Random.Range(0, alivedEnemyList.Count)];
        if (isTargetSelected)
        {
            player.PlayerTargetCharacter = alivedEnemyList[currentIndex];
            player.NextAction = BattlePlayer.ActionType.Attack;
        }
        else
        {
            player.NextAction = player.PreviousActionType;
        }
        yield return new WaitForSeconds(0.5f);
    }
}
