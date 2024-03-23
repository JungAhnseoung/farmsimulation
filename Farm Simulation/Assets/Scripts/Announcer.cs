using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Announcer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI announcementText = null;
    [SerializeField] private float fadeSpeed = 0;
    
    private void OnEnable()
    {
        EventHandler.DayPass += ShowIncomeAnnouncement;
    }

    private void OnDisable()
    {
        EventHandler.DayPass -= ShowIncomeAnnouncement;
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
            announcementText.text = ("Your goods in the chest were sold!\n" + "You got " + ChestUI.coinTotal.ToString() + " coin in your bag");
            ChestUI.coinTotal = 0;
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeOut()
    {
        float startAlpha = announcementText.alpha;
        while(announcementText.alpha > 0)
        {
            float newAlpha = announcementText.alpha - (1f / fadeSpeed) * Time.deltaTime;
            newAlpha = Mathf.Max(newAlpha, 0);
            announcementText.alpha = newAlpha;
            yield return null;
        }
        announcementText.gameObject.SetActive(false);
        announcementText.alpha = startAlpha;
    }
}
