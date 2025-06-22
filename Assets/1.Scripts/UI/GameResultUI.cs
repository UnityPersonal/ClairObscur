using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] TMP_Text gameResultText;
    [SerializeField] RectTransform gameRewardContainer;
    [SerializeField] CharacterRewardUI characterRewardUIPrefab;
    [SerializeField] Button continueButton;
    
    [SerializeField] RectTransform killLogContainer;
    [SerializeField] GameObject killLogPrefab;
    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GoToWorldScene();
        });
    }

    public void UpdateUI()
    {
        var playerGroup = BattleManager.Instance.CharacterGroup[BattleCharacterLayer.Player];
        var enemyGroup = BattleManager.Instance.CharacterGroup[BattleCharacterLayer.Monster];

        foreach (var player in playerGroup)
        {
            var characterRewardUI = Instantiate(characterRewardUIPrefab, gameRewardContainer);
            characterRewardUI.gameObject.SetActive(true);
            StartCoroutine(characterRewardUI.AnimateCharacterReward(player));
        }
        
        // todo: 킬로그에 몬스터를 넣어준다.
    }
}
