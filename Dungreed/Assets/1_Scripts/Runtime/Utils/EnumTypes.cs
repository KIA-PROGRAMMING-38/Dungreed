namespace EnumTypes
{
    public enum EnemyAIState
    {
        Die,
        Idle,
        Attack,
        Return,
        Trace,
        Patrol,
    }
    public enum CharacterState
    {
        Idle,
        Run,
        Jump,
        Dash,
        Fall,
        OnewayFall,
        Knockback,
    }


    public enum WeaponEquipType
    {
        OneHand,
        TwoHand,
    }
    public enum WeaponAttackType
    {
        Melee,
        Ranged,
    }

    public enum WeaponRarity
    {
        Normal,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public enum ProjectileType
    {
        Normal,
        Through,
    }

}