using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WorldItem : MonoBehaviourPunCallbacks, IPunObservable {
    public Item item;
    public string itemName;
    public bool canPickUp;
    float tmrPickup;

    void Start() {
        if (itemName.Length == 0) return;
        item = GameController.instance.FindItemByName(itemName);
        GetComponent<SpriteRenderer>().sprite = item.iconSprite;
    }

    void Update() {
        tmrPickup += Time.deltaTime;
        if(tmrPickup >= 0.5f) {
            canPickUp = true;
            tmrPickup = 0;
        }
    }

    public Item GetItem() {
        return item;
    }

    public void Pickup() {
        gameObject.SetActive(false);
        if(!PhotonNetwork.IsMasterClient) {
            photonView.RPC("RPC_Pickup", RpcTarget.MasterClient);
        } else {
            RPC_Pickup();
        }
    }

    [PunRPC]
    public void RPC_SetItem(string itemName) {
        this.itemName = itemName;
        item = GameController.instance.FindItemByName(itemName);
        GetComponent<SpriteRenderer>().sprite = item.iconSprite;
    }

    [PunRPC]
    public void RPC_Pickup() {
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(itemName);
        } else {
            bool wasNull = (itemName.Length == 0);
            itemName = (string)stream.ReceiveNext();
            if(wasNull) Start();
        }
    }
}
