using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GlobalLight : MonoBehaviourPunCallbacks, IPunObservable {
    [Header("Time taken to get from night to day")]
    public float duskToDawnTime = 30.0f;
    [Header("Time taken to get from day to night")]
    public float dawnToDuskTime = 60.0f;
    [Header("How long night lasts")]
    public float nightLength = 20.0f;
    [Header("How long day lasts")]
    public float dayLength = 30.0f;

    new Light light;
    bool dayNightReverse;
    float tmrNight;
    float tmrDay;

	void Start () {
        light = GetComponent<Light>();
	}
	
	void Update () {
        if (dayNightReverse) {
            if(light.color.r >= 0.95f) {
                // Stay light for a bit
                tmrDay += Time.deltaTime;
                if (tmrDay >= dayLength) {
                    dayNightReverse = false;
                    tmrDay = 0;
                }
            } else {
                light.color += (Color.white / duskToDawnTime) * Time.deltaTime;
            }
        } else {
            if (light.color.r <= 0.05f) {
                // Stay dark for a bit
                tmrNight += Time.deltaTime;
                if(tmrNight >= nightLength) {
                    dayNightReverse = true;
                    tmrNight = 0;
                }
            } else {
                light.color -= (Color.white / dawnToDuskTime) * Time.deltaTime;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(new float[] { light.color.r, light.color.g, light.color.b, light.color.a });
        } else {
            float[] c = (float[])stream.ReceiveNext();
            light.color = new Color(c[0], c[1], c[2], c[3]);
        }
    }
}
