using UnityEngine;

public abstract class Item : MonoBehaviour, IPickupable
{
    public abstract void Pickup(PlayerData playerData);
}