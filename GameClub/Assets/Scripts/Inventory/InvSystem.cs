using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InvSystem : MonoBehaviour {
    [SerializeField] public Slot[] slots = new Slot[8];
    [SerializeField] GameObject InventoryUI;
    public int selectedSlotIndex = 1;
    public Transform mapParent;

    private void Awake() {
        for (int i = 0; i < slots.Length; ++i) {
            if (slots[i].item == null) {
                for (int k = 0; k < slots[i].transform.childCount; ++k) {
                    slots[i].transform.GetChild(k).gameObject.SetActive(false);
                }
            }
        }
        selectedSlotIndex = 0;
        SetHighlightOnSlot(selectedSlotIndex, true);
    }

    void Update() {
        for (int i = 1; i <= slots.Length; ++i) {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i)) {
                SetHighlightOnSlot(i - 1, true);
                selectedSlotIndex = i - 1;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            ChangeHighlightSlot(-1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            ChangeHighlightSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            DropItem();
        }
        if (Input.GetMouseButtonDown(0)) {
            ThrowItem();
        }
    }

    void DropItem() {
        if (slots[selectedSlotIndex].item != null) {
            Vector3 playerPosition = transform.position;
            Vector3 playerForward = transform.forward;
            float dropDistance = 2f;
            Vector3 dropPosition = playerPosition + playerForward * dropDistance;
            GameObject droppedItem = Instantiate(slots[selectedSlotIndex].item.prefab, dropPosition, Quaternion.identity);
            droppedItem.transform.SetParent(mapParent);
            droppedItem.GetComponent<Object>().stats = slots[selectedSlotIndex].item;
            droppedItem.GetComponent<Object>().initialPosition = slots[selectedSlotIndex].originalPosition;
            droppedItem.SetActive(true);
            slots[selectedSlotIndex].item = null;
            slots[selectedSlotIndex].setMat();
        }
        else {
            Debug.Log("No item in the selected slot.");
        }
    }

    void ChangeHighlightSlot(int offset) {
        int newHighlightIndex = (selectedSlotIndex + offset) % slots.Length;
        if (newHighlightIndex < 0) {
            newHighlightIndex = slots.Length - 1;
        }
        SetHighlightOnSlot(selectedSlotIndex, false);
        SetHighlightOnSlot(newHighlightIndex, true);
        selectedSlotIndex = newHighlightIndex;
    }

    void SetHighlightOnSlot(int slotIndex, bool highlight) {
        for (int i = 0; i < slots.Length; ++i) {
            slots[i].highlight.gameObject.SetActive(i == slotIndex && highlight);
        }
    }

    public void PickUpItem(Object obj) {
        bool itemPicked = false;
        int slotIndex = slots[selectedSlotIndex].item != null ? Array.FindIndex(slots, slot => slot.item == null) : selectedSlotIndex;
        if (slotIndex != -1) {
            SetHighlightOnSlot(selectedSlotIndex, false);
            SetHighlightOnSlot(slotIndex, true);
            selectedSlotIndex = slotIndex;
            slots[slotIndex].item = obj.stats;
            slots[slotIndex].originalPosition = obj.initialPosition;
            Destroy(obj.gameObject);
            slots[slotIndex].setMat();
            itemPicked = true;
        }
        if (!itemPicked) {
            Debug.Log("No item in selected slot.");
        }
    }

    void ThrowItem() {
        if(slots[selectedSlotIndex].item == null) {
            Debug.Log("No item in selected slot.");
            return;
        }
        if (slots[selectedSlotIndex].item.type != Item.Types.Throwable) {
            Debug.Log("Cannot throw item.");
            return;
        }
        if (slots[selectedSlotIndex].item != null) {
            GameObject itemPrefab = slots[selectedSlotIndex].item.prefab;
            float throwForce = 10f;
            float throwAngle = Camera.main.GetComponent<CameraLook>().PitchAngle;
            float deltaY = 0.2f;
            Debug.Log(throwAngle);
            float throwDistance = 2f;
            float spinForce = 5f;

            Vector3 throwDirection = (transform.forward).normalized;
            throwDirection.y = Mathf.Sin(Mathf.Deg2Rad * throwAngle) + deltaY;
            Vector3 throwPosition = transform.position + throwDirection * throwDistance;
            
            GameObject thrownItem = Instantiate(itemPrefab, throwPosition, Quaternion.identity);
            thrownItem.transform.SetParent(mapParent);

            BoxCollider boxCollider = thrownItem.GetComponent<BoxCollider>();
            
            if (boxCollider != null) {
                // remove trigger
                boxCollider.isTrigger = false;
            } else {
                Debug.LogWarning("Box Collider component not found on the instantiated object.");
            }

            // gravity rigidbody
            Rigidbody thrownItemRigidbody = thrownItem.GetComponent<Rigidbody>();
            thrownItemRigidbody.useGravity = true;
            thrownItemRigidbody.isKinematic = false;

            if (thrownItem.GetComponent<Object>().stats.id == 4)
            {
                thrownItem.transform.localScale *= 0.6f;
            }

            // stats
            thrownItem.GetComponent<Object>().stats = slots[selectedSlotIndex].item;
            thrownItem.GetComponent<Object>().initialPosition = slots[selectedSlotIndex].originalPosition;
            
            // active
            thrownItem.SetActive(true);

            // force
            thrownItemRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            Vector3 spinDirection = UnityEngine.Random.insideUnitSphere.normalized;
            thrownItemRigidbody.AddTorque(spinDirection * spinForce, ForceMode.Impulse);
            
            // clear slot
            slots[selectedSlotIndex].item = null;
            slots[selectedSlotIndex].setMat();
        } else {
            Debug.Log("No item in the selected slot.");
        }
    }
}
