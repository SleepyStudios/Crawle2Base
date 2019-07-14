using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ItemSlot : MonoBehaviour {
    public Item item;
    GameObject iconComponent;

    public void SetItem (Item item) {
        this.item = item;
        iconComponent = Instantiate(Resources.Load("Items/ItemSlotIcon") as GameObject, transform.position, Quaternion.identity);
        iconComponent.transform.SetParent(transform);
        iconComponent.GetComponent<Image>().sprite = item.iconSprite;
        iconComponent.GetComponent<Button>().onClick.AddListener(RemoveItem);
    }

    public void RemoveItem() {
        if (item == null) return;

        GameObject localPlayer = (GameObject) PhotonNetwork.LocalPlayer.TagObject;
        Vector3 position = localPlayer.transform.position;
        GameController.instance.DropItem(position, item.name);

        item = null;
        Destroy(iconComponent);
    }

    public bool HasItem() {
        // Icon component = has an item
        return transform.childCount > 0;
    }
}
