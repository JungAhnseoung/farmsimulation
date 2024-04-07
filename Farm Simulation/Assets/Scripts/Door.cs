using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject[] doorClosed;
    private bool isDoorClosed = false;
    private void OnMouseDown()
    {
        if (!isDoorClosed)
        {
            for (int i = 0; i < doorClosed.Length; i++)
            {
                doorClosed[i].gameObject.SetActive(false);
            }
            isDoorClosed = true;
        }
        else
        {
            for (int i = 0; i < doorClosed.Length; i++)
            {
                doorClosed[i].gameObject.SetActive(true);
            }
            isDoorClosed = false;
        }
    }
}
