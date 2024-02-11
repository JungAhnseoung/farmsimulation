using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItemDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemDescriptionTextUp1 = null;
    [SerializeField] private TextMeshProUGUI itemDescriptionTextUp2 = null;
    [SerializeField] private TextMeshProUGUI itemDescriptionTextDown = null;

    public void SetDescriptionText(string textUp1, string textUp2, string textDown)
    {
        itemDescriptionTextUp1.text = textUp1;
        itemDescriptionTextUp2.text = textUp2;
        itemDescriptionTextDown.text = textDown;
    }
}
