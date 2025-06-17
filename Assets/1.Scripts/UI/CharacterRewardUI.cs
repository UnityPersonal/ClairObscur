using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRewardUI : MonoBehaviour
{
    [SerializeField] TMP_Text characterName;
    [SerializeField] Slider expSlider;
    [SerializeField] private TMP_Text currentExp;
    [SerializeField] private TMP_Text maxExp;
    [SerializeField] private TMP_Text levelText;
    
    public IEnumerator AnimateCharacterReward(BattleCharacter character)
    {
        // 여기서는 실제로 캐릭터의 레벨을 업데이트 하지 않는다.
        // 업데이트는 UI 애니메이션 완료되고 나면 처리된다.
        
        characterName.text = character.CharacterName;
        int level = character.Status.Level;
        levelText.text = $"{character.Status.Level}";

        int currentExpValue = character.Status.CurrentExp;
        int maxExpValue = character.Status.ExpToNextLevel;
        
        currentExp.text = currentExpValue.ToString();
        maxExp.text = maxExpValue.ToString();
        
        expSlider.value = (float)currentExpValue / maxExpValue;
        
        // Animate the exp slider
        float targetValue = (float)(currentExpValue) / maxExpValue;
        expSlider.value = Mathf.Lerp(expSlider.value, targetValue, 0.5f);
        
        // Update the current exp text
        currentExp.text = (currentExpValue).ToString();
        yield break;
    }
}
