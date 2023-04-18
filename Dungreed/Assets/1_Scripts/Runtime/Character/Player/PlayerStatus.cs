using System;

[Serializable]
public class PlayerStatus : Status
{
    public int      DashDamage;
    public int      MaxDashCount;
    public int      CriticalChance;
    public int      CriticalDamage;
}
