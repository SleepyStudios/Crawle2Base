using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviour {
    public static GameController instance;
    public Item[] items;
    public Inventory inventory;
    public List<WorldItem> worldItems;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame() {
        // Load items
        TextAsset itemsJson = Resources.Load("ItemList") as TextAsset;
        items = JsonHelper.getJsonArray<Item>(itemsJson.text);
        foreach(Item i in items) {
            i.Init();
        }
    }
	
    public Item FindItemByName(string itemName) {
        foreach(Item i in items) {
            if (i.name == itemName) return i;
        }
        return null;
    }

    public void DropItem(Vector3 position, string itemName) {
        object[] content = { position, itemName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        SendOptions sendOptions = new SendOptions { };
        PhotonNetwork.RaiseEvent((byte)Client.PhotonEventCodes.DropItem, content, raiseEventOptions, sendOptions);
    }
}
