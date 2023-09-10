using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Slot : MonoBehaviour {
    public Item item;
    RawImage icon;
    public RawImage highlight;
    public Vector3 originalPosition;
    TextMeshProUGUI displayName;

    public void setMat() {
        for (int i = 0; i < transform.childCount; ++i) {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        icon = GetComponentInChildren<RawImage>();
        displayName = GetComponentInChildren<TextMeshProUGUI>();
        if (item != null) {
            icon.texture = item.icon;
            displayName.text = $"{item.itemName}";
        } else {
            icon.texture = null;
            displayName.text = "";
        }
    }
}
