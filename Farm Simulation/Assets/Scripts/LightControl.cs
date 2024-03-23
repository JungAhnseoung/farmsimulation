using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    [SerializeField] private LightDay lightDay;
    private Light2D light2D;
    private Dictionary<string, float> lightStrengthDict = new Dictionary<string, float>();
    
    [SerializeField] private bool isFlash = false;
    [SerializeField][Range(0f, 1f)] private float flashBrightness;
    private float brightness;
    private float flashTime = 0f;
    [SerializeField][Range(0f, 0.2f)] private float minFlash;
    [SerializeField][Range(0f, 0.2f)] private float maxFlash;
    private Coroutine darkCoroutine;

    private void OnEnable()
    {
        EventHandler.AfterLoad += AfterLoad;
        EventHandler.HourPass += HourPass;
    }

    private void OnDisable()
    {
        EventHandler.AfterLoad -= AfterLoad;
        EventHandler.HourPass -= HourPass;
    }

    private void Awake()
    {
        light2D = GetComponentInChildren<Light2D>();

        foreach(LightStrength lightStrength in lightDay.lightStrengths) lightStrengthDict.Add(lightStrength.season.ToString() + lightStrength.time.ToString(), lightStrength.brightness);
    }

    private void Update()
    {
        if (isFlash) flashTime -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(flashTime <= 0f && isFlash)
        {
            light2D.intensity = Random.Range(brightness, brightness + (brightness * flashBrightness));
            flashTime = Random.Range(minFlash, maxFlash);
        }
        else
        {
            light2D.intensity = brightness;
        }
    }
    private void AfterLoad()
    {
        Season season = TimeManager.GetSeason();
        int hour = TimeManager.GetTime().Hours;
        SetLightBrightness(season, hour, false);
    }

    private void HourPass(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        SetLightBrightness(season, hour, false);
    }

    private void SetLightBrightness(Season season, int hour, bool faint)
    {
        int i = 0;
        while(i<=23)
        {
            if(lightStrengthDict.TryGetValue(season.ToString() + hour.ToString(), out float brightnessTo))
            {
                if(faint)
                {
                    if(darkCoroutine != null)
                    {
                        StopCoroutine(darkCoroutine);
                    }
                    darkCoroutine = StartCoroutine(DarkCoroutine(brightnessTo));
                }
                else
                {
                    brightness = brightnessTo;
                }
                break;
            }
            i++;
            hour--;
            if (hour < 0) hour = 23;
        }

    }

    private IEnumerator DarkCoroutine(float brightnessTo)
    {
        float speed = Mathf.Abs((brightness - brightnessTo) / 5f);
        while(!Mathf.Approximately(brightness, brightnessTo))
        {
            brightness = Mathf.MoveTowards(brightness, brightnessTo, speed * Time.deltaTime);
            yield return null;
        }

        brightness = brightnessTo;
    }

}
