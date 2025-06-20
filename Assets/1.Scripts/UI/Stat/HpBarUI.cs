using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    public BattleCharacter character;
    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        var hp = character.Stat(GameStat.HEALTH);
        slider.value = hp.StatValue / (float)hp.MaxValue;

    }
}
