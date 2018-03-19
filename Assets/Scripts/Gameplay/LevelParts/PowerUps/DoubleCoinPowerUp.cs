public class DoubleCoinPowerUp : PowerUp {

    public override void DestroyPowerUp()
    {
        AudioCenter.PlaySound("ShieldUp");
        gameObject.SetActive(false);
    }
}
