using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    public TMP_Text nameText;

    public void Set(Item data)
    {
        if(nameText != null)
        {
            nameText.text = data.itemName;
        }
    }
}
