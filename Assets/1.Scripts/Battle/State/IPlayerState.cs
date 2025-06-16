using UnityEngine;
using System.Collections;

public abstract class PlayerState : MonoBehaviour
{
    public abstract IEnumerator Execute(BattlePlayer player);
}
