using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [SerializeField] private GameObject bed = null;
    [SerializeField] private Vector3 position = new Vector3();

    private Player player;

    

    private void Awake()
    {
        bed.SetActive(false);
        player = FindObjectOfType<Player>();
    }

    private void OnMouseDown()
    {
        if(IsPlayerClose())
        {
            bed.SetActive(true);
        }
    }

    private bool IsPlayerClose()
    {
        Vector3 playerLocation = player.GetCenter();
        Vector3 chestLocation = this.transform.position;
        float distance = Vector3.Distance(playerLocation, chestLocation);
        return (distance > 3f) ? false : true;
    }

    public void SleepOnBed()
    {
        TimeManager.PassDay();
        bed.SetActive(false);
        float xCor = Mathf.Approximately(position.x, 0f) ? player.transform.position.x : position.x;
        float yCor = Mathf.Approximately(position.y, 0f) ? player.transform.position.y : position.y;
        SceneController.FadeOutLoad(SceneType.Home.ToString(), new Vector3(xCor, yCor, 0f));
    }

    public void NotSleepOnBed()
    {
        bed.SetActive(false);
    }

    
}
