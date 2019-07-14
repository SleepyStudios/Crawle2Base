using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class Client : MonoBehaviourPunCallbacks, IOnEventCallback {
    public enum PhotonEventCodes {
        DropItem = 0
    }

    public GameObject cam;
    public GameObject loadingOverlay;
    bool loadingDelay;
    float tmrLoadingDelay;

    public void ConnectToMaster () {
        if (GameObject.Find("PlayerNameInput").GetComponent<Text>().text.Trim().Length == 0) return;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected...");
        PhotonNetwork.LocalPlayer.NickName = GameObject.Find("PlayerNameInput").GetComponent<Text>().text;
        SceneManager.LoadScene("Scene");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Join room failed, making one...");

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 8 };
        PhotonNetwork.CreateRoom("crawle", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        Debug.Log("Joined room...");

        // Setup the local player
        GameObject localPlayer = PhotonNetwork.Instantiate("player", new Vector3(0, 0, 0), Quaternion.identity);
        cam.GetComponent<FollowPlayer>().player = localPlayer.GetComponent<Transform>();
        loadingDelay = true;
    }

    public void OnEvent(EventData eventData) {
        byte eventCode = eventData.Code;

        if((PhotonEventCodes)eventCode == PhotonEventCodes.DropItem) {
            object[] data = (object[])eventData.CustomData;

            GameObject worldItem = PhotonNetwork.InstantiateSceneObject("Items/WorldItem", (Vector3) data[0], Quaternion.identity) as GameObject;
            worldItem.GetComponent<WorldItem>().itemName = (string) data[1];
        }
    }

    void Update () {
        // Loading delay so that everything gets loaded in
		if(loadingDelay) {
            tmrLoadingDelay += Time.deltaTime;
            if(tmrLoadingDelay >= 0.5f) {
                loadingOverlay.SetActive(false);
                loadingDelay = false;
                tmrLoadingDelay = 0;
            }
        }
    }
}
