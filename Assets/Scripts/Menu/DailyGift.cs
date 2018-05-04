using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGift : MonoBehaviour {

    public GameObject giftButton;
    public GameObject dayPref;
    public GameObject noConnection;
    public GameObject loading;
    public GameObject content;
    Animator anim;
    UserData data;
    DataManager dataManager;
    DayGift[] gifts;
    RewardDay[] days;
    float prevTimestamp;
    int time;
    bool isClaimed;
    string time_api_url = "https://desired-games.000webhostapp.com/api/get_unix_timestamp";
    string hStr;
    void Start()
    {
        hStr = LanguageManager.Instance.GetLocalizedValue("Menu", "Gift_h");
        anim = GetComponent<Animator>();
        dataManager = DataManager.Instance;
        data = dataManager.GetUserData();

        StartCoroutine("Init");
        
    }
    IEnumerator Init()
    {
        while(true)
        {
            WWW request = new WWW(time_api_url);
            yield return request;
            if (String.IsNullOrEmpty(request.error))
            {
                giftButton.GetComponent<Button>().interactable = true;
                giftButton.GetComponent<CanvasGroup>().alpha = 1f;
                time = int.Parse(request.text);
                isClaimed = data.isClaimedGift;
                prevTimestamp = data.prevTimestamp;
                int remainingHours = 24 - (time - (int)prevTimestamp) / 3600;
                if (remainingHours <= 0)
                {
                    if (isClaimed)
                    {
                        isClaimed = false;
                        data.isClaimedGift = isClaimed;
                    }
                    StartCoroutine("GiftShake");
                }
                else
                {
                    giftButton.GetComponentInChildren<Text>().text = remainingHours.ToString() + hStr;
                }
                break;
            }
            else
            {
                giftButton.GetComponent<Button>().interactable = false;
                giftButton.GetComponent<CanvasGroup>().alpha = 0.4f;
            }

            yield return new WaitForSeconds(3f);
        }
        
    }
    public void ShowGift()
    {
        anim.Play("FadeIn");
        Time.timeScale = 0;
        SwipeController.isAnimating = true;
        if (!data.isClaimedGift)
            StartCoroutine("Initialize");
        else
        {
            currentDayInARow = data.currentDayInARow;
            UpdateInfo();
        }
           

        
    }
    IEnumerator GetServerTime()
    {
        WWW request = new WWW(time_api_url);
        yield return request;

        if (String.IsNullOrEmpty(request.error))
        {
            time = int.Parse(request.text);
        }

    }
    int currentDayInARow;
    IEnumerator GiftShake()
    {
        Animator giftShake = giftButton.GetComponent<Animator>();
        while (true)
        {
            giftShake.Play("Shake");
            yield return new WaitForSeconds(2.5f);
        }
    }

    public IEnumerator Initialize()
    {
        prevTimestamp = data.prevTimestamp;

        time = -1;
        
        noConnection.SetActive(false);
        while (true)
        {
            loading.SetActive(true);
            loading.GetComponent<Animator>().Play("Loading");
            yield return StartCoroutine("GetServerTime");
            loading.SetActive(false);
            if (time == -1)
            {
                noConnection.SetActive(true);
                yield return new WaitForSecondsRealtime(3f);
            }    
            else
                break;
        }
        noConnection.SetActive(false);
        float currentTimeStamp = time;
        float differenceInHours = (currentTimeStamp - prevTimestamp) / 3600.0f;

        currentDayInARow = data.currentDayInARow;
        isClaimed = data.isClaimedGift;

        if (differenceInHours >= 24.0f)
        {
           isClaimed = false;
           currentDayInARow++;  
        } 
        if (currentDayInARow > 6 || currentDayInARow < 0 || differenceInHours >= 48.0f)
            currentDayInARow = 0;

        UpdateInfo();

        days[currentDayInARow].claim.GetComponent<Button>().onClick.AddListener(delegate { Claim(currentDayInARow); });
        prevTimestamp = currentTimeStamp;
    }
    void UpdateInfo()
    {
        if (days != null)
        {
            for (int i = 0; i < days.Length; i++)
            {
                Destroy(days[i].gameObject);
            }  
        }
        gifts = new DayGift[7];
        days = new RewardDay[7];
        isClaimed = data.isClaimedGift;
        for (int i = 0; i < days.Length; i++)
        {
            days[i] = Instantiate(dayPref, content.transform).GetComponent<RewardDay>();

            DayGift gift = new DayGift();
            gift.isCrossed = false;
            gift.isToday = false;
            gift.isClaimed = false;
            if (i < currentDayInARow)
            {
                gift.isCrossed = true;
            }
            else if (i == currentDayInARow)
            {
                gift.isToday = true;
                gift.isClaimed = isClaimed;
            }
            gift.id = i + 1;
            switch (i)
            {
                case 0:
                    gift.amount = 50;
                    break;
                case 1:
                    gift.amount = 100;
                    break;
                case 2:
                    gift.amount = 200;
                    break;
                case 3:
                    gift.amount = 400;
                    break;
                case 4:
                    gift.amount = 600;
                    break;
                case 5:
                    gift.amount = 1000;
                    break;
                case 6:
                    gift.amount = 2000;
                    break;
            }
            days[i].Initialize(gift);
            gifts[i] = gift;
        }
    }
    public void Claim(int id)
    {
        StopCoroutine("GiftShake");
        if(id==6)
        {
            GooglePlayManager.Instance.Achieve(GooglePlayManager.alwaysWithYouAchive);
        }
        AudioCenter.Instance.PlaySound("Purchase");
        data.GoldAmount += gifts[id].amount;
        UIManager.Instance.coins.text = data.GoldAmount.ToString();
        isClaimed = true;
        data.isClaimedGift = true;
        data.prevTimestamp = (int)prevTimestamp;
        data.currentDayInARow = currentDayInARow;
        dataManager.SaveUserData(data);
        UpdateInfo();
        Hide();
        StopCoroutine("Init");
        StartCoroutine("Init");
    }
    public void Hide()
    {
        anim.Play("FadeOut");
    }
    public void End()
    {
        Time.timeScale = 1;
        SwipeController.isAnimating = false;
    }
}
