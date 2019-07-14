using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public ItemSlot[] slots;

    void Awake() {
        slots = GetComponentsInChildren<ItemSlot>();
    }

    public int FindFreeSlot() {
        for (int i=0; i<slots.Length; i++) {
            if (!slots[i].HasItem()) return i;
        }
        return -1;
    }

    public void PickupItem(WorldItem worldItem) {
        int freeSlot = FindFreeSlot();
        if (freeSlot == -1) return;

        slots[freeSlot].SetItem(worldItem.GetItem());
        worldItem.Pickup();
    }
}
