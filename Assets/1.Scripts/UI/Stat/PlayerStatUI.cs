using TMPro;
using UnityEngine;

public class PlayerStatUI : MonoSingleton<PlayerStatUI>
{
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private ApBarUI apBarUI;

    public void Setup(BattlePlayer player)
    {
        characterNameText.text = player.CharacterName;
        var ap = player.Stat(GameStat.AP);
        apBarUI.SetAp(ap.StatValue);
    }
}
