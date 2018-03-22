public class DoubleCoinPowerUp : PowerUp {

    public override void DestroyPowerUp()
    {
        AudioCenter.Instance.PlaySound("PowerUp");
        gameObject.SetActive(false);
    }
}
