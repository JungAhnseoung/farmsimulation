using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [HideInInspector] public GameObject itemDescription;

    public void RemoveItemDescription()
    {
        if (itemDescription != null) Destroy(itemDescription);
    }

    private void Update()
    {
       if(Input.GetKey(KeyCode.Escape))
        {
            RemoveItemDescription();
        }
    }
}
