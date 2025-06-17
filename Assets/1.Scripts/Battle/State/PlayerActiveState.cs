using System.Collections;
using UnityEngine;

public class PlayerActiveState : PlayerState
{
    public override IEnumerator Execute(BattlePlayer player)
    {
        player.SwapAction(ActionDataType.Active);
        var timeline = player.ActionMap[ActionDataType.Active].timelineAsset;
        yield return player.PlayTimeline(timeline,
            player.CharacterDefaultLocation,
            player.CharacterDefaultLocation,
            false);
        player.NextState = BattlePlayer.StateType.MainMenu;
    }
}
