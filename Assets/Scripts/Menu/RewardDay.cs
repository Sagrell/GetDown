using UnityEngine;
using UnityEngine.UI;

public class RewardDay : MonoBehaviour {

    public GameObject cross;
    public Text amount;
    public GameObject claim;
    public Text dayNumber;

    string dayStr;
    string claimedStr;
    public void Initialize(DayGift day)
    {
        dayStr = LanguageManager.Instance.GetLocalizedValue("Menu", "Gift_day");
        claimedStr = LanguageManager.Instance.GetLocalizedValue("Menu", "Gift_Claimed");
        cross.SetActive(day.isCrossed);
        dayNumber.text = dayStr + day.id.ToString();
        if (day.isToday)
        {
            claim.SetActive(true);
            if (day.isClaimed)
            {
                dayNumber.text = claimedStr;
                claim.SetActive(false);
            }
            
        }
        amount.text = day.amount.ToString();     
    }
}
