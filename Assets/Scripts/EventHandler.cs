using UnityEngine;

public class EventHandler : MonoBehaviour {

    public delegate void UnitEventHandler(GameObject unit);

    public static event UnitEventHandler OnCoinCollect;

    public static void CoinCollect(GameObject unit)
    {
        if(OnCoinCollect != null)
        {
            OnCoinCollect(unit);
        }
    }
}
