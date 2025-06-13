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
            Debug.Log("Continue button clicked");
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            // 여기에서 월드씬으로 넘어가는 처리 필요
        });
    }

    public void UpdateUI()
    {
        var playerGroup = BattleManager.Instance.CharacterGroup[BattleCharacterType.Player];
        var enemyGroup = BattleManager.Instance.CharacterGroup[BattleCharacterType.Enemy];

        foreach (var player in playerGroup)
        {
            var characterRewardUI = Instantiate(characterRewardUIPrefab, gameRewardContainer);
            characterRewardUI.gameObject.SetActive(true);
            StartCoroutine(characterRewardUI.AnimateCharacterReward(player));
        }
        
        // 킬로그에 몬스터를 넣어준다.
    }
}
