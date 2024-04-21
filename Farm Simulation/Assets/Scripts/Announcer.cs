using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Announcer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI announcementText = null;
    [SerializeField] private float fadeSpeed = 0;
    
    private void OnEnable()
    {
        EventHandler.DayPass += ShowIncomeAnnouncement;
        EventHandler.NotEnoughCoin += ShowNotEnoughCoinAnnouncement;
        EventHandler.NotEnoughStamina += ShowNotEnoughStamina;
        EventHandler.LateNightEvent += ShowLateNight;
        EventHandler.LostCoinEvent += ShowLostCoin;
    }

    private void OnDisable()
    {
        EventHandler.DayPass -= ShowIncomeAnnouncement;
        EventHandler.NotEnoughCoin -= ShowNotEnoughCoinAnnouncement;
        EventHandler.NotEnoughStamina -= ShowNotEnoughStamina;
        EventHandler.LateNightEvent -= ShowLateNight;
        EventHandler.LostCoinEvent -= ShowLostCoin;
    }

    void Awake()
    {
        announcementText.gameObject.SetActive(false);
    }

    private void ShowIncomeAnnouncement(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        if(ChestUI.coinTotal != 0)
        {
            announcementText.gameObject.SetActive(true);
            announcementText.text = ("You got " + ChestUI.coinTotal.ToString() + " coin in your bag from the chest");
            ChestUI.coinTotal = 0;
            StartCoroutine(FadeOut());
        }
    }

    private void ShowNotEnoughCoinAnnouncement()
    {
        announcementText.gameObject.SetActive(true);
        announcementText.text = ("There is not enough coin");
        StartCoroutine(FadeOut());
    }

    private void ShowNotEnoughStamina()
    {
        announcementText.gameObject.SetActive(true);
        announcementText.text = ("There is not enough stamina");
        StartCoroutine(FadeOut());
    }

    private void ShowLostCoin(int coin)
    {
        if(coin != 0)
        {
            announcementText.gameObject.SetActive(true);
            announcementText.text = announcementText.text +  ("\nYou lost " + coin +" gold");
            StartCoroutine(FadeOut());
        }
    }

    private void ShowLateNight()
    {
        announcementText.gameObject.SetActive(true);
        announcementText.text = ("It's getting Late...");
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        announcementText.alpha = 1f;
        while (announcementText.alpha > 0)
        {
            float newAlpha = announcementText.alpha - (1f / fadeSpeed) * Time.deltaTime;
            announcementText.alpha = newAlpha;
            yield return null;
        }
        announcementText.gameObject.SetActive(false);
    }
}
