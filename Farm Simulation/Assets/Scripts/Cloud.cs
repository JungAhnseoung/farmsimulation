using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Vector3 startLocation;

    private void OnEnable()
    {
        EventHandler.MinutePass += MoveCloud;
        EventHandler.DayPass += InitCloudLocation;
    }

    private void OnDisable()
    {
        EventHandler.MinutePass -= MoveCloud;
        EventHandler.DayPass -= InitCloudLocation;
    }


    private void Start()
    {
        startLocation = transform.position;
    }

    private void MoveCloud(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        transform.Translate(Vector3.left * 0.05f);
    }

    private void InitCloudLocation(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        transform.position = startLocation;
    }
}
