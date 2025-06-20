using System;

public partial class BattleCharacter
{
    public class CallbackEvents
    {
        public Action OnEffectorChanged;
        public Action OnStatusChanged;
        public Action OnAttributeChanged;
    }
    
    const float AttackDelay = 0.33f; // Delay before the monster can attack again
    const float ParryDelay = 0.16f; // Delay before the monster can attack again
}