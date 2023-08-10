using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Interact : MonoBehaviour {
    Transform cam;
    [SerializeField] LayerMask layer;
    [SerializeField] TextMeshProUGUI itemText;
    InvSystem inventorySys;

    void Start() {
        cam = Camera.main.transform;
        inventorySys = GetComponent<InvSystem>();
    }

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 3, layer)) {
            if (!hit.collider.GetComponent<Object>()) {
                return;
            }
            itemText.text = $"Press F to pick up {hit.collider.GetComponent<Object>().stats.itemName}";
            if (Input.GetKeyDown(KeyCode.F)) {
                Debug.Log(hit.collider.GetComponent<Object>());
                inventorySys.PickUpItem(hit.collider.GetComponent<Object>());
            }
        } else {
            itemText.text = string.Empty;
        }
    }
}
