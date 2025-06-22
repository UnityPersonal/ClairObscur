using System;

public partial class BattleCharacter
{
    public class CallbackEvents
    {
        public Action OnEffectorChanged;
        public Action OnStatusChanged;
        public Action OnAttributeChanged;
    }
    
    public BattleCharacterLayer EnemyLayer
    {
        get
        {
            if (CharacterLayer == BattleCharacterLayer.Player)
                return BattleCharacterLayer.Monster;
            else if (CharacterLayer == BattleCharacterLayer.Monster)
                return BattleCharacterLayer.Player;
            else
                throw new Exception("Invalid character layer");
        }
    }
    
    const float AttackDelay = 0.33f; // Delay before the monster can attack again
    const float ParryDelay = 0.16f; // Delay before the monster can attack again
}