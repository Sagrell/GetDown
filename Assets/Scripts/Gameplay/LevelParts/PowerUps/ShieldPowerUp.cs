

public class ShieldPowerUp : PowerUp
{
    public override void DestroyPowerUp()
    {
        AudioCenter.Instance.PlaySound("ShieldUp");
        gameObject.SetActive(false);
    }
}

