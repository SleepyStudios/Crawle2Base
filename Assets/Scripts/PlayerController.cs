using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour, IPunInstantiateMagicCallback {
    public float speed;
    GameObject tallGrassOverlay;

    float runModifier = 1.0f;
    Animator anim;
    Energy energyBar;

    GameObject loadingOverlay;

    void Start () {
        anim = GetComponent<Animator>();
        energyBar = GetComponent<Energy>();
        GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = gameObject.GetPhotonView().Owner.NickName;
        tallGrassOverlay = GameObject.Find("grass_overlay");
        loadingOverlay = GameObject.Find("LoadingOverlay");
    }

    void Update () {
        // Sync everyone but our local player
        if (!PhotonNetwork.LocalPlayer.IsLocal || loadingOverlay.activeSelf) return;

        // Horizontal movement
        int xUnitSpeed = (int)Input.GetAxis("Horizontal");
        float xSpeed = (float)xUnitSpeed * speed * runModifier * Time.deltaTime;

        anim.SetFloat("xSpeed", xSpeed);

        // Vertical movement
        int yUnitSpeed = (int)Input.GetAxis("Vertical");
        float ySpeed = (float)yUnitSpeed * speed * runModifier * Time.deltaTime;

        anim.SetFloat("ySpeed", ySpeed);

        // Double speed if running
        if (energyBar.running) {
            runModifier = 2.0f;
            anim.speed = 2.0f;
        } else {
            runModifier = 1.0f;
            anim.speed = 1.0f;
        }

        // Magnitude to make diagonal movements the same speed as vertical and horizontal
        float magnitude = 1.0f;
        if (!(xUnitSpeed == 0 && yUnitSpeed == 0)) {
            magnitude = Mathf.Sqrt(Mathf.Pow(xUnitSpeed, 2) + Mathf.Pow(yUnitSpeed, 2));
        } else {
            magnitude = 1.0f;
        }

        transform.Translate(xSpeed / magnitude, ySpeed / magnitude, 0.0f);
    }

    void OnTriggerStay2D(Collider2D collisionInfo) {
        if (collisionInfo.gameObject.tag == "TallGrass") {
            tallGrassOverlay.GetComponent<SpriteRenderer>().enabled = true;
            tallGrassOverlay.transform.position = this.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collisionInfo) {
        if(collisionInfo.gameObject.tag == "WorldItem" && PhotonNetwork.LocalPlayer.IsLocal) {
            // Try to pick it up
            WorldItem worldItem = collisionInfo.gameObject.GetComponent<WorldItem>();
            if(worldItem.canPickUp) GameController.instance.inventory.PickupItem(worldItem);
        }
    }

    void OnTriggerExit2D(Collider2D collisionInfo) {
        if (collisionInfo.gameObject.tag == "TallGrass") {
            tallGrassOverlay.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        // Set the associated PUN player to this gameobject
        info.Sender.TagObject = gameObject;
    }
}
