﻿using UnityEngine;

public interface IDamageable
{
    public void Hit(int damage, GameObject hitter = null);
}