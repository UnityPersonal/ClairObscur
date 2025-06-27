using UnityEngine;
using UnityEngine.UI;

public class BattleScreenUI : MonoSingleton<BattleScreenUI>
{
    [SerializeField] private HpBarUI bossHpBarUI;
    [SerializeField] Button exitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        exitButton.onClick.AddListener(() =>
        {
            // Exit the battle screen
            // This could be a call to a GameManager or SceneManager to load the previous scene
            Debug.Log("Exit button clicked. Implement exit logic here.");
            GameManager.Instance.GoToWorldScene();
        });
    }

    public void SetBossHpBar(BattleCharacter boss)
    {
        if (boss == null)
        {
            Debug.LogError("Boss character is null. Cannot set HP bar.");
            return;
        }
        
        bossHpBarUI.gameObject.SetActive(true);
        bossHpBarUI.SetUp(boss);
        bossHpBarUI.character = boss;
    }
}
