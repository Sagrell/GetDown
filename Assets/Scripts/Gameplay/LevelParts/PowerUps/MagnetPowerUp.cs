
public class MagnetPowerUp : PowerUp
{

    public override void DestroyPowerUp()
    {
        AudioCenter.PlaySound("ShieldUp");
        gameObject.SetActive(false);
    }
}
