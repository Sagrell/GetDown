
public class MagnetPowerUp : PowerUp
{

    public override void DestroyPowerUp()
    {
        AudioCenter.PlaySound(AudioCenter.shieldUpSoundId);
        gameObject.transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
