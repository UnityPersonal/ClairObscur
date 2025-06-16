using UnityEngine;
using System.Collections;

public abstract class PlayerState : MonoBehaviour
{
    public abstract BattlePlayer.ActionType StateType { get; }
    public abstract IEnumerator Execute(BattlePlayer player);
}
