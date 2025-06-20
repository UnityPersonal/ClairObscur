public enum DamageType
{
    Physical,
    Light,
    Dark,
    Void,
    Fire,
    Ice,
    Earth,
    Lightning,
}

public class DamageEffector : StatusEffector
{
    public DamageType damageType;
}