public class Gold : Item
{
    private int _amount;

    public void SetAmount(int amount) => _amount = amount;

    public override void Pickup(PlayerData playerData)
    {
        playerData.Gold += _amount;
    }
}
