using TMPro;
using UnityEngine;

public class PlayerStatUI : MonoSingleton<PlayerStatUI>
{
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private ApBarUI apBarUI;

    public void Setup(BattlePlayer player)
    {
        characterNameText.text = player.CharacterName;
        apBarUI.SetAp(player.PlayerStat.currentAP);
    }
}
